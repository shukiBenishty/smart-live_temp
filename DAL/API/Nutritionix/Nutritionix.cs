using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using BE.Models;
using BE.API.Nutritionix.Result;
using System.Threading;
using DAL.DB;
using DAL.Tools;
using System.Data.Entity.Migrations;

namespace DAL.API.Nutritionix
{

    public class Nutritionix : NutritionixBaseAPI
    {
        public async Task<List<FoodUnit>> SearchFood(string search)
        {
            var db = new DAL.DB.SmartLifeDbContext();
            var result = new List<FoodUnit>();
            try
            {
                var response = await Request(search, NutritionixReqType.searchFoods);
                if (response.TryGetValue("success", out object payload))
                {
                    var foodAutoComplete = SearchFoodsResult.FromJson(JsonConvert.SerializeObject(response));
                    foreach (var item in foodAutoComplete.Success.Select(Success => Success.Common).First())
                    {
                        var food = new FoodUnit()
                        {
                            Id = Guid.NewGuid(),
                            Name = item.FoodName,
                            ImageUrl = item.Photo.Thumb
                        };
                        result.Add(food);
                    }
                    SaveData(search, db, result);
                }

            }
            catch (Exception)
            {

            }
           
            return result;
        }

        private async void SaveData(string search, SmartLifeDbContext db, List<FoodUnit> result)
        {
            foreach (var item in result)
            {
                var foodNutritions = await GetFoodNutrition(item.Name);
                if (foodNutritions != null)
                {
                    item.FoodNutritionsItemID = foodNutritions.ID;
                }
            }
            var saveData = new SearchFood(search);
            saveData.FoodUnitList = result;
            db.SearchFood.AddOrUpdate(S => S.FoodName, saveData);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {

               
            }
          
        }

        public async Task<FoodNutritionsItem> GetFoodNutrition(string foodName)
        {
            try
            {
                var response = await Request(foodName, NutritionixReqType.getFoodsNutrients);
                if (response.TryGetValue("success", out object payload))
                {
                    var foodNutrition = GetFoodsNutrientsResultApi.FromJson(JsonConvert.SerializeObject(response));
                    var food = ((foodNutrition.Success.First()).Foods.First());

                    (new Thread(() =>
                    {
                        var db = new SmartLifeDbContext();
                        try
                        {
                            db.FoodsNutritions.AddOrUpdate(food);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            //Do nothing
                            if (System.Diagnostics.Debugger.IsAttached == false)
                                System.Diagnostics.Debugger.Launch();
                        }

                    })).Start();
                    return food;
                }
            }
            catch (Exception)
            {
            }
            
            return null;
        }
    }
}
