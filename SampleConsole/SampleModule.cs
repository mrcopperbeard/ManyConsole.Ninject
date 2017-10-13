using System;
using ManyConsole.Ninject;
using Ninject.Modules;
using SampleConsole.Command;

namespace SampleConsole
{
	internal class SampleModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IConsoleCommand>().To<DumpEmlFilesCommand>();
			Bind<IConsoleCommand>().To<DumpEmlFilesCommand>();
			Bind<IConsoleCommand>().To<EchoStringsCommand>();
			Bind<IConsoleCommand>().To<GetTimeCommand>();
			Bind<IConsoleCommand>().To<MattsCommand>();
			Bind<IConsoleCommand>().To<ThrowExceptionCommand>();
			Bind<IConsoleModeCommand>().To<StatefulConsoleModeCommand>();
			Bind<IConsoleModeCommand>().To<SimpleConsoleModeCommand>();
		}
	}
}
