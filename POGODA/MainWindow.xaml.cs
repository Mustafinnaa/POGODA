using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using POGODA.DATA;

namespace POGODA
{
    public partial class MainWindow : Window
    {
        private List<DataContext> _weatherData;
        private List<Typess> _weatherTypes;

        public MainWindow()
        {
            InitializeComponent();

            _weatherData = Weather.WeatherDataList;
            _weatherTypes = Typess.Types;
            LstView.Items.Clear();

            LstView.ItemsSource = _weatherData;
            FilterCB.ItemsSource = _weatherTypes;
        }

        private void DeleteMI_Click(object sender, RoutedEventArgs e)
        {
            DataContext selectedData = (DataContext)LstView.SelectedItem;
            _weatherData.Remove(selectedData);
        }

        private void ViewMI_Click(object sender, RoutedEventArgs e)
        {
            DataContext selectedData = (DataContext)LstView.SelectedItem;
            MessageBox.Show($"Date: {selectedData.Date.ToShortDateString()} \nTemperature: {selectedData.Temperature}°C", "Weather Info");
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AnyChanged();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AnyChanged();
        }
        private void AnyChanged()
        {
            var tempLst = _weatherData;

            var type = FilterCB.SelectedItem as ComboBoxItem;
            if (type != null)
            {
                switch (type.Content.ToString())
                {
                    case "Тепло":
                        tempLst = tempLst.Where(x => x.Temperature > 0).ToList();
                        break;
                    case "Холодно":
                        tempLst = tempLst.Where(x => x.Temperature < 0).ToList();
                        break;
                }
            }

            var sortType = SortCB.SelectedItem as ComboBoxItem;
            if (sortType != null)
            {
                switch (sortType.Content.ToString())
                {
                    case "По умолчанию":
                        break;
                    case "По дате":
                        tempLst = tempLst.OrderBy(x => x.Date).ToList();
                        break;
                    case "По возрастанию темпы":
                        tempLst = tempLst.OrderBy(x => x.Temperature).ToList();
                        break;
                    case "По убыванию темпы":
                        tempLst = tempLst.OrderBy(x => x.Temperature).ToList();
                        break;
                }
            }

            LstView.ItemsSource = tempLst; 
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            FilterCB.SelectedItem = null;
            SortCB.SelectedItem = null;
            UpdateListView();
        }

        private void AddWeatherData()
        {
            if (DateTime.TryParse(Date.Text, out DateTime date) && double.TryParse(Temperature.Text, out double temperature))
            {

                if (_weatherData.Any(d => d.Date == date))
                {
                    MessageBox.Show("нельзя добавить дату, которая уже существует");
                    return;
                }

                var newData = new DataContext { Date = date, Temperature = temperature };
                _weatherData.Add(newData);
                LstView.Items.Refresh();
                Date.Text = "";
                Temperature.Text = "";
               
            }
            
                
                else
                {
                MessageBox.Show("ты дура, неверный формат даты (dd.MM.yyyy) или температуры");
            }
        }
        
        private void UpdateListView()
        {
            LstView.ItemsSource = _weatherData;
        }

        private void CalculateStatistics()
        {
            double averageTemperature = _weatherData.Average(d => d.Temperature);
            int maxTemperature = (int)_weatherData.Max(d => d.Temperature);
            int minTemperature = (int)_weatherData.Min(d => d.Temperature);
            int sameTemperatureCount = _weatherData.GroupBy(d => d.Temperature).Count(g => g.Count() > 1);
            var anomalies = _weatherData.Zip(_weatherData.Skip(1), (a, b) => new { a, b }).Where(x => Math.Abs(x.a.Temperature - x.b.Temperature) >= 10);
            int anomalyCount = anomalies.Count();

            string anomalyDetails = anomalies.Any() ? string.Join("\n", anomalies.Select(x => $"Date: {x.a.Date.ToShortDateString()}, Temperature: {x.a.Temperature}°C -> {x.b.Temperature}°C")) : "нет дней с аномальными изменениями температуры";

            
            var sameTemperatureDays = _weatherData.GroupBy(d => d.Temperature).Where(g => g.Count() > 1)
                .Select(g => $"Date: {string.Join(", ", g.Select(d => d.Date.ToShortDateString()))}, Temperature: {g.Key}°C");

            string sameTemperatureDetails = sameTemperatureDays.Any() ? string.Join("\n", sameTemperatureDays) : "нет дней с одинаковой температурой";

            MessageBox.Show($"средняя температура: {averageTemperature}\n\nмаксимальная температура: {maxTemperature}\n\nминимальная температура: {minTemperature}\n\nтемпература была одинаковой: {sameTemperatureCount} раз\n{sameTemperatureDetails}\n\nаномальный подъем и спад температуры: {anomalyCount} раз\n{anomalyDetails}");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWeatherData();
        }

        private void ButtonStatistics_Click(object sender, RoutedEventArgs e)
        {
            CalculateStatistics();
        }
    }
}