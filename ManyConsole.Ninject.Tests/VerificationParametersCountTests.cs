using System;

using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	public class VerificationParametersCountTests : DispatcherTests
	{
		[Test]
		public void CommandDispatch_WithCorrectCountOfParameters_ShouldWorkFine()
		{
			var command = new TestCommand()
				.IsCommand("command")
				.HasAdditionalArguments(5);

			var args = new[] {"command", "1", "2", "3", "4", "5"};

			CommandDispatcher.AssertWorksCorrectly(command, args);
		}

		[Test]
		public void CommandDispatch_WithMoreThanRequiredParamentes_ShouldWorkWithErrors()
		{
			var command = new TestCommand()
				.IsCommand("command")
				.HasAdditionalArguments(5);

			var args = new[] { "command", "1", "2", "3", "4", "5", "6", "7", "8" };

			CommandDispatcher.AssertWorksWithErrors(command, args, "Extra parameters specified: 6, 7, 8");
		}

		[Test]
		public void CommandDispatch_WithLessThanRequiredParamentes_ShouldWorkWithErrors()
		{
			var command = new TestCommand()
				.IsCommand("command")
				.HasAdditionalArguments(5);

			var args = new[] { "command" };

			CommandDispatcher.AssertWorksWithErrors(command, args, "Invalid number of arguments-- expected 5 more.");
		}
	}
}
