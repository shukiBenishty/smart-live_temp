using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BE.Models
{
    public class ExerciseInTraining
    {
        public ExerciseInTraining()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Key]
        public string Exercise { get; set; }

        public int Quantity { get; set; }

        public float Calories { get; set; }



    }
}
