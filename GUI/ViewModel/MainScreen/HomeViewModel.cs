using BE.Events;
using BE.Models;
using BL;
using GUI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    public class HomeViewModel : ViewModelBase, IHomeViewModel
    {
        private IEventAggregator _eventAggregator;
        private IBlRouter _blRouter;
        private DateTime _dateTime;
        private Goal goal;
        private Goal consume;



        public HomeViewModel(IEventAggregator eventAggregator,
              BottomChartViewModel bottomChartViewModel,
              IMessageDialogService messageDialogService,
            IBlRouter blRouter)
        {
            BottomChartViewModel = bottomChartViewModel;
            this.Consume = new Goal();
            _dateTime = DateTime.Now;

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<BE.Events.SelectedDateChangedEvent>()
            .Subscribe(OnSelectedDateChanged);
            _eventAggregator.GetEvent<BE.Events.MealAddedEvent>().Subscribe(UpdateConsume);
            _eventAggregator.GetEvent<BE.Events.UserLogInSeccEvent>().Subscribe(UpdateData);
            _eventAggregator.GetEvent<BE.Events.GoalsIsUpdate>().Subscribe(UpdateGoal);
            _blRouter = blRouter;
            UpdateData();
        }

        private void UpdateData(UserLogInSeccEventArg obj)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            UpdateGoal();
            UpdateConsume();
           
        }

        private async void  UpdateConsume()
        {
            Consume = await new BlRouter(_eventAggregator).GetConsume(_dateTime);
            
        }

        private void OnSelectedDateChanged(SelectedDateChangedEventArg obj)
        {
            _dateTime = obj.SelectedDate;
            UpdateData();
        }

        private bool isLoad;

       
        private async void UpdateGoal()
        {
           var result = await  new BlRouter(_eventAggregator).GetGoal(_dateTime);
            if (result == null)
            {
                result =  new BlRouter(_eventAggregator).GetDefaultGoal(_dateTime);
            }
            Goal = result;
        }

        public BottomChartViewModel BottomChartViewModel { get; }


        private Goal to;

        public Goal To
        {
            get { return to; }
            set { to = value; OnPropertyChanged(); }
        }

       


        public Goal Consume
        {
            get { return consume; }
            set { consume = value;OnPropertyChanged(); }
        }


        public BE.Models.Goal Goal
        {
            get { return goal; }
            set { goal = value; OnPropertyChanged(); }
        }



        public bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; OnPropertyChanged(); }
        }


        public Task LoadAsync()
        {
            return null;
        }
    }
}
