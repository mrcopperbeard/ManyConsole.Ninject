using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole.Ninject.Internal;
using NDesk.Options;

namespace ManyConsole.Ninject
{
    public class ConsoleModeCommand : ConsoleCommand, IConsoleModeCommand
    {
	    public const string FriendlyContinuePrompt = "Enter a command or 'x' to exit or '?' for help";
		private TextReader _inputStream;
        private TextWriter _outputStream;
	    private Func<IReadOnlyCollection<IConsoleCommand>> _commandSource;
        private string _continuePrompt;
	    private IConsoleRedirectionDetection _redirectionDetector = new ConsoleRedirectionDetection();

	    protected readonly IConsoleCommandDispatcher _commandDispatcher;

		public ConsoleModeCommand(IConsoleCommandDispatcher commandDispatcher)
        {
	        _commandDispatcher = commandDispatcher;
        }

		// TODO: Clean upp the code and kick this workaround.
	    [Obsolete("Its preferred to override methods on ConsoleModeCommand and use the shorter constructor.")]
        public void Init(
            Func<IEnumerable<IConsoleCommand>> commandSource,
			TextWriter outputStream = null,
            TextReader inputStream = null,
            string friendlyContinueText = null,
            OptionSet options = null)
        {

	        _inputStream = inputStream ?? Console.In;
            _outputStream = outputStream ?? Console.Out;

            IsCommand("run-console", "Run in console mode, treating each line of console input as a command.");

            Options = options ?? Options;  //  added per request from https://github.com/fschwiet/ManyConsole/issues/7

            _commandSource = () =>
            {
                var commands = commandSource();
                return commands.Where(c => !(c is ConsoleModeCommand)).ToList();  // don't cross the beams
            };

            _continuePrompt = friendlyContinueText ?? FriendlyContinuePrompt;
        }

        /// <summary>
        /// Writes to the console to prompt the user for their next command.
        /// Is skipped if commands are being ran without user interaction.
        /// </summary>
        public virtual void WritePromptForCommands()
        {
            if (!string.IsNullOrEmpty(_continuePrompt))
                _outputStream.WriteLine(_continuePrompt);
        }

        /// <summary>
        /// Runs to get the next available commands
        /// </summary>
        /// <returns></returns>
        public virtual IReadOnlyCollection<IConsoleCommand> GetNextCommands()
        {
            return _commandSource();
        }

        public override int Run(string[] remainingArguments)
        {
	        var isInputRedirected = _redirectionDetector.IsInputRedirected();

            if (!isInputRedirected)
            {
                WritePromptForCommands();
            }

            var haveError = false;
            var input = _inputStream.ReadLine();

	        while (!input.Trim().Equals("x"))
            {
                if (input.Trim().Equals("?"))
                {
                    ConsoleHelp.ShowSummaryOfCommands(GetNextCommands(), _outputStream);
                }
                else
                {
                    var args = CommandLineParser.Parse(input);

                    var result = _commandDispatcher.DispatchCommand(GetNextCommands(), args, _outputStream, true);
                    if (result != 0)
                    {
                        haveError = true;

                        if (isInputRedirected)
                            return result;
                    }
                }
                
                if (!isInputRedirected)
                {
                    _outputStream.WriteLine();
	                WritePromptForCommands();
				}

                input = _inputStream.ReadLine();
            }

            return haveError ? -1 : 0;
        }

        public void SetConsoleRedirectionDetection(IConsoleRedirectionDetection consoleRedirectionDetection)
        {
            _redirectionDetector = consoleRedirectionDetection;
        }
    }
}
