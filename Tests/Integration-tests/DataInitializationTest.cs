using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

		[ClassCleanup]
		public static async Task ClassCleanup()
		{
			await Global.CleanupAsync();
		}

		[ClassInitialize]
		public static async Task ClassInitialize(TestContext _)
		{
			await Global.CleanupAsync();
		}

		[TestMethod]
		public async Task IfInitializeDatabaseIsDisabledAndADatabaseDoesNotExist_ShouldNotCreateADatabase()
		{
			ConfigurationSystem.ApplicationSettings = () => new NameValueCollection
			{
				{ DisableableInitializationConfiguration.KeyPrefix + "*:InitializeDatabase", "false" }
			};

			Assert.IsFalse(await DatabaseHelper.EPiServerDatabaseExistsAsync());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Configure();

			var dataInitializationModule = initializationEngine.Modules.First(module => module is DataInitialization);

			dataInitializationModule.Initialize(initializationEngine);

			Assert.IsFalse(await DatabaseHelper.EPiServerDatabaseExistsAsync());

			dataInitializationModule.Uninitialize(initializationEngine);
		}

		[TestMethod]
		public async Task IfInitializeDatabaseIsNotDisabledAndADatabaseDoesNotExist_ShouldCreateADatabase()
		{
			Assert.IsFalse(await DatabaseHelper.EPiServerDatabaseExistsAsync());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(await DatabaseHelper.EPiServerDatabaseExistsAsync());

			initializationEngine.Uninitialize();
		}

		[TestMethod]
		public async Task IfInitializeDatabaseIsNotDisabledAndADatabaseFileDoesNotExist_ShouldRecreateADatabase()
		{
			await Task.CompletedTask;

			Assert.Fail("Maybe we should remove this test.");

			//DatabaseHelper.CreateEPiServerDatabase();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			//Thread.Sleep(500);

			//DatabaseHelper.DropEPiServerDatabaseFile();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			//Assert.IsFalse(DatabaseHelper.EPiServerDatabaseFileExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			//var initializationEngine = Global.CreateInitializationEngine();

			//initializationEngine.Initialize();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			//initializationEngine.Uninitialize();
		}

		[TestMethod]
		public async Task IfInitializeDatabaseIsNotDisabledAndADatabaseLogFileDoesNotExist_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			Assert.Fail("Maybe we should remove this test.");

			//DatabaseHelper.CreateEPiServerDatabase();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			//Thread.Sleep(500);

			//DatabaseHelper.DropEPiServerDatabaseLogFile();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			//Assert.IsFalse(DatabaseHelper.EPiServerDatabaseLogFileExists());

			//var initializationEngine = Global.CreateInitializationEngine();

			//initializationEngine.Initialize();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			//initializationEngine.Uninitialize();
		}

		[TestMethod]
		public async Task IfInitializeDatabaseIsNotDisabledAndDatabaseFilesDoesNotExist_ShouldRecreateADatabase()
		{
			await Task.CompletedTask;

			Assert.Fail("Maybe we should remove this test.");

			//DatabaseHelper.CreateEPiServerDatabase();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			//Thread.Sleep(500);

			//DatabaseHelper.DropEPiServerDatabaseFiles();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			//Assert.IsFalse(DatabaseHelper.EPiServerDatabaseFileExists());
			//Assert.IsFalse(DatabaseHelper.EPiServerDatabaseLogFileExists());

			//var initializationEngine = Global.CreateInitializationEngine();

			//initializationEngine.Initialize();

			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseFileExists());
			//Assert.IsTrue(DatabaseHelper.EPiServerDatabaseLogFileExists());

			//initializationEngine.Uninitialize();
		}

		[TestMethod]
		public async Task IfInitializeDataDirectoryIsDisabled_ShouldNotSetTheDataDirectory()
		{
			await Task.CompletedTask;

			ConfigurationSystem.ApplicationSettings = () => new NameValueCollection
			{
				{ DisableableInitializationConfiguration.KeyPrefix + "*:InitializeDataDirectory", "false" }
			};

			TestInitialization.ServiceConfiguration = serviceConfiguration => serviceConfiguration.AddSingleton(Mock.Of<IDatabaseInitializer>());

			Assert.IsNull(AppDomainHelper.GetDataDirectory());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Configure();

			var dataInitializationModule = initializationEngine.Modules.First(module => module is DataInitialization);

			dataInitializationModule.Initialize(initializationEngine);

			Assert.IsNull(AppDomainHelper.GetDataDirectory());

			dataInitializationModule.Uninitialize(initializationEngine);
		}

		[TestMethod]
		public async Task IfInitializeDataDirectoryIsNotDisabled_ShouldSetTheDataDirectory()
		{
			await Task.CompletedTask;

			Assert.IsNull(AppDomainHelper.GetDataDirectory());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.AreEqual(Path.Combine(Global.ProjectDirectoryPath, "App_Data"), AppDomainHelper.GetDataDirectory() as string);

			initializationEngine.Uninitialize();
		}

		[TestCleanup]
		public async Task TestCleanup()
		{
			await Global.CleanupAsync();
		}

		#endregion
	}
}