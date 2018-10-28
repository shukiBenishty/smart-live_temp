using BE.Events;
using BE.Models;
using BL;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModel.MainScreen
{
    public class AddBiometrcViewModel :  ViewModelBase
    {
        private float weight;

        public AddBiometrcViewModel(IBlRouter blRouter, IEventAggregator eventAggregator, DateTime dateTime)
        {
            _dateTime = dateTime;
            _eventAggregator = eventAggregator;
            _blRouter = blRouter;
            AddWeightommand = new DelegateCommand(OnAddWeight, CanAddWeight);
            _eventAggregator.GetEvent<BE.Events.SelectedDateChangedEvent>().
                Subscribe(DataUpdate);
        }

        private bool CanAddWeight()
        {
            if (Height > 0 && Weight > 0)
            {
                return true;
            }
            return false;
        }

        private async void OnAddWeight()
        {
            _eventAggregator.GetEvent<OpenHomeEvent>().Publish();
            var bodyDimmenssions = new BodyDimmenssions();
            bodyDimmenssions.DateTime = _dateTime;
            bodyDimmenssions.Weight = Weight;
            bodyDimmenssions.Height = Height;
            await _blRouter.AddBodyDimmenssions(bodyDimmenssions);
            _eventAggregator.GetEvent<BE.Events.BodyDimmenssionsUpdateEvent>().Publish();




        }

        private void DataUpdate(SelectedDateChangedEventArg obj)
        {
            _dateTime = obj.SelectedDate;
        }

        private float height;

        public float Height
        {
            get { return height; }
            set {
                height = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWeightommand).RaiseCanExecuteChanged();

            }
        }


        public float Weight
        {
            get { return weight; }
            set {
                weight = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWeightommand).RaiseCanExecuteChanged();
            }
        }

        private DateTime _dateTime;
        private IEventAggregator _eventAggregator;
        private IBlRouter _blRouter;

        public ICommand AddWeightommand { get; }
    }
}
