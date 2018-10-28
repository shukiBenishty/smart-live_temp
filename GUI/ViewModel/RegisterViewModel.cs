using BE.Models;
using BL;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.ViewModel
{
    public  class RegisterViewModel :ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IBlRouter _blRouter;


        public RegisterViewModel(BL.IBlRouter blRouter , IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _blRouter = blRouter;
            
            sex = GENDER.MALE;
           
            SaveCommand = new  DelegateCommand(OnSave, CanSave);
        }

        private bool CanSave()
        {
           
            if (FiestName!= "" && LastName != ""
                && Birthday != null && Birthday < DateTime.Now
                && Height > 0 && Weight > 0)
            {
                return true;
            }
            return false;
        }

        private async void OnSave()
        {
            var bodyDimmenssions = new BodyDimmenssions();
            bodyDimmenssions.Weight = Weight;
            bodyDimmenssions.Height = Height;
            bodyDimmenssions.DateTime = DateTime.Now;
            var profile = new Profile();
            profile.FiestName = FiestName;
            profile.LastName = LastName;
            profile.Birthday = Birthday;
            profile.Sex = Sex;
          
            _eventAggregator.GetEvent<BE.Events.OpenHomeEvent>().Publish();
           await  _blRouter.RegisterDataSave(bodyDimmenssions, profile);
            _eventAggregator.GetEvent<BE.Events.BodyDimmenssionsUpdateEvent>().Publish();
        }







        private string fiestName;

        public string FiestName
        {
            get { return fiestName; }
            set { fiestName = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private DateTime? birthday;

        public DateTime? Birthday
        {
            get { return birthday; }
            set { birthday = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
        private GENDER sex;

        public GENDER Sex
        {
            get { return sex; }
            set { sex = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }



        private float height;

        public float Height
        {
            get { return height; }
            set { height = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private float weight;

        public float Weight
        {
            get { return weight; }
            set { weight = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }




      

        public ICommand SaveCommand { get; }


    }
}
