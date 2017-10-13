using System;
using FluentAssertions;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	public class SingleCommandDispatchTests : DispatcherTests
	{
		[TestCase]
		[TestCase("Example")]
		public void CommandDispatch_WithoutParameters_ShouldNotFail(params string[] args)
		{
			var command = new TestCommand()
				.IsCommand("Example")
				.HasOption("f|foo=", "This foo to use.", v => v.Should().BeNull())
				.SkipsCommandSummaryBeforeRunning();

			CommandDispatcher.AssertWorksCorrectly(command, args);
		}

		[TestCase("/f=bar")]
		[TestCase("Example", "/f=bar")]
		public void CommandDispatch_WithParameters_ShouldParseItCorrectly(params string[] args)
		{
			var command = new TestCommand()
				.IsCommand("Example")
				.HasOption("f|foo=", "This foo to use.", v => v.Should().Be("bar"))
				.SkipsCommandSummaryBeforeRunning();

			CommandDispatcher.AssertWorksCorrectly(command, args);
		}
	}
}
