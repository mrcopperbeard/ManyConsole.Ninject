using System.Collections.Generic;
using System.ComponentModel;

namespace ManyConsole.Ninject.Tests
{
	public partial class CommandDispatchTraceTests
	{
		private class SomeCommand : ConsoleCommand
		{
			public SomeCommand()
			{
				this.IsCommand("thecommand", "One-line description");
				PropertyB = "def";
			}

			public string FieldA = "abc";
			public string PropertyB { get; set; }
			public int? PropertyC { get; set; }
			public IEnumerable<int> PropertyD = new [] {1, 2, 3};

			public override int Run(string[] remainingArguments)
			{
				return 0;
			}
		}
		private class SomeCommandWithAParameter : ConsoleCommand
		{
			public SomeCommandWithAParameter()
			{
				IsCommand("some");
				HasOption("a=", "a parameter", v => { });
			}

			public override int Run(string[] remainingArguments)
			{
				return 0;
			}
		}

		private class SomeCommandThrowingAnException : ConsoleCommand
		{
			public SomeCommandThrowingAnException()
			{
				this.IsCommand("some");
			}

			public override int Run(string[] remainingArguments)
			{
				throw new InvalidAsynchronousStateException();
			}
		}
	}
}