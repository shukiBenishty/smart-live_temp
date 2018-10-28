using BE.Events;
using BE.Models;
using BL;
using GUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GUI.ViewModel.MainScreen
{
    public class AddExerciseViewModel : ViewModelBase
    {
        private AddExerciseItemViewModel addExerciseItemViewModel;
        private ObservableCollection<Exercise> exercisesList;
        private ObservableCollection<ExerciseInTraining> training;

        private IBlRouter _blRouter;
        private String search;

        public AddExerciseViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            DateTime selectedDate,
            IBlRouter blRouter)
        {
            _selectedDate = selectedDate;
            ExercisesList = new ObservableCollection<Exercise>();
            Training = new ObservableCollection<ExerciseInTraining>();
            EventAggregator = eventAggregator;
            _blRouter = blRouter;

            FinishCommand = new DelegateCommand<Type>(OnFinish, CanFinish);

            subscribe();
        }


        private void OnFinish(Type obj)
        {

            EventAggregator.GetEvent<TrainingAddedEvent>().Publish();
            EventAggregator.GetEvent<OpenHomeEvent>().Publish();

        }

        private bool CanFinish(Type arg)
        {
            return true;
        }

        private void subscribe()
        {
            EventAggregator.GetEvent<BE.Events.AddExerciseToTraningEvent>().
                 Subscribe(AddExerciseToTraining);
            EventAggregator.GetEvent<BE.Events.SelectedDateChangedEvent>()
                .Subscribe(OnSelectedDateChanged);
        }


        private void OnSelectedDateChanged(SelectedDateChangedEventArg obj)
        {
            _selectedDate = obj.SelectedDate;
        }

        public AddExerciseItemViewModel AddExerciseItemViewModel
        {
            get { return addExerciseItemViewModel; }
            set { addExerciseItemViewModel = value; OnPropertyChanged(); }
        }

        public IEventAggregator EventAggregator { get; }



        public String Search
        {
            get { return search; }
            set
            {
                search = value;
                OnPropertyChanged();
                SearchExercise();
            }
        }

        public async void SearchExercise()
        {

            var result = await new BL.BlRouter(EventAggregator).GetExercises(Search);
            ExercisesList.Clear();
            foreach (var exercies in result)
            {
                ExercisesList.Add(exercies);
            }

        }
        private void AddExerciseToTraining(AddExerciseToTraningArg obj)
        {
            Training.Add(new ExerciseInTraining
            {
                Exercise = obj.Exercise,
                Quantity = obj.Quantity,
            });
        }


        public ICommand FinishCommand { get; }

        private DateTime _selectedDate;

        public ObservableCollection<Exercise> ExercisesList
        {
            get { return exercisesList; }
            set { exercisesList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ExerciseInTraining> Training
        {
            get { return training; }
            set { training = value; OnPropertyChanged(); }
        }

    }
}

