using GalaSoft.MvvmLight.Messaging;
using EngineeringThesis.Model;
using EngineeringThesis.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringThesis.ViewModel
{
    public class StatisticsViewModel : INotifyPropertyChanged
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
                OnPropertyChanged("ReadButtonIsEnabled");
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

        public StatisticsViewModel()
        {
            _DataFromThingSpeak = new RootObject();
            _ReadMeasurements = new ReadMeasurementFromWeb();
            ReadMeasurementsClick = new ReadMeasurementsCommand(this);
            AmontOfReading = 0;
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
            if (!_ReadMeasurements.ReadAmountOfFeeds())
                ReadButtonIsEnabled = true;
            _DataFromThingSpeak = await _ReadMeasurements.ReadChannelField();
            SendReadMeasurements();

            AmontOfReading = _DataFromThingSpeak.Measurements.Count;

            SearchHighLowHumidity();
            SearchHighLowTemperatureBMP();
            SearchHighLowTemperatureDHT();
            SearchHighLowPressure();

            UpdateValues();
            ReadButtonIsEnabled = true;            
        }

        private void SearchHighLowHumidity()
        {
            var sort = from humidity in _DataFromThingSpeak.Measurements
                       where humidity.Humidity != null && humidity.Humidity != "0"
                       orderby humidity.Humidity descending
                       select humidity;

            LowestHumidity = sort.Last().Humidity.Replace("\r\n\r\n", "");
            LowestHumidityDate = sort.Last().CreatedData;

            HighestHumidity = sort.First().Humidity.Replace("\r\n\r\n", "");
            HighestHumidityDate = sort.First().CreatedData;
        }
        private void SearchHighLowTemperatureBMP()
        {
            var sort = from temp in _DataFromThingSpeak.Measurements
                       where  temp.TemperatureBmp != null
                       orderby temp.TemperatureBmp descending
                       select temp;

            LowestTemperatureBMP = sort.Last().TemperatureBmp.Replace("\r\n\r\n", "");
            LowestTemperatureDateBMP = sort.Last().CreatedData;

            HighestTemperatureBMP = sort.First().TemperatureBmp.Replace("\r\n\r\n", "");
            HighestTemperatureDateBMP = sort.First().CreatedData;
        }
        private void SearchHighLowTemperatureDHT()
        {
            var sort = from temp in _DataFromThingSpeak.Measurements
                       where temp.TemperatureDht != null && temp.TemperatureDht != "0"
                       orderby temp.TemperatureDht descending
                       select temp;

            LowestTemperatureDHT = sort.Last().TemperatureDht.Replace("\r\n\r\n", "");
            LowestTemperatureDateDHT = sort.Last().CreatedData;

            HighestTemperatureDHT = sort.First().TemperatureDht.Replace("\r\n\r\n", "");
            HighestTemperatureDateDHT = sort.First().CreatedData;
        }
        private void SearchHighLowPressure()
        {
            var sort = from pressure in _DataFromThingSpeak.Measurements
                       where pressure.Pressure != null
                       orderby pressure.Pressure descending
                       select pressure;

            LowestPressure = sort.Last().Pressure.Replace("\r\n\r\n", "");
            LowestPressureDate = sort.Last().CreatedData;

            HighestPressure = sort.First().Pressure.Replace("\r\n\r\n", "");
            HighestPressureDate = sort.First().CreatedData;
        }

        private void UpdateValues()
        {
            OnPropertyChanged("HighestHumidity");
            OnPropertyChanged("HighestHumidityDate");
            OnPropertyChanged("LowestHumidity");
            OnPropertyChanged("LowestHumidityDate");

            OnPropertyChanged("HighestTemperatureBMP");
            OnPropertyChanged("HighestTemperatureDateBMP");
            OnPropertyChanged("LowestTemperatureBMP");
            OnPropertyChanged("LowestTemperatureDateBMP");

            OnPropertyChanged("HighestTemperatureDHT");
            OnPropertyChanged("HighestTemperatureDateDHT");
            OnPropertyChanged("LowestTemperatureDHT");
            OnPropertyChanged("LowestTemperatureDateDHT");

            OnPropertyChanged("HighestPressure");
            OnPropertyChanged("HighestPressureDate");
            OnPropertyChanged("LowestPressure");
            OnPropertyChanged("LowestPressureDate");

            OnPropertyChanged("AmontOfReading");
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if(propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
