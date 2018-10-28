using BE.API.Nutritionix.Result;
using LiveCharts;
using System;
using BE.Models;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;


namespace GUI.ViewModel.MainScreen
{
    public class AddFoodItemViewModel : ViewModelBase
    {
       
        private BL.IBlRouter blRouter;
        private int quantity;
        private IEventAggregator _eventAggregator;
        private bool isLoad;
        private string _name;
        

        public  AddFoodItemViewModel(String name, IEventAggregator eventAggregator)
        {
            IsLoad = true;
            Quantity = 1;
            _eventAggregator = eventAggregator;
            AddFoodCommand = new DelegateCommand<Type>(OnAddFood, CanAddFood);
            _name = name;
             Init(_name);
        }

        private bool CanAddFood(Type arg)
        {
            return true;
        }

        private async void OnAddFood(Type obj)
        {
            if (Food ==null)
            {
                Food = await blRouter.GetFoodNutritions(_name);
            }
            _eventAggregator.GetEvent<BE.Events.AddFoodToMealEvent>()
                .Publish(new BE.Events.AddFoodToMealEventArg()
                {
                    Nutritions = Food,
                    Food = new FoodInMeal()
                    {
                        FoodName = _name,
                        Quantity = this.Quantity
                    }
                });
        }

        private async void Init(string name)
        {

            blRouter = new BL.BlRouter(_eventAggregator);
            Food =  await  blRouter.GetFoodNutritions(name);
            IsLoad = false;
        }

        public ICommand AddFoodCommand { get; }

       

        public int Quantity
        {
            get { return quantity; }
            set {
                if (value < 0)
                {
                    return;
                }
                quantity = value; OnPropertyChanged();
            }
        }


        private FoodNutritionsItem food;

        public FoodNutritionsItem Food
        {
            get { return food; }
            set { food = value; OnPropertyChanged(); }
        }

        public bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; OnPropertyChanged(); }
        }
    }
   
}
