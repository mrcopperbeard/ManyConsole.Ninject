using System;
using FluentAssertions;
using NDesk.Options;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	[TestFixture]
	public class PropertyOverrideTests : DispatcherTests
	{
		private class OverwriteCommand : ConsoleCommand
		{
			private int A;
			private int B;
			public string Result;

			public OverwriteCommand()
			{
				IsCommand("foo", "bar");
				HasOption<int>("A=", "first value", v => A = v);
				SkipsCommandSummaryBeforeRunning();

				var optionSet = new OptionSet();

				Options = optionSet;
				optionSet.Add<int>("B=", "second option", v => B = v);
			}

			public override int Run(string[] remainingArguments)
			{
				Result = A + "," + B;

				return 0;
			}
		}

		[Test]
		public void ComandDispatch_ShouldOverwriteProperties()
		{
			var command = new OverwriteCommand();
			var args = new[] {"/A", "1", "/B", "2"};

			CommandDispatcher.AssertWorksCorrectly(command, args);
			command.Result.Should().Be("1,2");
		}
	}
}
