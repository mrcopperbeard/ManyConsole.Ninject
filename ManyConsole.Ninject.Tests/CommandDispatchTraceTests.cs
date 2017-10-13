using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	[TestFixture]
	public partial class CommandDispatchTraceTests : DispatcherTests
	{
		[Test]
		public void CommandDispatch_ShouldWriteExpectedTrace()
		{
			var expected = @"
Executing thecommand (One-line description):
    FieldA : abc
    PropertyB : def
    PropertyC : null
    PropertyD : 1, 2, 3

";

			var output = new StringWriter();
			CommandDispatcher.DispatchCommand(new SomeCommand(), new[] {"thecommand"}, output);

			output.ToString().Should().Be(expected);
		}

		[Test]
		public void CommandDispatch_WithoutRequiredParameter_ShouldShowFriendlyErrorInfo()
		{
			var output = new StringWriter();

			CommandDispatcher.DispatchCommand(new SomeCommandWithAParameter(), new[] {"some", "/a"}, output);

			var outputMessage = output.ToString().ToLower();

			outputMessage.AssertContainsInOrder(new []
			{
				"missing required value for option '/a'",
				"expected usage:",
			});

			outputMessage.Contains("ndesk.options").Should().BeFalse();
			outputMessage.Contains("exception").Should().BeFalse();
		}
	}
}
