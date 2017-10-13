using System.Collections.Generic;
using System.Linq;
using ManyConsole.Ninject;

namespace SampleConsole.Command
{
	public class SimpleConsoleModeCommand : ConsoleModeCommand, IConsoleModeCommand
	{
        public SimpleConsoleModeCommand(IConsoleCommandDispatcher dispatcher)
			: base(dispatcher)
        {
            this.IsCommand("console-mode", "Starts a console interface that allows multiple commands to be run.");
        }

        public override IReadOnlyCollection<IConsoleCommand> GetNextCommands()
        {
            return _commandDispatcher.FindCommands()
				.Where(c => !(c is ConsoleModeCommand))
				.ToList();
        }
    }
}
