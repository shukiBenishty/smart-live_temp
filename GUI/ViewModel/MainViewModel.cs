using BE.Events;
using BL;
using GUI.Event;
using GUI.View;
using GUI.View.MainScreen;
using GUI.View.Services;
using GUI.ViewModel.google;
using GUI.ViewModel.MainScreen;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
      
        private bool getIsLogOn;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private IBlRouter _blRouter;
        private object selectedView;
        private DateTime getDateTime;
        private Boolean registerMode;
        private bool isLoad;
        private ActivityView activityView;
        private ActivityViewModel activityViewModel;
        private IHomeViewModel _homeViewModel;

        public MainViewModel(IEventAggregator eventAggregator,    IMessageDialogService messageDialogService,
            IHomeViewModel homeViewModel,
            IAddMenuViewModel addMenuViewModel, IGoogleLoginViewModel googleLoginViewModel,
            RegisterViewModel registerViewModel, 
            BottomChartViewModel bottomChartViewModel,
            IBlRouter blRouter )
        {
            IsLoad = true;
            RegisterViewModel = registerViewModel;
            AddMenuViewModel = addMenuViewModel;
            GoogleLoginViewModel = googleLoginViewModel;
           
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _blRouter = blRouter;
            getDateTime = DateTime.Now;
          
            GetIsLogOn = false;

            Subscribe();
            CheckIsLogOn();
            _homeViewModel = homeViewModel;
            HomeView = new HomeView();
            HomeView.DataContext = _homeViewModel;
            OpenHome();
            
          
        }

        public object SelectedView
        {
            get { return selectedView; }
            set { selectedView = value; OnPropertyChanged(); }
        }

        public GUI.View.HomeView HomeView { get;  }
        public IAddMenuViewModel AddMenuViewModel { get; }
        public IGoogleLoginViewModel GoogleLoginViewModel { get;  }
      
        
        public RegisterViewModel RegisterViewModel { get; }

        public Boolean GetIsLogOn
        {
            get { return getIsLogOn; }
            set {
                getIsLogOn = value;
                OnPropertyChanged();
            }
        }

        public Boolean RegisterMode
        {
            get { return registerMode; }
            set { registerMode = value; OnPropertyChanged(); }
        }

        private async void CheckIsLogOn()
        {
            GetIsLogOn = await _blRouter.GetislogOn();
            IsLoad = false;
        }

        private void OpenProfile(UserLogInSeccEventArg obj)
        {
            GetIsLogOn = true;
        }

        private void OpenAddExercise(OpenAddExerciseEventArg obj)
        {
            var view = new AddExerciseView(new AddExerciseViewModel(_eventAggregator,_messageDialogService,getDateTime, new BlRouter(_eventAggregator)));
            SelectedView = view;
        }

        private void OpenAddFood(OpenAddFoodEventArg obj)
        {
            SelectedView = new SearchFoodView(
                new SearchViewModel(_eventAggregator, _messageDialogService, getDateTime, _blRouter));
        }

        private void Subscribe()
        {
            _eventAggregator.GetEvent<OpenAddFoodEvent>()
                 .Subscribe(OpenAddFood);
            _eventAggregator.GetEvent<OpenAddExerciseEvent>()
                .Subscribe(OpenAddExercise);
            _eventAggregator.GetEvent<UserLogInSeccEvent>()
               .Subscribe(OpenProfile);
            _eventAggregator.GetEvent<BE.Events.OpenHomeEvent>()
                .Subscribe(OpenHome);
            _eventAggregator.GetEvent<OpenAddBiometrcEvent>()
                .Subscribe(OpenAddBiometrc);
            _eventAggregator.GetEvent<BE.Events.SelectedDateChangedEvent>()
                .Subscribe(OnSelectedDateChanged);
            _eventAggregator.GetEvent<OpenRegisterModeEvent>()
                .Subscribe(OnRegisterMode);
            _eventAggregator.GetEvent<OpenActivityEvent>()
                .Subscribe(OpenActivity);
            _eventAggregator.GetEvent<OpenGoalsEvent>()
                .Subscribe(OpenGoals);
        }

        private void OpenGoals()
        {
            var view = new GUI.View.AddGoalsView();
            view.DataContext = new GUI.ViewModel.AddGoalsViewModel(_eventAggregator, _messageDialogService, getDateTime, new BL.BlRouter(_eventAggregator));
            SelectedView = view;
        }

        private void OpenActivity()
        {
            if (activityView == null)
            {
                activityView = new ActivityView();
                activityViewModel = new ActivityViewModel(_eventAggregator, new BL.BlRouter(_eventAggregator),getDateTime );
                activityView.DataContext = activityViewModel;
            }
            else
            {
                activityViewModel.LoadAsync();
            }
            SelectedView = activityView;
        }

        private void OnRegisterMode()
        {
            RegisterMode = true;
        }

        private void OnSelectedDateChanged(SelectedDateChangedEventArg obj)
        {
            getDateTime = obj.SelectedDate;
        }

        private void OpenAddBiometrc()
        {
            var view = new GUI.View.MainScreen.AddBiometrcView();
            view.DataContext = new AddBiometrcViewModel(new BL.BlRouter(_eventAggregator), _eventAggregator,getDateTime );
            SelectedView = view;
        }

        private void OpenHome()
        {
            RegisterMode = false;
            SelectedView = HomeView;
        }

       

        public bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value;OnPropertyChanged(); }
        }


    }
}

