using BE.API.Nutritionix.Result;
using BE.Models;
using DAL;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BL
{
   public class BlRouter : IBlRouter
    {
        private IEventAggregator _eventAggregator;

        private IDalRouter dalRouter { get; }

        public BlRouter(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            dalRouter = new DalRouter(_eventAggregator);
        }

        public async Task AddMeal( Meal meal)
        {
           await dalRouter.AddMeal( meal);
        }

        public async Task<List<BodyDimmenssions>> GetBodyDimmenssions(DateTime start, DateTime end)
        {
            return await dalRouter.GetBodyDimmenssions(start, end);
        }

        public async Task<List<BodyDimmenssions>> GetBodyDimmenssions()
        {
            return await dalRouter.GetBodyDimmenssions();
        }

        public async Task<Guid> GetCurrentAccountId()
        {
            return  await dalRouter.GetCurrentAccountId();
        }

        public async Task<FoodNutritionsItem> GetFoodNutritions(string name)
        {
           return await dalRouter.GetFoodNutritions(name);
        }

        public async Task<List<FoodUnit>> GetSearchFood(string search)
        {
          return await dalRouter.GetSearchFood(search);
        }

        public async Task<BitmapImage> ImageFromUrl(string url)
        {
            return await dalRouter.ImageFromUrl(url);
        }

        public async Task MoveToGoogleLogin(IEventAggregator eventAggregator)
        {
          await  dalRouter.MoveToGoogleLogin(eventAggregator);
        }

        public async Task<bool> GetislogOn()
        {
           return await dalRouter.GetIslogOn();
        }

        public async Task AddBodyDimmenssions(BodyDimmenssions bodyDimmenssions)
        {
            await dalRouter.AddBodyDimmenssions(bodyDimmenssions);
        }

        public async Task RegisterDataSave(BodyDimmenssions bodyDimmenssions, Profile profile)
        {
           await dalRouter.RegisterDataSave(bodyDimmenssions, profile);
        }

        public async Task<List<Activity>> GetActivityListAsync(DateTime dateTime)
        {
            return await dalRouter.GetActivityList(dateTime);
        }

        public async Task<bool> GetisRegister()
        {
           return await dalRouter.GetisRegister();
        }

        public async Task SaveGoal(Goal goal)
        {
             await dalRouter.SaveGoal(goal);
        }

        public async Task<Goal> GetGoal(DateTime dateTime)
        {
            dateTime = dateTime.AddDays(-(double)dateTime.DayOfWeek);
            return await  dalRouter.GetGoal(dateTime);
        }

        public async Task<Goal> GetDefaultGoal(DateTime dateTime)
        {
            dateTime = dateTime.AddDays(-(double)dateTime.DayOfWeek);
            var  goal = new Goal();
            goal.From = dateTime;
            goal.To = dateTime.AddDays(7);
            goal.CaloriesConsume = 2400;
            goal.CarbohydrateConsume = 300;
            goal.DietaryFiberConsume = 100;
            goal.TotalfatConsume = 87;
            goal.ProteinConsume = 140;
            goal.SodiumConsume = 56;
            goal.CholesterolConsume = 83;
            await SaveGoal(goal);
            return goal;
          
        }

        public async Task<Goal> GetConsume(DateTime dateTime)
        {
            List<Meal> meals = await dalRouter.GetAllMeals(dateTime);
            if (meals == null)
            {
                meals = new List<Meal>();
            }
            Goal consume = new Goal();
            foreach (var meal in meals)
            {
                foreach (var food in meal.FoodsInMeals)
                {
                    var test1 = food;
                    var nut = await dalRouter.GetFoodNutritions(food.FoodName);
                    consume.CaloriesConsume += (nut.NfCalories != null)? (int) nut.NfCalories * food.Quantity: 0;
                    consume.CarbohydrateConsume += (nut.NfTotalCarbohydrate != null) ? (int)nut.NfTotalCarbohydrate * food.Quantity : 0;
                    consume.CholesterolConsume += (nut.NfCholesterol != null) ? (int)nut.NfCholesterol * food.Quantity : 0;
                    consume.DietaryFiberConsume += (nut.NfDietaryFiber != null) ? (int)nut.NfDietaryFiber * food.Quantity : 0;
                    consume.ProteinConsume += (nut.NfProtein != null) ? (int)nut.NfProtein * food.Quantity : 0;
                    consume.SodiumConsume += (nut.NfSodium != null) ? (int)nut.NfSodium * food.Quantity : 0;
                    consume.TotalfatConsume += (nut.NfTotalFat != null) ? (int)nut.NfTotalFat * food.Quantity : 0;
                }
                
            }
            return consume;
        }

        public async Task<Exercise> GetExercise(string name)
        {
            return await dalRouter.GetExercise(name);
        }

        public async Task<List<Exercise>> GetExercises(string search)
        {
            return await dalRouter.GetExercises(search);
        }
    }
}
