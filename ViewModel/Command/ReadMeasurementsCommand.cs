using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EngineeringThesis.ViewModel.Commands
{
    public class ReadMeasurementsCommand :ICommand
    {
        private readonly StatisticsViewModel _ViewModel;
        public event EventHandler CanExecuteChanged;

        public ReadMeasurementsCommand(StatisticsViewModel viewModel)
        {
            this._ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Task.Run(() => (_ViewModel).ReadData());
        }
    }
}
