using BE.Events;
using BE.Models;
using BL;
using GUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace GUI.ViewModel
{
    public class AddGoalsViewModel : ViewModelBase
    {
        private Goal goal;
        private IEventAggregator _eventAggregator;
        private IBlRouter _blRouter;
        private IMessageDialogService _messageDialogService;
        private DateTime fromSelectedDate;
        private DateTime toSelectedDate;
        private bool isLoad;

        public AddGoalsViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            DateTime date,
            IBlRouter blRouter)
        {
            _date = date;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<BE.Events.SelectedDateChangedEvent>().
                Subscribe(UpdateDate);
            _blRouter = blRouter;
            _messageDialogService = messageDialogService;
            CancelCommand = new DelegateCommand(OnCancel);
            FinishCommand = new DelegateCommand(OnFinish, CanFinish);
            UpdateDate(_date);
           

        }

        private void UpdateDate(SelectedDateChangedEventArg obj)
        {
            _date = obj.SelectedDate;
            UpdateDate(obj.SelectedDate);
        }

        private void UpdateDate(DateTime date)
        {
            IsLoad = true;
            var tempFromSelectedDate = FromSelectedDate;
            fromSelectedDate = date.AddDays(-(double)date.DayOfWeek);
            toSelectedDate = fromSelectedDate.AddDays(6);
            if (tempFromSelectedDate != fromSelectedDate)
            {
                FromSelectedDate = fromSelectedDate;
                ToSelectedDate = toSelectedDate;
                GetGoals();
            }
            else
            {
                IsLoad = false;
            }

        }

        private async void GetGoals()
        {
         
            IsLoad = true;
            Goal OldGoal = await new BlRouter(_eventAggregator).GetGoal(FromSelectedDate);
            if (OldGoal != null)
            {
                Goal = OldGoal;
            }
            else
            {
                Goal = new Goal();
                Goal.From = FromSelectedDate;
                Goal.To = ToSelectedDate;
            }
            IsLoad = false;

        }

        private bool CanFinish()
        {
            return true;
        }

        private async void OnFinish()
        {
            try
            {
                IsLoad = true;
                await _blRouter.SaveGoal(Goal);
                IsLoad = false;
                await _messageDialogService.ShowInfoDialogAsync("The Goal is Saved ");
                _eventAggregator.GetEvent<BE.Events.GoalsIsUpdate>().Publish();
            }
            catch (Exception e)
            {
                await _messageDialogService.ShowInfoDialogAsync("Error The Goal not Save Please contact the administrator");
                IsLoad = false;
            }


        }

        private void OnCancel()
        {
            _eventAggregator.GetEvent<BE.Events.OpenHomeEvent>().Publish();
        }

        public DateTime ToSelectedDate
        {
            get { return toSelectedDate; }
            set
            {
                toSelectedDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime FromSelectedDate
        {
            get { return fromSelectedDate; }
            set
            {
                fromSelectedDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        public Goal Goal
        {
            get { return goal; }
            set { goal = value; OnPropertyChanged(); }
        }

        public ICommand CancelCommand { get; set; }
        public ICommand FinishCommand { get; set; }


        public bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; OnPropertyChanged(); }
        }

    }
}
