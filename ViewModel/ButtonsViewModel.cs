
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using EngineeringThesis.ViewModel.Commands;
using System.ComponentModel;
using System.Windows;

namespace EngineeringThesis.ViewModel
{
    public class ButtonsViewModel : ViewModelBase
    {
        public MainCommand MainWindowClick { get; set; }
        public TemperatureCommand TemperatureWindowClick { get; set; }
        public HumidityCommand HumidityWindowClick { get; set; }
        public PressureCommand PressureWindowClick { get; set; }

        private bool _MainWindowButtonIsEnabled;
        public bool MainWindowButtonIsEnabled
        {
            get { return _MainWindowButtonIsEnabled; }
            set
            {
                _MainWindowButtonIsEnabled = value;
                RaisePropertyChanged("MainWindowButtonIsEnabled");
            }
        }

        private bool _TemperatureButtonIsEnabled;
        public bool TemperatureButtonIsEnabled
        {
            get { return _TemperatureButtonIsEnabled; }
            set
            {
                _TemperatureButtonIsEnabled = value;
                RaisePropertyChanged("TemperatureButtonIsEnabled");
            }
        }

        private bool _HumidityButtonIsEnabled;
        public bool HumidityButtonIsEnabled
        {
            get { return _HumidityButtonIsEnabled; }
            set
            {
                _HumidityButtonIsEnabled = value;
                RaisePropertyChanged("HumidityButtonIsEnabled");
            }
        }

        private bool _PressureButtonIsEnabled;
        public bool PressureButtonIsEnabled
        {
            get { return _PressureButtonIsEnabled; }
            set
            {
                _PressureButtonIsEnabled = value;
                RaisePropertyChanged("PressureButtonIsEnabled");
            }
        }

        private Visibility _PlotIsVisible;
        public Visibility PlotIsVisible
        {
            get { return _PlotIsVisible; }
            set
            {
                _PlotIsVisible = value;
                RaisePropertyChanged("PlotIsVisible");
            }
        }
        private Visibility _StatisticWindowIsVisible;
        public Visibility StatisticWindowIsVisible
        {
            get { return _StatisticWindowIsVisible; }
            set
            {
                _StatisticWindowIsVisible = value;
                RaisePropertyChanged("StatisticWindowIsVisible");
            }
        }

        public ButtonsViewModel()
        {           
            MainWindowClick = new MainCommand(this);
            TemperatureWindowClick = new TemperatureCommand(this);
            PressureWindowClick = new PressureCommand(this);
            HumidityWindowClick = new HumidityCommand(this);
            Messenger.Default.Register<MvvmMessage>(this, HandleMessage);

            DisableAllButtons();
            PlotIsVisible = Visibility.Hidden;            
        }
        private void SelectNumberAction(int selectedNumber)
        {
            var msg = new MvvmMessage() { SelectedWindow = selectedNumber };
            Messenger.Default.Send<MvvmMessage>(msg);
        }
        private void DisableAllButtons()
        {
            MainWindowButtonIsEnabled = false;
            TemperatureButtonIsEnabled = false;
            HumidityButtonIsEnabled = false;
            PressureButtonIsEnabled = false;
        }
        private void HandleMessage(MvvmMessage message)
        {
            if (message.Data != null)
            {
                TemperatureButtonIsEnabled = true;
                HumidityButtonIsEnabled = true;
                PressureButtonIsEnabled = true;
            }
        }
        public void MainWindowCommand()
        {
            MainWindowButtonIsEnabled = false;
            TemperatureButtonIsEnabled = true;
            HumidityButtonIsEnabled = true;
            PressureButtonIsEnabled = true;
            PlotIsVisible = Visibility.Hidden;
            StatisticWindowIsVisible = Visibility.Visible;
            SelectNumberAction(0);
        }
        public void TemperatureWindowCommand()
        {
            MainWindowButtonIsEnabled = true;
            TemperatureButtonIsEnabled = false;
            HumidityButtonIsEnabled = true;
            PressureButtonIsEnabled = true;
            StatisticWindowIsVisible = Visibility.Hidden;
            PlotIsVisible = Visibility.Visible;            
            SelectNumberAction(1);
        }
        public void HumidityWindowCommand()
        {
            MainWindowButtonIsEnabled = true;
            TemperatureButtonIsEnabled = true;
            HumidityButtonIsEnabled = false;
            PressureButtonIsEnabled = true;
            PlotIsVisible = Visibility.Visible;
            StatisticWindowIsVisible = Visibility.Hidden;
            SelectNumberAction(2);
        }
        public void PressureWindowCommand()
        {
            MainWindowButtonIsEnabled = true;
            TemperatureButtonIsEnabled = true;
            HumidityButtonIsEnabled = true;
            PressureButtonIsEnabled = false;
            PlotIsVisible = Visibility.Visible;
            StatisticWindowIsVisible = Visibility.Hidden;
            SelectNumberAction(3);
        }
    }
}


