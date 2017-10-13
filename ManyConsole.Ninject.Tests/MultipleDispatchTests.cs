using System;
using System.IO;
using NDesk.Options;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	public partial class MultipleDispatchTests : DispatcherTests
	{
		[Test]
		public void MultipleDispatch_ShouldNotInterferWithEachOther()
		{
			var trace = new StringWriter();
			var expectedTraceParts = new []
			{
				"You walk to 1, 2 and find a maze of twisty little passages, all alike.",
				"You walk to 3, 0 and find a maze of twisty little passages, all alike.",
				"You walk to 0, 4 and find a maze of twisty little passages, all alike.",
				"You walk to 0, 0 and find a maze of twisty little passages, all alike.",
			};

			CommandDispatcher.DispatchCommand(SomeProgram.GetCommands(trace), new[] {"move", "-x", "1", "-y", "2"}, new StringWriter());
			CommandDispatcher.DispatchCommand(SomeProgram.GetCommands(trace), new[] { "move", "-x", "3" }, new StringWriter());
			CommandDispatcher.DispatchCommand(SomeProgram.GetCommands(trace), new[] { "move", "-y", "4" }, new StringWriter());
			CommandDispatcher.DispatchCommand(SomeProgram.GetCommands(trace), new[] { "move" }, new StringWriter());

			trace.ToString().AssertContainsInOrder(expectedTraceParts);
		}
	}
}
