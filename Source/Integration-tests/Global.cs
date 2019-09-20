using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation.AutoDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization.Configuration;
using RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Helpers;

namespace RegionOrebroLan.EPiServer.Initialization.IntegrationTests
{
	[TestClass]
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
	public static class Global
	{
		#region Fields

		// ReSharper disable PossibleNullReferenceException
		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		// ReSharper restore PossibleNullReferenceException

		#endregion

		#region Methods

		[AssemblyCleanup]
		public static void Cleanup()
		{
			DatabaseHelper.DropDatabasesIfTheyExist();
		}

		public static void CleanupEachTest()
		{
			AppDomain.CurrentDomain.SetData("DataDirectory", null);
			ConfigurationSystem.Reset();
			DatabaseHelper.DropEPiServerDatabaseIfItExists();
			TestInitialization.Reset();
		}

		public static InitializationEngine CreateInitializationEngine()
		{
			return new InitializationEngine((IServiceLocatorFactory) null, HostType.TestFramework, new AssemblyList(true).AllowedAssemblies);
		}

		[AssemblyInitialize]
		[CLSCompliant(false)]
		public static void Initialize(TestContext testContext)
		{
			if(testContext == null)
				throw new ArgumentNullException(nameof(testContext));

			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

			DatabaseHelper.DropDatabasesIfTheyExist();
		}

		#endregion
	}
}