using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Commands
{
    public class SaveConfigurationParametersCommand : ICommand
    {
        private readonly IEnumerable<IElementViewModel> _parameters;
        private readonly Panel _parametersPanel;

        public SaveConfigurationParametersCommand(IEnumerable<IElementViewModel> parameters, Panel parametersPanel)
        {
            _parameters = parameters;
            _parametersPanel = parametersPanel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object panelObject)
        {
            //foreach (var parameter in _parameters)
            //{
            //    parameter.Save(_parametersPanel);
            //}
        }

        public event EventHandler CanExecuteChanged;
    }
}