using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	[TestFixture]
	public class DisplayHelpTests : DispatcherTests
	{
		private const string CommandA = "command-a";
		private const string CommandB = "command-b";
		private const string CommandC = "command-c";
		private const string DescriptionA = "oneline description a";
		private const string DescriptionB = "oneline description b";
		private const string DescriptionC = "oneline description c";

		private const string LondDescriptionC = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit,
					sed do eiusmod tempor incididunt ut labore et dolore magna
					aliqua. Ut enim ad minim veniam, quis nostrud exercitation
					ullamco laboris nisi ut aliquip ex ea commodo consequat.
					Duis aute irure dolor in reprehenderit in voluptate velit
					esse cillum dolore eu fugiat nulla pariatur. Excepteur sint
					occaecat cupidatat non proident, sunt in culpa qui officia
					deserunt mollit anim id est laborum.";

		private static readonly List<IConsoleCommand> commands = new List<IConsoleCommand>
		{
			new TestCommand().IsCommand(CommandA, DescriptionA),
			new TestCommand().IsCommand(CommandB, DescriptionB),
		};

		[TestCase]
		[TestCase("help")]
		public void CommandDispatch_ForAllCommands_ShouldShowDigestHelp(params string[] args)
		{
			CommandDispatcher.AssertOutputContainsInOrder(commands, args, new []{CommandA, DescriptionA, CommandB, DescriptionB});
		}

		[TestCase("command-c", "/?")]
		[TestCase("help", "command-c")]
		public void CommandDispatch_ForConcreteCommand_ShouldShowFullHelp(params string[] args)
		{
			var commandC = new TestCommand()
				.IsCommand(CommandC, DescriptionC)
				.HasLongDescription(LondDescriptionC)
				.HasAdditionalArguments(0, "<remaining> <args>")
				.HasOption("o|option=", "option description", v => { });

			commands.Add(commandC);

			CommandDispatcher.AssertOutputContainsInOrder(
				commands,
				args,
				new[] { CommandC, DescriptionC, LondDescriptionC, commandC.RemainingArgumentsHelpText, "-o", "--option", "option description"});
		}
	}
}
