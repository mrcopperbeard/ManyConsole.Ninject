using System;
using Ninject.Modules;

namespace ManyConsole.Ninject
{
	/// <summary>
	/// Ninject loading module.
	/// </summary>
	public class ManyConsoleModule : NinjectModule
	{
		/// <summary>
		/// Loading dependencies.
		/// </summary>
		public override void Load()
		{
			Bind<IConsoleCommandDispatcher>().To<ConsoleCommandDispatcher>();
		}
	}
}
