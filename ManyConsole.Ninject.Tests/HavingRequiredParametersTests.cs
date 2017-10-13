using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	[TestFixture]
	public class HavingRequiredParametersTests : DispatcherTests
	{
		[Test]
		public void CommandDispatch_WithRequiredParameters_ShouldNotThrowAndHaveCorrectValue()
		{
			var command = new TestCommand()
				.IsCommand("required", "This command has a required parameter")
				.HasOption("ignored=", "An extra option.", v => { })
				.HasRequiredOption("f|foo=", "This foo to use.", v => v.Should().Be("bar"))
				.SkipsCommandSummaryBeforeRunning();

			var args = new[] {"required", "-foo", "bar"};

			CommandDispatcher.AssertWorksCorrectly(command, args);
		}

		[Test]
		public void CommandDispatch_ShouldParseIntCorrectly()
		{
			var command = new TestCommand()
				.IsCommand("parse-int")
				.HasRequiredOption<int>("value=", "The integer value", v => v.Should().Be(42));

			var args = new[] { "parse-int", "-value", "42" };

			CommandDispatcher.AssertWorksCorrectly(command, args);
		}

		[Test]
		public void CommandDispatch_WithoutRequiredParameters_ShouldGiveErrorOutput()
		{
			var command = new TestCommand()
				.IsCommand("command")
				.HasRequiredOption<int>("value=", "The integer value", v => { });

			var args = new[] { "command" };

			CommandDispatcher.AssertWorksWithErrors(command, args, "Missing option: ");
		}
	}
}
