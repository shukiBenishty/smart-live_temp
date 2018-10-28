using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BE.API.Nutritionix.Result;
using BE.Models;
using DAL.API.Nutritionix;
using DAL.Tools;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Prism.Events;
using DAL.API.Google;
using DAL.LocalDb;

namespace DAL
{
    public class DalRouter : IDalRouter
    {
        private Login GetLogin { get; }
        private LocalDb.LocalDb localDb { get; }
        public DalRouter(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DbContext = new DB.SmartLifeDbContext();
            GetLogin = new Login(_eventAggregator);
        }

        private IEventAggregator _eventAggregator;

        public DB.SmartLifeDbContext DbContext { get; set; }

        public async Task<FoodNutritionsItem> GetFoodNutritions(string name)
        {
            var result = await (from food in DbContext.FoodsNutritions
                                where food.FoodName == name
                                select food).SingleOrDefaultAsync();
            if (result != null)
            {
                return result;
            }
            result = await new Nutritionix().GetFoodNutrition(name);
            return result;
        }


        public async Task<List<FoodUnit>> GetSearchFood(string search)
        {
            try
            {
                var foodList = await (from searchItem in DbContext.SearchFood
                                      where searchItem.FoodName == search
                                      select searchItem.FoodUnitList).SingleOrDefaultAsync();

                if (foodList == null)
                {
                    throw new Exception();
                }
                return foodList;
            }
            catch (Exception)
            {

                return await new Nutritionix().SearchFood(search);
            }
        }

        public async Task<BitmapImage> ImageFromUrl(string url)
        {
            return await new Download().ImageFromUrl(url);
        }

        public async Task<Guid> GetCurrentAccountId()
        {
            return await new Login(_eventAggregator).GetAccountId();
        }

        public async Task<List<BodyDimmenssions>> GetBodyDimmenssions(DateTime start, DateTime end)
        {
            var AccountId = await GetLogin.GetAccountId();
            var list = await DbContext.Accounts.Where(A => A.Id == AccountId).Select(A => A.BodyDimmenssionsList).SingleOrDefaultAsync();
            return list.Where(B => B.DateTime >= start && B.DateTime <= end).ToList();
        }

        public async Task<List<BodyDimmenssions>> GetBodyDimmenssions()
        {
            var isLogOn = await new Login(_eventAggregator).GetIsLogOn();
            if (!isLogOn)
            {
                return new List<BodyDimmenssions>();
            }
            var AccountId = await GetLogin.GetAccountId();
            return await DbContext.Accounts.Where(A => A.Id == AccountId).Select(A => A.BodyDimmenssionsList).SingleOrDefaultAsync();
        }

        public async Task AddMeal(Meal meal)
        {
            var id = await GetLogin.GetAccountId();
            var account = await DbContext.Accounts
                .Where(A => A.Id == id)
                .Include(A => A.Meals).SingleOrDefaultAsync();
            if (account != null)
            {
                account.Meals.Add(meal);
                DbContext.SaveChanges();
            }

        }

        public async Task MoveToGoogleLogin(IEventAggregator eventAggregator)
        {
            var result = await OAuth.doOAuth();
            await new Login(_eventAggregator).TryGoogleLoginAsync(result, eventAggregator);
        }

        public async Task<bool> GetIslogOn()
        {
            return await new Login(_eventAggregator).GetIsLogOn();

        }

        public async Task AddBodyDimmenssions(BodyDimmenssions bodyDimmenssions)
        {
            var id = await GetLogin.GetAccountId();
            var account = await GetAccountAsync(id);
            account.BodyDimmenssionsList.Add(bodyDimmenssions);
            DbContext.SaveChanges();
        }

        private async Task<Account> GetAccountAsync(Guid id)
        {
            return await DbContext.Accounts.Where(a => a.Id == id).Include(a => a.Meals).SingleOrDefaultAsync();
        }

        public async Task RegisterDataSave(BodyDimmenssions bodyDimmenssions, Profile profile)
        {
            var id = await GetLogin.GetAccountId();
            var account = await GetAccountAsync(id);
            account.Profile = profile;
            account.BodyDimmenssionsList.Add(bodyDimmenssions);
            DbContext.SaveChanges();
        }

        public async Task<List<Activity>> GetActivityList(DateTime dateTime)
        {
            FoodNutritionsItem nut;


            var meals = await GetAllMeals(dateTime);
            if (meals == null)
            {
                return new List<Activity>();
            }
            List<Activity> result = new List<Activity>();
            foreach (var meal in meals)
            {
                foreach (var food in meal.FoodsInMeals)
                {
                    var activity = new Activity();
                    nut = await GetFoodNutritions(food.FoodName);
                    activity.Description += nut.FoodName;
                    activity.Amount = food.Quantity;
                    activity.Calories = nut.NfCalories * food.Quantity;
                    activity.Protein = nut.NfProtein * food.Quantity;
                    activity.Unit = nut.ServingUnit;
                    activity.NetCarbs = nut.NfTotalCarbohydrate * food.Quantity;
                    result.Add(activity);

                }
            }
            return result;
        }

        public async Task<bool> GetisRegister()
        {
            return await new Login(_eventAggregator).GetisRegister();
        }

        public async Task SaveGoal(Goal goal)
        {
            var id = await GetLogin.GetAccountId();
            if (id == new Guid())
            {
                return;
            }
            var account = await DbContext.Accounts.Where(A => A.Id == id).Include(A => A.Goals).SingleOrDefaultAsync();
            var oldGoal = account.Goals.Where(G => G.From.Year == goal.From.Year && G.From.Month == goal.From.Month && G.From.Day == goal.From.Day).SingleOrDefault();
            if (oldGoal != null)
            {
                DbContext.Entry(oldGoal).CurrentValues.SetValues(goal);
            }
            else
            {
                account.Goals.Add(goal);
            }
            DbContext.SaveChanges();
        }

        public async Task<Goal> GetGoal(DateTime from)
        {
            var AccountId = await GetLogin.GetAccountId();
            if (AccountId == new Guid())
            {
                return null;
            }
            var list = await DbContext.Accounts.Where(A => A.Id == AccountId).Select(A => A.Goals).SingleOrDefaultAsync();
            var result = list.Where(G => G.From.Year == from.Year && G.From.Month == from.Month && G.From.Day == from.Day).SingleOrDefault();
            return result;
        }


        public async Task<List<Meal>> GetAllMeals(DateTime dateTime)
        {
            var id = await GetLogin.GetAccountId();
            if (id == new Guid())
            {
                return null;
            }
            var account = await DbContext.Accounts.Include(a => a.Meals).Include(a => a.Meals.Select(m => m.FoodsInMeals)).SingleOrDefaultAsync();


            return account.Meals.Where(m => m.Date.Year == dateTime.Year && m.Date.Month == dateTime.Month
                 && m.Date.Day == dateTime.Day).ToList();

        }

        public async Task<Exercise> GetExercise(string name)
        {
            return await new LocalDb.LocalDb().GetExercise(name);
        }

        public async Task<List<Exercise>> GetExercises(string search)
        {
            return await new LocalDb.LocalDb().GetExercises(search);
        }
    }
}
