using System;
using ManyConsole.Ninject;

namespace SampleConsole.Command
{
	public class MattsCommand : ConsoleCommand, IConsoleCommand
	{
        public string Baz;

        public MattsCommand()
        {
            this.IsCommand("matts");
            this.HasOption("b|baz=", "baz", v => Baz = v);
            AllowsAnyAdditionalArguments("<foo1> <foo2> <fooN> where N is bar");
        }
        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine("baz is " + (Baz ?? "<null>"));
            Console.WriteLine("foos are: " + String.Join(", ", remainingArguments));
            return 0;
        }
    }
}
