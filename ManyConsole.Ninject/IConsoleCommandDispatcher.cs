using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ManyConsole.Ninject
{
	public interface IConsoleCommandDispatcher
	{
		int DispatchCommand(IConsoleCommand command, string[] arguments, TextWriter consoleOut);
		int DispatchCommand(IReadOnlyCollection<IConsoleCommand> commands, string[] arguments, TextWriter consoleOut, bool skipExeInExpectedUsage = false);
		IReadOnlyCollection<IConsoleCommand> FindCommands();
	}
}