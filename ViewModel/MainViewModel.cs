using GalaSoft.MvvmLight.Messaging;
using EngineeringThesis.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;

namespace EngineeringThesis.ViewModel
{
    public class MainViewModel :  INotifyPropertyChanged
    {
        private DispatcherTimer Timer;
        private PlotModel _CurrentPlotModel;
        public PlotModel CurrentPlotModel
        {
            get { return _CurrentPlotModel; }
            private set
            {
                _CurrentPlotModel = value;
                OnPropertyChanged("CurrentPlotModel");
            }
        }

        private int _CurrentWindow { get; set; }
        public int CurrentWindow
        {
            get { return _CurrentWindow; }
            set
            {
                _CurrentWindow = value;
            }
        }

        private RootObject _DataFromThingSpeak { get; set; }
        private ReadMeasurementFromWeb _ReadMasurements { get; set; }

        private int _SelectedWindow { get; set; }

        public MainViewModel()
        {
            _DataFromThingSpeak = new RootObject();
            _ReadMasurements = new ReadMeasurementFromWeb();
            Messenger.Default.Register<MvvmMessage>(this,HandleMessage);

            SetupModel();
             Timer = new DispatcherTimer();
             Timer.Tick += Timer_Tick;
             Timer.Interval = new TimeSpan(0,5,0);
        }
        private void HandleMessage(MvvmMessage message)
        {
            if (message.Data != null)
            {
                _DataFromThingSpeak = message.Data;
                Timer.Start();
            }
            _SelectedWindow = message.SelectedWindow;
            RedrawGraph(_SelectedWindow);
            CurrentPlotModel.ResetAllAxes();
            CurrentPlotModel.InvalidatePlot(true);            
        }
        private void SetupModel()
        {
            CurrentPlotModel = new PlotModel();
            CurrentPlotModel.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Red,  });
            CurrentPlotModel.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Blue, });
            CurrentPlotModel.Axes.Add(new DateTimeAxis { StringFormat="HH:mm", Title = "Data pomiaru", EndPosition = 5});
            SetUpLegend();
        }
        private void SetUpLegend()
        {
            CurrentPlotModel.LegendTitle = "Legenda";
            CurrentPlotModel.LegendOrientation = LegendOrientation.Vertical;
            CurrentPlotModel.LegendPosition = LegendPosition.TopRight;
            CurrentPlotModel.LegendPlacement = LegendPlacement.Inside;
            CurrentPlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            CurrentPlotModel.LegendBorder = OxyColors.Black;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            var read = await _ReadMasurements.ReadChannelLastMeasurements();
            _DataFromThingSpeak.Measurements.Add(read.Measurements[0]);
            RedrawGraph(_SelectedWindow);
            CurrentPlotModel.InvalidatePlot(true);
        }
        private void RedrawGraph(int selectedWindow)
        {
            var firstSeries = (LineSeries)CurrentPlotModel.Series[0];
            firstSeries.Points.Clear();
            var secondSeries = (LineSeries)CurrentPlotModel.Series[1];
            secondSeries.Points.Clear();
            var s = (LineSeries)CurrentPlotModel.Series[0];
            if (selectedWindow == 1)
            {
                for (int i = 0; i < _DataFromThingSpeak.Measurements.Count; i++)
                {
                    if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp) != 0)
                        firstSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].CreatedData),
                            Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp)));
                    if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureDht) != 0)
                        secondSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].CreatedData),
                            Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureDht)));
                }
                firstSeries.Title = "Temperatura (BMP280)";
                secondSeries.Title = "Temperatura (DHT22)";
                s.YAxis.Title = "Temperatra [°C]";
                s.YAxis.AbsoluteMaximum = 35;
                s.YAxis.AbsoluteMinimum = -10;
            }
            else if (selectedWindow == 2)
            {
                for (int i = 0; i < _DataFromThingSpeak.Measurements.Count; i++)
                {
                    if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp) != 0)
                        firstSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].CreatedData),
                            Convert.ToDouble(_DataFromThingSpeak.Measurements[i].Humidity)));
                }
                firstSeries.Title = "Wilgotność";
                secondSeries.Title = "";
                s.YAxis.Title = "Wilgotność [%]";
                s.YAxis.AbsoluteMinimum = 0;
                s.YAxis.AbsoluteMaximum = 100;
            }
            else if (selectedWindow == 3)
            {
                for (int i = 0; i < _DataFromThingSpeak.Measurements.Count; i++)
                {
                    if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp) != 0)
                        firstSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].CreatedData),
                            Convert.ToDouble(_DataFromThingSpeak.Measurements[i].Pressure)));
                }
                firstSeries.Title = "Ciśnienie";
                secondSeries.Title = "";
                s.YAxis.Title = "Ciśnienie [hPa]";
                s.YAxis.AbsoluteMinimum = 960;
                s.YAxis.AbsoluteMaximum = 1050;
            }
            s.XAxis.AbsoluteMinimum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[0].CreatedData);
        }        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
