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
        private IEventAggregator _eventAggregator;

        private string _name;

        public AddExerciseItemViewModel(String name, IEventAggregator eventAggregator)
        {
            Quantity = 1;
            _eventAggregator = eventAggregator;
            AddExerciseCommand = new DelegateCommand<Type>(OnAddExercise, CanAddExercise);
            _name = name;
            Init(_name);
        
         }
        private bool CanAddExercise(Type arg)
        {
            return true;
        }

        private async void OnAddExercise(Type obj)
        {
            if (Exercise == null)
            {
                Exercise = await blRouter.GetExercise(_name);
            }
            _eventAggregator.GetEvent<BE.Events.AddExerciseToTraningEvent>()
                .Publish(new BE.Events.AddExerciseToTraningArg()
                {
                    Exercise = Exercise.Name,
                    Quantity = Quantity
                });
        }

        private async void Init(string name)
        {
            blRouter = new BL.BlRouter(_eventAggregator);
            Exercise = await blRouter.GetExercise(name);
        }

      

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
