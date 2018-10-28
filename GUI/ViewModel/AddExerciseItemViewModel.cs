using BE.Models;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModel.MainScreen
{
   public class AddExerciseItemViewModel : ViewModelBase
    {
        private BL.IBlRouter blRouter;
        public AddExerciseItemViewModel()
        {
            Exercise = new Exercise();
            Exercise.Name = "Dumbbell Incline Curl";
            Exercise.Description = @"With elbows back to sides, raise one dumbbell and rotate forearm until forearm is vertical and palm faces shoulder.Lower to original position and repeat with opposite arm.Continue to alternate between sides.";
            for (int i = 0; i < 17; i++)
            {
                if (i == 1 || i == 3 || i == 6)
                {
                    var m = new Muscles();
                    m.Id = i;
                    m.IsFront = true;
                    Exercise.MainMuscles.Add(m);
                }
                else
                {
                    if (i == 11)
                    {
                        var m = new Muscles();
                        m.Id = i;
                        m.IsFront = false;
                        Exercise.MainMuscles.Add(m);
                    }
                    Exercise.MainMuscles.Add(null);
                }
            }
        }

        private string _name;

        public AddExerciseItemViewModel(String name, IEventAggregator eventAggregator)
        {
            Quantity = 1;
            _eventAggregator = eventAggregator;
            AddExerciseCommand = new DelegateCommand<Type>(OnAddFood, CanAddFood);
            _name = name;
            Init(_name);
        
         }
        private bool CanAddFood(Type arg)
        {
            return true;
        }

        private async void OnAddFood(Type obj)
        {
            if (Exercise == null)
            {
                Exercise = await blRouter.GetExercise(_name);
            }
            _eventAggregator.GetEvent<BE.Events.AddExerciseToTraningEvent>()
                .Publish(new BE.Events.AddExerciseToTraningArg()
                {
                    Exercise = Exercise
                });
        }

        private async void Init(string name)
        {
            blRouter = new BL.BlRouter();
            Exercise = await blRouter.GetExercise(name);
        }

        private IEventAggregator _eventAggregator;

        public ICommand AddExerciseCommand { get; }

        private Exercise exercise;

        public Exercise Exercise
        {
            get { return exercise; }
            set {exercise = value;
                OnPropertyChanged();

            }
        }
        private int quantity;
            
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value;
                OnPropertyChanged();
            }
        }

    }
}
