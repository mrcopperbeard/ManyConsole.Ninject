using System;

using ManyConsole.Ninject;

using Ninject;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
			var kernel = new StandardKernel(
				new ManyConsoleModule(),
				new SampleModule());

			// get dispatcher from DI core.
	        var dispatcher = kernel.Get<IConsoleCommandDispatcher>();

	        var commands = dispatcher.FindCommands();

			// then run them.
			var result = dispatcher.DispatchCommand(commands, args, Console.Out);

			Console.WriteLine($"result is {result}");
	        Console.ReadLine();
        }
    }
}
