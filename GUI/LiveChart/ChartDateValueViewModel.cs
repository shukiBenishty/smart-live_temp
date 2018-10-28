using GUI.ViewModel;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI.LiveChart
{
   public class ChartDateValueViewModel : ViewModelBase
    {
        public ChartDateValueViewModel()
        {
            try
            {
                
                Formatter = value =>
                {
                    if (value < 0 )
                    {
                        value = 0;
                    }
                    DateTime dateTime = new DateTime((long)(value * TimeSpan.FromDays(1).Ticks));
                    return dateTime.ToString("d");
                };
            }
            catch (Exception e)
            {
                Formatter = null;
            }
          
           
        }

        public void Update(List<DateModel> data)
        {
            if (data.Count==0)
            {
                return;
            }
          
            var lineSeries = new LineSeries();
            lineSeries.Values = new  ChartValues<DateModel>();
            var columnSeries = new ColumnSeries();
            columnSeries.Values = new ChartValues<DateModel>();
            var minDate = data[0].DateTime;
            foreach (var item in data)
            {
                lineSeries.Values.Add(item);
                columnSeries.Values.Add(item);
                if (item.DateTime < minDate)
                {
                    minDate = item.DateTime;
                }
            }
            var dayConfig = Mappers.Xy<DateModel>()
           .X(dateModel => dateModel.DateTime.Ticks / TimeSpan.FromDays(1).Ticks)
           .Y(dateModel => dateModel.Value);
            Series = new SeriesCollection(dayConfig)
            {
                lineSeries,
                columnSeries
            };
            if (Formatter == null)
            {
                Formatter = value => new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("d");
            }

        }

        private Func<double, string> formatter;

        public Func<double, string> Formatter
        {
            get { return formatter; }
            set { formatter = value;OnPropertyChanged(); }
        }

        private SeriesCollection series;

        public SeriesCollection Series
        {
            get { return series; }
            set { series = value;OnPropertyChanged(); }
        }

       
    }
}
