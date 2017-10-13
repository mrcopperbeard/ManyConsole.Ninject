using System;
using System.Collections.Generic;
using ManyConsole.Ninject;

namespace SampleConsole.Command
{
	public class StatefulConsoleModeCommand : ConsoleModeCommand, IConsoleModeCommand
	{
        public int Count;

        public StatefulConsoleModeCommand(IConsoleCommandDispatcher dispatcher)
			: base(dispatcher)
        {
            IsCommand("stateful", "Starts a stateful console interface that allows multiple commands to be run.");
        }

        public override void WritePromptForCommands()
        {
            Console.WriteLine("You have seen this console {0} times.", Count++);

            base.WritePromptForCommands();
        }

        public override IReadOnlyCollection<IConsoleCommand> GetNextCommands()
        {
            return new List<IConsoleCommand> {new GetTimeCommand(), new MattsCommand(), new DumpEmlFilesCommand(), new DumpEmlFilesCommand()};
        }
    }
}
