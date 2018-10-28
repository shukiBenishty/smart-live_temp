using BE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.LocalDb
{
    public class LocalDb
    {
        static private List<Muscles> muscles;
        static private List<Exercise> exercises;

        static LocalDb()
        {
            muscles = new List<Muscles>();
            exercises = new List<Exercise>();
            LoadJson();
        }
        
        static private void LoadJson()
        {
            using (StreamReader r = new StreamReader("‏‏file2.json"))
            {
                string json = r.ReadToEnd();
                muscles = new List<Muscles>();
                for (int i = 0; i < 16; i++)
                {
                    muscles.Add(null);
                }
                var temp = JsonConvert.DeserializeObject<List<Muscles>>(json);
                foreach (var item in temp)
                {
                    muscles[item.Id] = item;
                }

            }
            using (StreamReader r1 = new StreamReader("file.json"))
            {
                string json1 = r1.ReadToEnd();
                List<DataExercise> items = JsonConvert.DeserializeObject<List<DataExercise>>(json1);

                foreach (var item in items)
                {
                    exercises.Add(new Exercise(item, muscles));
                }
            }
        }

        public async Task<Exercise> GetExercise(string name)
        {
            return exercises.Where(e => e.Name == name).SingleOrDefault();
        }

        public async Task<List<Exercise>> GetExercises(string search)
        {
            return exercises.Where( e => e.Name.ToLower().Contains(search.ToLower())).ToList();
        }
    }
}
