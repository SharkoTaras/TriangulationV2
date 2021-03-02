using System;
using System.Windows.Input;

namespace Triangulation.UI.ViewModels.Base
{
    internal class RelayCommand : ICommand
    {
        private Action action = null;

        private Action<object> parametrizedAction = null;

        public RelayCommand(Action<object> action)
        {
            parametrizedAction = action;
        }

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (action is null)
            {
                parametrizedAction(parameter);
            }
            else
            {
                action();
            }
        }
    }
}
