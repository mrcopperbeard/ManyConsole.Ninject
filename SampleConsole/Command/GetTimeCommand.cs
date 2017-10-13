using System;
using ManyConsole.Ninject;

namespace SampleConsole.Command
{
	/// <summary>
    /// As an example of ManyConsole usage, get-time is meant to show the simplest case possible usage.
    /// </summary>
    public class GetTimeCommand : ConsoleCommand, IConsoleCommand
	{
        public GetTimeCommand()
        {
            IsCommand("get-time", "Returns the current system time.");
        }

        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine(DateTime.UtcNow);

            return 0;
        }
    }
}
