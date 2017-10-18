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
        //private DispatcherTimer Timer;
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

        private int WindowNumber { get; set; }

        public MainViewModel()
        {
            _DataFromThingSpeak = new RootObject();
            //_ReadMasurements = new ReadMeasurementFromWeb();
            Messenger.Default.Register<MvvmMessage>(this,HandleMessage);

            SetupModel();
            //ReadData();

            // Timer = new DispatcherTimer();
            //Timer.Tick += Timer_Tick;
            // Timer.Interval = new TimeSpan(0, 0, 1);
        }

        private void HandleMessage(MvvmMessage message)
        {
            if (message.Data != null)
            {
                _DataFromThingSpeak = message.Data;
            }
            if (message.SelectedWindow != null)
            {
                WindowNumber = message.SelectedWindow;
                if (WindowNumber == 1)
                    DrawTempGraph();
                if (WindowNumber == 2)
                    DrawHumidityGraph();
                if (WindowNumber == 3)
                    DrawPressureGraph();
                CurrentPlotModel.ResetAllAxes();
                CurrentPlotModel.InvalidatePlot(true);
            }
        }

        private void SetupModel()
        {
            CurrentPlotModel = new PlotModel();
            CurrentPlotModel.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Red, });
            CurrentPlotModel.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Color = OxyColors.Blue, });
            CurrentPlotModel.Axes.Add(new DateTimeAxis { StringFormat="HH:mm"});
            //Timer.Start();
        }
        private void SetUpLegend()
        {
            CurrentPlotModel.LegendTitle = "Legenda";
            CurrentPlotModel.LegendOrientation = LegendOrientation.Horizontal;
            CurrentPlotModel.LegendPosition = LegendPosition.TopRight;
            CurrentPlotModel.LegendPlacement = LegendPlacement.Outside;
            CurrentPlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            CurrentPlotModel.LegendBorder = OxyColors.Black;
        }

        private void DrawTempGraph()
        {           
            var firstSeries = (LineSeries)CurrentPlotModel.Series[0];
            firstSeries.Points.Clear();
            var secondSeries = (LineSeries)CurrentPlotModel.Series[1];
            secondSeries.Points.Clear();
            
            for (int i = 0; i < _DataFromThingSpeak.Measurements.Count; i++)
            {
                if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp) != 0)
                    firstSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].created_at),
                        Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp)));
                if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureDht) != 0)
                    secondSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].created_at),
                        Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureDht)));
            }
            var s = (LineSeries)CurrentPlotModel.Series[0];
            s.XAxis.Title = "Data pomiaru";
            s.XAxis.EndPosition = 5;
            s.XAxis.AbsoluteMinimum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[0].created_at);
            s.XAxis.AbsoluteMaximum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[_DataFromThingSpeak.Measurements.Count - 1].created_at);
            s.YAxis.Title = "Temperatra [°C]";
            s.YAxis.AbsoluteMaximum = 35;
            s.YAxis.AbsoluteMinimum = -10;            
        }

        private void DrawHumidityGraph()
        {
            var firstSeries = (LineSeries)CurrentPlotModel.Series[0];
            firstSeries.Points.Clear();
            var secondSeries = (LineSeries)CurrentPlotModel.Series[1];
            secondSeries.Points.Clear();
            for (int i = 0; i < _DataFromThingSpeak.Measurements.Count; i++)
            {
                if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp) != 0)
                    firstSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].created_at), 
                        Convert.ToDouble(_DataFromThingSpeak.Measurements[i].Humidity)));
            }
            var s = (LineSeries)CurrentPlotModel.Series[0];
            s.XAxis.Title = "Data pomiaru";
            s.XAxis.EndPosition = 5;
            s.XAxis.AbsoluteMinimum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[0].created_at);
            s.XAxis.AbsoluteMaximum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[_DataFromThingSpeak.Measurements.Count - 1].created_at);
            s.YAxis.Title = "Wilgotność [%]";
            s.YAxis.AbsoluteMinimum = 0;
            s.YAxis.AbsoluteMaximum = 100;  
        }
        private void DrawPressureGraph()
        {
            var firstSeries = (LineSeries)CurrentPlotModel.Series[0];
            firstSeries.Points.Clear();
            var secondSeries = (LineSeries)CurrentPlotModel.Series[1];
            secondSeries.Points.Clear();
            for (int i = 0; i < _DataFromThingSpeak.Measurements.Count; i++)
            {
                if (Convert.ToDouble(_DataFromThingSpeak.Measurements[i].TemperatureBmp) != 0)
                    firstSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[i].created_at),
                        Convert.ToDouble(_DataFromThingSpeak.Measurements[i].Pressure)));
            }
            var s = (LineSeries)CurrentPlotModel.Series[0];
            s.XAxis.Title = "Data pomiaru";
            s.XAxis.EndPosition = 5;
            s.XAxis.AbsoluteMinimum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[0].created_at);
            s.XAxis.AbsoluteMaximum = DateTimeAxis.ToDouble(_DataFromThingSpeak.Measurements[_DataFromThingSpeak.Measurements.Count - 1].created_at);
            s.YAxis.Title = "Ciśnienie [hPa]";
            s.YAxis.AbsoluteMinimum = 960;
            s.YAxis.AbsoluteMaximum = 1050;            
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
