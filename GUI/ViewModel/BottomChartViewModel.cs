using BL;
using GUI.LiveChart;
using Prism.Events;
using System.Collections.Generic;

namespace GUI.ViewModel
{
    public class BottomChartViewModel :ViewModelBase
    {
        public BottomChartViewModel(IBlRouter blRouter, IEventAggregator eventAggregator) 
        {
            _blRouter = blRouter;
            WeightChartViewModel = new ChartDateValueViewModel();
            BmiChartViewModel = new ChartDateValueViewModel();
            InitCharts(blRouter);
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<BE.Events.BodyDimmenssionsUpdateEvent>()
               .Subscribe(OnBodyDimmenssionsUpdate);

        }

        private  void OnBodyDimmenssionsUpdate()
        {
            InitCharts(_blRouter);
        }

        private async void InitCharts(IBlRouter blRouter)
        {
            var result = await blRouter.GetBodyDimmenssions();
            if (result.Count == 0)
            {
                return;
            }
            IniTWeightChart(result);
            InitBmiChart(result);
        }

        private void IniTWeightChart(List<BE.Models.BodyDimmenssions> result)
        {
            var list = new List<DateModel>();
            foreach (var item in result)
            {
                list.Add(new DateModel()
                {
                    DateTime = item.DateTime,
                    Value = item.Weight / (item.Height * item.Height / 10000)
                });
            }
            BmiChartViewModel.Update(list);
        }

        private  void InitBmiChart(List<BE.Models.BodyDimmenssions> result)
        {
            var list = new List<DateModel>();
            foreach (var item in result)
            {
                list.Add(new DateModel()
                {
                    DateTime = item.DateTime,
                    Value = item.Weight
                });
            }
            WeightChartViewModel.Update(list);
        }

        private ChartDateValueViewModel weightChartViewModel;
        private IEventAggregator _eventAggregator;
        private IBlRouter _blRouter;

        public ChartDateValueViewModel WeightChartViewModel
        {
            get { return weightChartViewModel; }
            set { weightChartViewModel = value;OnPropertyChanged(); }
        }

        private ChartDateValueViewModel bbmiChartViewModel;

        public ChartDateValueViewModel BmiChartViewModel
        {
            get { return bbmiChartViewModel; }
            set { bbmiChartViewModel = value;OnPropertyChanged(); }
        }


    }
}
