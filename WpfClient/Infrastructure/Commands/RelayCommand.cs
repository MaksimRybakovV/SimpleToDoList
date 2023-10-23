using System;
using WpfClient.Infrastructure.Commands.Base;

namespace WpfClient.Infrastructure.Commands
{
    internal class RelayCommand : BaseCommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter) => canExecute?.Invoke(parameter!) ?? true;

        public override void Execute(object? parameter) => execute(parameter!);
    }
}
