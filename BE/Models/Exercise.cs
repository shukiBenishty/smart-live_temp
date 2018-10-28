using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class DataExercise
    {
        public DataExercise()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }


        public String Name { get; set; }
        public String Description { get; set; }
        public virtual List<int> MainMuscles { get; set; }
        public virtual List<int> SecondaryMuscles { get; set; }
        public int Category { get; set; }


    }

    public enum ExerciseCategory
    {
        Abs = 10,
        Arms = 8,
        Back = 12,
        Calves = 14,
        Chest = 11,
        Legs = 9,
        Shoulders = 13
    }

    public class Muscles
    {
        public Muscles()
        {

        }
        public Muscles(Int64 i)
        {
            Id = (int)i;

        }
        // User-defined conversion from Digit to double
        public static explicit operator Muscles(Int64 i)
        {
            return new Muscles(i);
        }
        // User-defined conversion from Digit to double
        public static explicit operator Int64(Muscles i)
        {
            return i.Id;
        }
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public bool IsFront { get; set; }
    }

    public class Exercise
    {
        public Exercise(DataExercise exercise, List<Muscles> muscles)
        {
            Id = exercise.Id;
            MainMuscles = new List<Muscles>();
            SecondaryMuscles = new List<Muscles>();
            for (int i = 0; i < 16; i++)
            {
                MainMuscles.Add(null);
                SecondaryMuscles.Add(null);
            }
            foreach (var item in exercise.MainMuscles)
            {
                MainMuscles[item] = muscles[item];
            }
            foreach (var item in exercise.SecondaryMuscles)
            {
                SecondaryMuscles[item] = muscles[item];
            }
            Category = (ExerciseCategory)exercise.Category;
            Description = exercise.Description;
            Name = exercise.Name;

        }
        public Exercise()
        {

        }
        [Key]
        public Guid Id { get; set; }


        public String Name { get; set; }
        public String Description { get; set; }
        public List<Muscles> MainMuscles { get; set; }
        public List<Muscles> SecondaryMuscles { get; set; }
        public ExerciseCategory Category { get; set; }
        public float MetabolicEquivalent { get; set; }

    }







}

