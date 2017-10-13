using System;
using ManyConsole.Ninject;

namespace SampleConsole.Command
{
	public class ThrowExceptionCommand : ConsoleCommand, IConsoleCommand
	{
        public ThrowExceptionCommand()
        {
            this.IsCommand("throw-exception", "Throws an exception.");
            this.HasOption("m=", "Error message to be thrown.", v => Message = v);
        }

        public string Message = "Command ThrowException threw an exception with this message.";

        public override int Run(string[] remainingArguments)
        {
            throw new Exception(Message);
        }
    }
}
