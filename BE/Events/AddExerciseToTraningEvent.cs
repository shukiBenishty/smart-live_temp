using BE.API.Nutritionix.Result;
using BE.Models;
using Prism.Events;

namespace BE.Events
{
    public class AddExerciseToTraningEvent : PubSubEvent<AddExerciseToTraningArg>
    {
    }


    public class AddExerciseToTraningArg
    {
        public string Exercise { get; set; }
        public int Quantity { get; set; }
    }
}
