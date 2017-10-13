using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	[TestFixture]
	public class ChangingCommandBehaviourTests : DispatcherTests
	{
		private class OverridingCommand : ConsoleCommand
		{
			public OverridingCommand()
			{
				IsCommand("fail-me-maybe");
				HasOption<int>("n=", "number", v => Maybe = v);
			}

			private int? Maybe;

			public override int? OverrideAfterHandlingArgumentsBeforeRun(string[] remainingArguments)
			{
				return Maybe;
			}

			public override int Run(string[] remainingArguments)
			{
				return 0;
			}
		}

		[Test]
		public void OverridingCommand_ShouldWork()
		{
			var command = new OverridingCommand();
			var output = new StringWriter();

			CommandDispatcher.DispatchCommand(command, new[] {"/n", "123"}, output).Should().Be(123);
			output.ToString().Should().BeNullOrEmpty();
		}
	}
}
