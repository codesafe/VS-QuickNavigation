﻿
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;

namespace VS_QuickNavigation
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class QuickSymbolCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0103;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("ad64a987-3060-494b-94c1-07bab75f9da3");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package package;

		private QuickMethodToolWindow window;

		/// <summary>
		/// Initializes a new instance of the <see cref="QuickSymbolCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private QuickSymbolCommand(Package package)
		{
			if (package == null)
			{
				throw new ArgumentNullException("package");
			}

			this.package = package;

			OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				var menuCommandID = new CommandID(CommandSet, CommandId);
				var menuItem = new MenuCommand(this.ShowToolWindow, menuCommandID);
				commandService.AddCommand(menuItem);
			}

			window = new QuickMethodToolWindow(true,
					Data.SymbolData.ESymbolType.Namespace |
					Data.SymbolData.ESymbolType.Struct |
					Data.SymbolData.ESymbolType.Class |
					Data.SymbolData.ESymbolType.Interface |
					Data.SymbolData.ESymbolType.Macro |
					Data.SymbolData.ESymbolType.Enumerator |
					Data.SymbolData.ESymbolType.Enumeration |
					Data.SymbolData.ESymbolType.Method |
					Data.SymbolData.ESymbolType.MethodPrototype |
					Data.SymbolData.ESymbolType.Field |
					Data.SymbolData.ESymbolType.Property);
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static QuickSymbolCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new QuickSymbolCommand(package);
		}

		/// <summary>
		/// Shows the tool window when the menu item is clicked.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		private void ShowToolWindow(object sender, EventArgs e)
		{
			window.OpenDialog();
		}
	}
}