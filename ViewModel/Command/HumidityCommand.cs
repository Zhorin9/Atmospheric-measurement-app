﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EngineeringThesis.ViewModel.Commands
{
    public class HumidityCommand : ICommand
    {
        private readonly ButtonsViewModel _ViewModel;
        public event EventHandler CanExecuteChanged;

        public HumidityCommand(ButtonsViewModel viewModel)
        {
            this._ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._ViewModel.HumidityWindowCommand();
        }
    }
}
