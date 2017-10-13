using System;
using NDesk.Options;

namespace ManyConsole.Ninject
{
	public interface IConsoleCommand
	{
		string Command { get; }
		string OneLineDescription { get; }
		string LongDescription { get; }
		OptionSet Options { get; }
		bool TraceCommandAfterParse { get; }
		int? RemainingArgumentsCount { get; }
		string RemainingArgumentsHelpText { get; }
		ConsoleCommand IsCommand(string command, string oneLineDescription = "");
		ConsoleCommand HasLongDescription(string longDescription);
		ConsoleCommand HasAdditionalArguments(int? count = 0, string helpText = "");
		ConsoleCommand AllowsAnyAdditionalArguments(string helpText = "");
		ConsoleCommand SkipsCommandSummaryBeforeRunning();
		ConsoleCommand HasOption(string prototype, string description, Action<string> action);
		ConsoleCommand HasRequiredOption(string prototype, string description, Action<string> action);
		ConsoleCommand HasOption<T>(string prototype, string description, Action<T> action);
		ConsoleCommand HasRequiredOption<T>(string prototype, string description, Action<T> action);
		ConsoleCommand HasOption(string prototype, string description, OptionAction<string, string> action);
		ConsoleCommand HasOption<TKey, TValue>(string prototype, string description, OptionAction<TKey, TValue> action);
		void CheckRequiredArguments();
		int? OverrideAfterHandlingArgumentsBeforeRun(string[] remainingArguments);
		int Run(string[] remainingArguments);
		OptionSet GetActualOptions();
	}
}