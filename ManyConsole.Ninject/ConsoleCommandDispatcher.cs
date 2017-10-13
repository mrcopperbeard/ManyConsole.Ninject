using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ManyConsole.Ninject.Internal;

using Ninject;
using Ninject.Syntax;

namespace ManyConsole.Ninject
{
    public class ConsoleCommandDispatcher : IConsoleCommandDispatcher
    {
		/// <summary>
		/// Dependency injection core.
		/// </summary>
	    private readonly IResolutionRoot _resolutionRoot;

	    public ConsoleCommandDispatcher(IResolutionRoot resolutionRoot)
	    {
		    _resolutionRoot = resolutionRoot;
	    }

	    public int DispatchCommand(IConsoleCommand command, string[] arguments, TextWriter consoleOut)
        {
            return DispatchCommand(new [] {command}, arguments, consoleOut);
        }

        public int DispatchCommand(IReadOnlyCollection<IConsoleCommand> commands, string[] arguments, TextWriter consoleOut, bool skipExeInExpectedUsage = false)
        {
            IConsoleCommand selectedCommand = null;

            var console = consoleOut;

            foreach (var command in commands)
            {
                ValidateConsoleCommand(command);
            }

            try
            {
                List<string> remainingArguments;
                if (commands.Count == 1)
                {
                    selectedCommand = commands.First();

                    if (arguments.Any() && string.Equals(arguments.First(), selectedCommand.Command, StringComparison.OrdinalIgnoreCase))
                    {
                        remainingArguments = selectedCommand.GetActualOptions().Parse(arguments.Skip(1));
                    }
                    else
                    {
                        remainingArguments = selectedCommand.GetActualOptions().Parse(arguments);
                    }
                }
                else
                {
                    if (!arguments.Any())
                        throw new ConsoleHelpAsException("No arguments specified.");

                    if (arguments[0].Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedCommand = GetMatchingCommand(commands, arguments.Skip(1).FirstOrDefault());

                        if (selectedCommand == null)
                            ConsoleHelp.ShowSummaryOfCommands(commands, console);
                        else
                            ConsoleHelp.ShowCommandHelp(selectedCommand, console, skipExeInExpectedUsage);

                        return -1;
                    }

                    selectedCommand = GetMatchingCommand(commands, arguments.First());

	                remainingArguments = selectedCommand?.GetActualOptions().Parse(arguments.Skip(1)) ?? throw new ConsoleHelpAsException("Command name not recognized.");
                }

                selectedCommand.CheckRequiredArguments();

                CheckRemainingArguments(remainingArguments, selectedCommand.RemainingArgumentsCount);

                var preResult = selectedCommand.OverrideAfterHandlingArgumentsBeforeRun(remainingArguments.ToArray());

                if (preResult.HasValue)
                    return preResult.Value;

                ConsoleHelp.ShowParsedCommand(selectedCommand, console);

                return selectedCommand.Run(remainingArguments.ToArray());
            }
            catch (ConsoleHelpAsException e)
            {
                return DealWithException(e, console, skipExeInExpectedUsage, selectedCommand, commands);
            }
            catch (NDesk.Options.OptionException e)
            {
                return DealWithException(e, console, skipExeInExpectedUsage, selectedCommand, commands);
            }
        }

        private static int DealWithException(
			Exception e,
			TextWriter console,
			bool skipExeInExpectedUsage,
			IConsoleCommand selectedCommand,
			IEnumerable<IConsoleCommand> commands)
        {
            if (selectedCommand != null)
            {
                console.WriteLine();
                console.WriteLine(e.Message);
                ConsoleHelp.ShowCommandHelp(selectedCommand, console, skipExeInExpectedUsage);
            }
            else
            {
                ConsoleHelp.ShowSummaryOfCommands(commands, console);
            }

            return -1;
        }
  
        private static IConsoleCommand GetMatchingCommand(IEnumerable<IConsoleCommand> command, string name)
        {
            return command.FirstOrDefault(c => c.Command.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateConsoleCommand(IConsoleCommand command)
        {
            if (string.IsNullOrEmpty(command.Command))
            {
                throw new InvalidOperationException($"Command {command.GetType().Name} did not call IsCommand in its constructor to indicate its name and description.");
            }
        }

        private static void CheckRemainingArguments(List<string> remainingArguments, int? parametersRequiredAfterOptions)
        {
            if (parametersRequiredAfterOptions.HasValue)
                ConsoleUtil.VerifyNumberOfArguments(remainingArguments.ToArray(),
                    parametersRequiredAfterOptions.Value);
        }

        public IReadOnlyCollection<IConsoleCommand> FindCommands()
        {
	        var commands = _resolutionRoot.GetAll<IConsoleCommand>().ToList();
			commands.AddRange(_resolutionRoot.GetAll<IConsoleModeCommand>().Select(c => (IConsoleCommand)c));

			return commands;
        }
    }
}
