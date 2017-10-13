using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace ManyConsole.Ninject.Tests
{
	internal static class DispatcherExtensions
	{
		public static void AssertWorksCorrectly(
			this IConsoleCommandDispatcher dispatcher,
			IConsoleCommand command,
			string[] args)
		{
			var output = new StringWriter();

			dispatcher.DispatchCommand(command, args, output).Should().Be(0);

			if (!command.TraceCommandAfterParse)
			{
				output.ToString().Should().BeNullOrEmpty();
			}
		}

		public static void AssertWorksCorrectly(
			this IConsoleCommandDispatcher dispatcher,
			IReadOnlyCollection<IConsoleCommand> commands,
			string[] args)
		{
			var output = new StringWriter();

			dispatcher.DispatchCommand(commands, args, output).Should().Be(0);
			output.ToString().Should().BeNullOrEmpty();
		}

		public static void AssertWorksWithErrors(
			this IConsoleCommandDispatcher dispatcher,
			IConsoleCommand command,
			string[] args,
			string expectedError)
		{
			var output = new StringWriter();

			var resultCode = dispatcher.DispatchCommand(command, args, output);

			resultCode.Should().Be(-1);
			var factError = output.ToString();

			Regex.IsMatch(factError, expectedError).Should().BeTrue();
		}

		public static void AssertOutputContainsInOrder(this IConsoleCommandDispatcher dispatcher,
			IReadOnlyCollection<IConsoleCommand> commands,
			string[] args,
			string[] expectedParams)
		{
			var output = new StringWriter();

			dispatcher.DispatchCommand(commands, args, output).Should().Be(-1);

			output.ToString().AssertContainsInOrder(expectedParams);
		}

		public static void AssertContainsInOrder(this string input, string[] expectedParams)
		{
			var pattern = string.Join(@"[\s\S]+", expectedParams);
			Regex.IsMatch(input, $@"{pattern}").Should().BeTrue();
		}
	}
}
