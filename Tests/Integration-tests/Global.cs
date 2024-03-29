using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation.AutoDiscovery;
using IntegrationTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization.Configuration;

namespace IntegrationTests
{
	[TestClass]
	public static class Global
	{
		#region Fields

		// ReSharper disable PossibleNullReferenceException
		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		// ReSharper restore PossibleNullReferenceException

		#endregion

		#region Methods

		public static async Task CleanupAsync()
		{
			AppDomainHelper.ResetDataDirectory();
			await DatabaseHelper.DropLocalDatabasesAsync();
			ConfigurationSystem.Reset();
			TestInitialization.Reset();
		}

		public static InitializationEngine CreateInitializationEngine()
		{
			return new InitializationEngine((IServiceLocatorFactory)null, HostType.TestFramework, new AssemblyList(true).AllowedAssemblies);
		}

		[AssemblyInitialize]
		public static void Initialize(TestContext _)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
		}

		#endregion
	}
}