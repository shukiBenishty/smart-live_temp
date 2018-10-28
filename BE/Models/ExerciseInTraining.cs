using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BE.Models
{
    public class ExerciseInTraining1
    {
        public ExerciseInTraining1()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Key]
        public string Exercise { get; set; }

        public string Quantity { get; set; }

        public float Calories { get; set; }



    }
}
