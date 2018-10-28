﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BE.API.Nutritionix.Result;
using BE.Models;
using Prism.Events;

namespace DAL
{
    public interface IDalRouter
    {
        Task<FoodNutritionsItem> GetFoodNutritions(string name);
        Task<List<FoodUnit>> GetSearchFood(string search);
        Task<BitmapImage> ImageFromUrl(string url);
      

        Task<Guid> GetCurrentAccountId();
        Task AddMeal( Meal meal);
        Task<List<BodyDimmenssions>> GetBodyDimmenssions( DateTime start, DateTime end);
        Task<List<BodyDimmenssions>> GetBodyDimmenssions();
        Task MoveToGoogleLogin(IEventAggregator eventAggregator);
        Task<bool> GetIslogOn();
        Task AddBodyDimmenssions(BodyDimmenssions bodyDimmenssions);
        Task RegisterDataSave(BodyDimmenssions bodyDimmenssions, Profile profile);
        Task<List<Activity>> GetActivityList(DateTime dateTime);
        Task<bool> GetisRegister();
        Task SaveGoal(Goal goal);
        Task<Goal> GetGoal(DateTime fromSelectedDate);
        Task<List<Meal>> GetAllMeals(DateTime dateTime);
        Task<Exercise> GetExercise(string name);
        Task<List<Exercise>> GetExercises(string search);
    }
}