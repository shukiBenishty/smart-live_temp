using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Events;
using GUI.Event;
using BE.Events;
using Prism.Commands;

namespace GUI.ViewModel
{
    public class AddMenuViewModel : ViewModelBase,  IAddMenuViewModel
    {
        private IEventAggregator _eventAggregator;
        private DateTime selectedDate;

        public AddMenuViewModel(IEventAggregator eventAggregator)
        {
            
            _eventAggregator = eventAggregator;
            selectedDate = DateTime.Now;
            AddFoodCommand = new DelegateCommand<Type>(OnAddFoodOpen, CanAddFoodOpen);
            AddExerciseCommand = new DelegateCommand<Type>(OnAddExercise, CanAddExercise);
            AddBiometrcCommand = new DelegateCommand<Type>(OnAddBiometrc, CanAddBiometrc);
            OpenHomeCommand = new DelegateCommand<Type>(OnOpenHome, CanOpenHome);
            OpenActivityCommand = new DelegateCommand<Type>(OnOpenActivity, CanOpenActivity);
            OpenGoalsCommand = new DelegateCommand(OnOpenGoals, CanOpenGoals);
        }

        private bool CanOpenGoals()
        {
            return true;
        }

        private void OnOpenGoals()
        {
            _eventAggregator.GetEvent<OpenGoalsEvent>().Publish();
        }

        private bool CanOpenActivity(Type arg)
        {
           return true;
        }

        private void OnOpenActivity(Type obj)
        {
            _eventAggregator.GetEvent<OpenActivityEvent>().Publish();
        }

        private bool CanOpenHome(Type obj)
        {
            return true;
        }

        private void OnOpenHome(Type obj)
        {
            _eventAggregator.GetEvent<OpenHomeEvent>().Publish();
        }

        public AddMenuViewModel()
        {
        }

        private bool CanAddBiometrc(Type obj)
        {
            return true;
        }

        private void OnAddBiometrc(Type obj)
        {
            _eventAggregator.GetEvent<BE.Events.OpenAddBiometrcEvent>().Publish();
        }

        private bool CanAddExercise(Type obj)
        {
            return true;
        }

        private void OnAddExercise(Type obj)
        {

            _eventAggregator.GetEvent<OpenAddExerciseEvent>().Publish(
                 new OpenAddExerciseEventArg());
        }

        private bool CanAddFoodOpen(object arg)
        {
            return true;
        }


        private void OnAddFoodOpen(Type obj)
        {
            _eventAggregator.GetEvent<OpenAddFoodEvent>().Publish(
                new OpenAddFoodEventArg());
        }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged();
                _eventAggregator.GetEvent<SelectedDateChangedEvent>()
                    .Publish(new SelectedDateChangedEventArg(value));
            }
        }

        public Task LoadAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public ICommand OpenHomeCommand { get; }
        public ICommand AddFoodCommand { get; }
        public ICommand AddExerciseCommand { get; }
        public ICommand AddBiometrcCommand { get; }
        public ICommand OpenActivityCommand { get; }
        public ICommand OpenGoalsCommand { get; }
        



    }
}
