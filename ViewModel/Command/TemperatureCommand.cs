using System;
using System.Windows.Input;

namespace EngineeringThesis.ViewModel.Commands
{
    public class TemperatureCommand : ICommand
    {
        private readonly ButtonsViewModel _ViewModel;
        public event EventHandler CanExecuteChanged;

        public TemperatureCommand(ButtonsViewModel viewModel)
        {
            this._ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;           
        }

        public void Execute(object parameter)
        {
            this._ViewModel.TemperatureWindowCommand();
        }
    }
}
