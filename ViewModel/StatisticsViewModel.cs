using GalaSoft.MvvmLight.Messaging;
using EngineeringThesis.Model;
using EngineeringThesis.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace EngineeringThesis.ViewModel 
{
    public class StatisticsViewModel : ViewModelBase
    {
        private RootObject _DataFromThingSpeak { get; set; }
        private ReadMeasurementFromWeb _ReadMeasurements { get; set; }

        public ReadMeasurementsCommand ReadMeasurementsClick { get; set; }
        private bool _ReadButtonIsEnabled;
        public bool ReadButtonIsEnabled
        {
            get { return _ReadButtonIsEnabled; }
            set
            {
                _ReadButtonIsEnabled = value;
                RaisePropertyChanged("ReadButtonIsEnabled");
            }
        }

        public string LowestTemperatureBMP { get; private set; }
        public DateTime LowestTemperatureDateBMP { get; private set; }
        public string HighestTemperatureBMP { get; private set; }
        public DateTime HighestTemperatureDateBMP { get; private set; }

        public string LowestTemperatureDHT { get; private set; }
        public DateTime LowestTemperatureDateDHT { get; private set; }
        public string HighestTemperatureDHT { get; private set; }
        public DateTime HighestTemperatureDateDHT { get; private set; }

        public string LowestPressure { get; private set; }
        public DateTime LowestPressureDate { get; private set; }
        public string HighestPressure { get; private set; }
        public DateTime HighestPressureDate { get; private set; }

        public string LowestHumidity { get; private set; }
        public DateTime LowestHumidityDate { get; private set; }
        public string HighestHumidity { get; private set; }
        public DateTime HighestHumidityDate { get; private set; }

        public int AmontOfReading { get; private set; }

        public IEnumerable<string> TimeRange
        {
            get
            {
                var TimeRange = new string[] { "Ostatnia godzina","Ostatni dzień", "Ostatni tydzień", "Wszystkie możliwe próbki" };
                return TimeRange;                
            }       
        }

        private int _SelectedTimeRangeIndex;
        public int SelectedTimeRangeIndex
        {
            get { return _SelectedTimeRangeIndex; }
            set
            {
                _SelectedTimeRangeIndex = value;
                if (value == 0)
                    _SelectedTimeRange = 12;
                if (value == 1)
                    _SelectedTimeRange = 288;
                if (value == 2)
                    _SelectedTimeRange = 2016;
                if (value == 3)
                    _SelectedTimeRange = 8000;
            }
        }
        private int _SelectedTimeRange;                    
            
        public StatisticsViewModel()
        {
            _DataFromThingSpeak = new RootObject();
            _ReadMeasurements = new ReadMeasurementFromWeb();
            ReadMeasurementsClick = new ReadMeasurementsCommand(this);

            AmontOfReading = 0;
            _SelectedTimeRange = 12;
            ReadButtonIsEnabled = true;
        }
        
        private void SendReadMeasurements()
        {
            var msg = new MvvmMessage() { Data = _DataFromThingSpeak };
            Messenger.Default.Send<MvvmMessage>(msg);
        }        
        public async void ReadData()
        {
            ReadButtonIsEnabled = false;
            _ReadMeasurements = new ReadMeasurementFromWeb();            
            _DataFromThingSpeak = await _ReadMeasurements.ReadChannelField(_SelectedTimeRange);
            if (_DataFromThingSpeak != null)
            { 
                SendReadMeasurements();

                AmontOfReading = _DataFromThingSpeak.Measurements.Count;

                SearchHighLowHumidity();
                SearchHighLowTemperatureBMP();
                SearchHighLowTemperatureDHT();
                SearchHighLowPressure();

                UpdateValues();
            }
            ReadButtonIsEnabled = true;            
        }
        private void SearchHighLowHumidity()
        {
            var sort = from humidity in _DataFromThingSpeak.Measurements
                       orderby humidity.Humidity descending
                       select humidity;

            LowestHumidity = sort.Last().Humidity;
            LowestHumidityDate = sort.Last().CreatedData;

            HighestHumidity = sort.First().Humidity;
            HighestHumidityDate = sort.First().CreatedData;
        }
        private void SearchHighLowTemperatureBMP()
        {
            var sort = from temp in _DataFromThingSpeak.Measurements
                       orderby temp.TemperatureBmp descending
                       select temp;

            LowestTemperatureBMP = sort.Last().TemperatureBmp;
            LowestTemperatureDateBMP = sort.Last().CreatedData;

            HighestTemperatureBMP = sort.First().TemperatureBmp;
            HighestTemperatureDateBMP = sort.First().CreatedData;
        }
        private void SearchHighLowTemperatureDHT()
        {
            var sort = from temp in _DataFromThingSpeak.Measurements
                       orderby temp.TemperatureDht descending
                       select temp;

            LowestTemperatureDHT = sort.Last().TemperatureDht;
            LowestTemperatureDateDHT = sort.Last().CreatedData;

            HighestTemperatureDHT = sort.First().TemperatureDht;
            HighestTemperatureDateDHT = sort.First().CreatedData;
        }
        private void SearchHighLowPressure()
        {
            var sort = from pressure in _DataFromThingSpeak.Measurements
                       orderby pressure.Pressure descending
                       select pressure;

            LowestPressure = sort.Last().Pressure;
            LowestPressureDate = sort.Last().CreatedData;

            HighestPressure = sort.First().Pressure;
            HighestPressureDate = sort.First().CreatedData;
        }
        private void UpdateValues()
        {
            RaisePropertyChanged("HighestHumidity");
            RaisePropertyChanged("HighestHumidityDate");
            RaisePropertyChanged("LowestHumidity");
            RaisePropertyChanged("LowestHumidityDate");

            RaisePropertyChanged("HighestTemperatureBMP");
            RaisePropertyChanged("HighestTemperatureDateBMP");
            RaisePropertyChanged("LowestTemperatureBMP");
            RaisePropertyChanged("LowestTemperatureDateBMP");

            RaisePropertyChanged("HighestTemperatureDHT");
            RaisePropertyChanged("HighestTemperatureDateDHT");
            RaisePropertyChanged("LowestTemperatureDHT");
            RaisePropertyChanged("LowestTemperatureDateDHT");

            RaisePropertyChanged("HighestPressure");
            RaisePropertyChanged("HighestPressureDate");
            RaisePropertyChanged("LowestPressure");
            RaisePropertyChanged("LowestPressureDate");

            RaisePropertyChanged("AmontOfReading");
        }
    }
}
