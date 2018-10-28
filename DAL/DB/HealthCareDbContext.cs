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
                LoadJson();
            }

        }

        public DbSet<SearchFood> SearchFood { get; set; }
        //public DbSet<Exercise> Exercise { get; set; }
        //public DbSet<Muscles> Muscles { get; set; }
        public DbSet<FoodNutritionsItem> FoodsNutritions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CurrentAccount> CurrentAccount { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        private void LoadJson()
        {

            //List<Muscles> muscles;
            //using (StreamReader r = new StreamReader("‏‏file2.json"))
            //{
            //    string json = r.ReadToEnd();
            //    muscles = JsonConvert.DeserializeObject<List<Muscles>>(json);
            //    foreach (var item in muscles)
            //    {
            //        this.Muscles.Add(item);
            //        this.SaveChanges();
            //    }
            //}
            //using (StreamReader r1 = new StreamReader("file.json"))
            //{
            //    string json1 = r1.ReadToEnd();
            //    List<DataExercise> items = JsonConvert.DeserializeObject<List<DataExercise>>(json1);
            //    foreach (var item in items)
            //    {
            //        Exercise.Add(item);
            //    }
            //    SaveChanges();
            //}

            //List<Muscles> muscles;
            //using (StreamReader r = new StreamReader("‏‏file2.json"))
            //{
            //    string json = r.ReadToEnd();
            //    muscles = JsonConvert.DeserializeObject<List<Muscles>>(json);
            //}
            //using (StreamReader r1 = new StreamReader("file.json"))
            //{
            //    string json1 = r1.ReadToEnd();
            //    List<DataExercise> items = JsonConvert.DeserializeObject<List<DataExercise>>(json1);

            //    foreach (var item in items)
            //    {
            //        var d = new BE.Models.Exercise(item, muscles);
            //        Exercise.Add(d);
            //        SaveChanges();
            //        var test=   Exercise.Where(E => E.Id == item.Id).Include(e =>e.MainMuscles).SingleOrDefault();
            //        foreach (var _item in d.MainMuscles)
            //        {
            //            test.MainMuscles.Add(_item);
            //        }
                    
            //    }

               
              
            //}
        }

    }
}
