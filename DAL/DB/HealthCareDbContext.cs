using BE.API.Nutritionix.Result;
using BE.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
namespace DAL.DB
{
    public class SmartLifeDbContext : DbContext
    {
        public SmartLifeDbContext() : base("SmartLifeDb")
        {
            if (!Database.Exists())
            {
                Database.Create();
               //LoadJson();
            }

        }

        public DbSet<SearchFood> SearchFood { get; set; }
        public DbSet<FoodNutritionsItem> FoodsNutritions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CurrentAccount> CurrentAccount { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //private void LoadJson()
        //{

        //    List<Muscles> muscles;
        //    using (StreamReader r = new StreamReader("‏‏file2.json"))
        //    {
        //        string json = r.ReadToEnd();
        //        muscles = JsonConvert.DeserializeObject<List<Muscles>>(json);
                
        //    }
        //    using (StreamReader r1 = new StreamReader("file.json"))
        //    {
        //        string json1 = r1.ReadToEnd();
        //        List<DataExercise> items = JsonConvert.DeserializeObject<List<DataExercise>>(json1);
        //        foreach (var item in items)
        //        {
        //            Exercise.Add(item);
        //        }
        //        SaveChanges();
        //    }

           



          
        //}

    }
}
