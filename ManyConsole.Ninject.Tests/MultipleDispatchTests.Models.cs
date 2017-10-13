using System.Collections.Generic;
using System.IO;
using NDesk.Options;

namespace ManyConsole.Ninject.Tests
{
	public partial class MultipleDispatchTests
	{
		private class SomeProgram
		{
			public static IReadOnlyCollection<IConsoleCommand> GetCommands(StringWriter trace)
			{
				return new List<IConsoleCommand>
				{
					new CoordinateCommand(trace)
				};
			}
		}

		private class CoordinateCommand : ConsoleCommand
		{
			readonly TextWriter _recorder;

			public CoordinateCommand(TextWriter recorder)
			{
				_recorder = recorder;

				this.IsCommand("move");
				Options = new OptionSet()
				{
					{"x=", "Coordinate along the x axis.", v => X = int.Parse(v)},
					{"y=", "Coordinate along the y axis.", v => Y = int.Parse(v)},
				};
			}

			public int X;
			public int Y;

			public override int Run(string[] remainingArguments)
			{
				_recorder.WriteLine("You walk to {0}, {1} and find a maze of twisty little passages, all alike.", X, Y);
				return 0;
			}
		}
	}
}