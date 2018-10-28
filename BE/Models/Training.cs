using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
