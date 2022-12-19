using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using EPiServer.ServiceLocation;
using IntegrationTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.EPiServer.Initialization;
using RegionOrebroLan.EPiServer.Initialization.Configuration;
using RegionOrebroLan.EPiServer.Initialization.Internal;

namespace IntegrationTests
{
	[TestClass]
	public class DataInitializationTest
	{
		#region Methods

		[TestCleanup]
		public void Cleanup()
		{
			Global.CleanupEachTest();
		}

		[TestMethod]
		public void IfInitializeDatabaseIsDisabledAndADatabaseDoesNotExist_ShouldNotCreateADatabase()
		{
			ConfigurationSystem.ApplicationSettings = () => new NameValueCollection
			{
				{ DisableableInitializationConfiguration.KeyPrefix + "*:InitializeDatabase", "false" }
			};

			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseExists());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Configure();

			var dataInitializationModule = initializationEngine.Modules.First(module => module is DataInitialization);

			dataInitializationModule.Initialize(initializationEngine);

			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseExists());

			dataInitializationModule.Uninitialize(initializationEngine);
		}

		[TestMethod]
		public void IfInitializeDatabaseIsNotDisabledAndADatabaseDoesNotExist_ShouldCreateADatabase()
		{
			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseExists());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			initializationEngine.Uninitialize();
		}

		[TestMethod]
		public void IfInitializeDatabaseIsNotDisabledAndADatabaseFileDoesNotExist_ShouldRecreateADatabase()
		{
			DatabaseHelper.CreateEPiServerDatabase();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			Thread.Sleep(500);

			DatabaseHelper.DropEPiServerDatabaseFile();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseFileExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			initializationEngine.Uninitialize();
		}

		[TestMethod]
		public void IfInitializeDatabaseIsNotDisabledAndADatabaseLogFileDoesNotExist_ShouldWorkProperly()
		{
			DatabaseHelper.CreateEPiServerDatabase();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			Thread.Sleep(500);

			DatabaseHelper.DropEPiServerDatabaseLogFile();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseLogFileExists());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			initializationEngine.Uninitialize();
		}

		[TestMethod]
		public void IfInitializeDatabaseIsNotDisabledAndDatabaseFilesDoesNotExist_ShouldRecreateADatabase()
		{
			DatabaseHelper.CreateEPiServerDatabase();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			Thread.Sleep(500);

			DatabaseHelper.DropEPiServerDatabaseFiles();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseFileExists());
			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseLogFileExists());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			initializationEngine.Uninitialize();
		}

		[TestMethod]
		public void IfInitializeDataDirectoryIsDisabled_ShouldNotSetTheDataDirectory()
		{
			ConfigurationSystem.ApplicationSettings = () => new NameValueCollection
			{
				{ DisableableInitializationConfiguration.KeyPrefix + "*:InitializeDataDirectory", "false" }
			};

			TestInitialization.ServiceConfiguration = serviceConfiguration => serviceConfiguration.AddSingleton(Mock.Of<IDatabaseInitializer>());

			Assert.IsNull(AppDomain.CurrentDomain.GetData("DataDirectory"));

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Configure();

			var dataInitializationModule = initializationEngine.Modules.First(module => module is DataInitialization);

			dataInitializationModule.Initialize(initializationEngine);

			Assert.IsNull(AppDomain.CurrentDomain.GetData("DataDirectory"));

			dataInitializationModule.Uninitialize(initializationEngine);
		}

		[TestMethod]
		public void IfInitializeDataDirectoryIsNotDisabled_ShouldSetTheDataDirectory()
		{
			Assert.IsNull(AppDomain.CurrentDomain.GetData("DataDirectory"));

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.AreEqual(Path.Combine(Global.ProjectDirectoryPath, "App_Data"), AppDomain.CurrentDomain.GetData("DataDirectory") as string);

			initializationEngine.Uninitialize();
		}

		#endregion
	}
}