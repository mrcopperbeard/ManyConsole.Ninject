using System;
using System.Collections.Generic;
using System.IO;
using ManyConsole.Ninject.Internal;
using NDesk.Options;

namespace ManyConsole.Ninject
{
	public interface IConsoleModeCommand
	{
		void Init(
			Func<IEnumerable<IConsoleCommand>> commandSource,
			TextWriter outputStream = null,
			TextReader inputStream = null,
			string friendlyContinueText = null,
			OptionSet options = null);

		/// <summary>
		/// Writes to the console to prompt the user for their next command.
		/// Is skipped if commands are being ran without user interaction.
		/// </summary>
		void WritePromptForCommands();

		/// <summary>
		/// Runs to get the next available commands
		/// </summary>
		/// <returns></returns>
		IReadOnlyCollection<IConsoleCommand> GetNextCommands();

		void SetConsoleRedirectionDetection(IConsoleRedirectionDetection consoleRedirectionDetection);
	}
}