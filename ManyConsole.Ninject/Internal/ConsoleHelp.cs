using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ManyConsole.Ninject.Internal
{
    public class ConsoleHelp
    {
        public static void ShowSummaryOfCommands(IEnumerable<IConsoleCommand> commands, TextWriter console)
        {
            console.WriteLine();
            console.WriteLine("Available commands are:");
            console.WriteLine();

            const string helpCommand = "help <name>";

            var commandList = commands.ToList();
            var n = commandList.Select(c => c.Command).Concat(new [] { helpCommand}).Max(c => c.Length) + 1;
            var commandFormatString = "    {0,-" + n + "}- {1}";

            foreach (var command in commandList)
            {
                console.WriteLine(commandFormatString, command.Command, command.OneLineDescription);
            }
            console.WriteLine();
            console.WriteLine(commandFormatString, helpCommand, "For help with one of the above commands");
            console.WriteLine();
        }

        public static void ShowCommandHelp(IConsoleCommand selectedCommand, TextWriter console, bool skipExeInExpectedUsage = false)
        {
            var haveOptions = selectedCommand.GetActualOptions().Count > 0;

            console.WriteLine();
            console.WriteLine("'" + selectedCommand.Command + "' - " + selectedCommand.OneLineDescription);
            console.WriteLine();

            if (!string.IsNullOrEmpty(selectedCommand.LongDescription))
            {
                console.WriteLine(selectedCommand.LongDescription);
                console.WriteLine();
            }

            console.Write("Expected usage:");

            if (!skipExeInExpectedUsage)
            {
                console.Write(" " + AppDomain.CurrentDomain.FriendlyName);
            }

            console.Write(" " + selectedCommand.Command);

            if (haveOptions)
                console.Write(" <options> ");

            console.WriteLine(selectedCommand.RemainingArgumentsHelpText);

            if (haveOptions)
            {
                console.WriteLine("<options> available:");
                selectedCommand.GetActualOptions().WriteOptionDescriptions(console);
            }
            console.WriteLine();
        }

        public static void ShowParsedCommand(IConsoleCommand consoleCommand, TextWriter consoleOut)
        {
            if (!consoleCommand.TraceCommandAfterParse)
            {
                return;
            }

            var skippedProperties = new []{
                "Command",
                "OneLineDescription",
                "LongDescription",
                "Options",
                "TraceCommandAfterParse",
                "RemainingArgumentsCount",
                "RemainingArgumentsHelpText",
                "RequiredOptions"
            };

            var properties = consoleCommand.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !skippedProperties.Contains(p.Name));

            var fields = consoleCommand.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !skippedProperties.Contains(p.Name));

            var allValuesToTrace = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(consoleCommand, new object[0]);
                allValuesToTrace[property.Name] = value?.ToString() ?? "null";
            }

            foreach (var field in fields)
            {
                allValuesToTrace[field.Name] = MakeObjectReadable(field.GetValue(consoleCommand));
            }

            consoleOut.WriteLine();

            string introLine = $"Executing {consoleCommand.Command}";

            if (string.IsNullOrEmpty(consoleCommand.OneLineDescription))
                introLine = introLine + ":";
            else
                introLine = introLine + " (" + consoleCommand.OneLineDescription + "):";

            consoleOut.WriteLine(introLine);
            
            foreach(var value in allValuesToTrace.OrderBy(k => k.Key))
                consoleOut.WriteLine("    " + value.Key + " : " + value.Value);

            consoleOut.WriteLine();
        }

	    private static string MakeObjectReadable(object value)
        {
            string readable;

            if (value is IEnumerable && !(value is string))
            {
                readable = "";
                var separator = "";

                foreach (var member in (IEnumerable) value)
                {
                    readable += separator + MakeObjectReadable(member);
                    separator = ", ";
                }
            }
            else if (value != null)
                readable = value.ToString();
            else
                readable = "null";
            return readable;
        }
    }
}
