using System;
using Ninject;
using NUnit.Framework;

namespace ManyConsole.Ninject.Tests
{
	/// <summary>
	/// Abstract class initializing command dispatcher.
	/// </summary>
	public abstract class DispatcherTests
	{
		/// <summary>
		/// Testing command dispatcher.
		/// </summary>
		protected IConsoleCommandDispatcher CommandDispatcher { get; private set; }

		/// <summary>
		/// Initializing command dispatcher.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			var kernel = new StandardKernel(new ManyConsoleModule());

			CommandDispatcher = kernel.Get<IConsoleCommandDispatcher>();
		}
	}
}
