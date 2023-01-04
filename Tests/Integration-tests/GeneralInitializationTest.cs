using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using IntegrationTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models.Pages;

namespace IntegrationTests
{
	[TestClass]
	public class GeneralInitializationTest
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

		protected internal virtual async Task Content_Test(IServiceLocator serviceLocator)
		{
			if(serviceLocator == null)
				throw new ArgumentNullException(nameof(serviceLocator));

			await Task.CompletedTask;

			var contentLoader = serviceLocator.GetInstance<IContentLoader>();

			Assert.AreEqual(25, contentLoader.GetDescendents(ContentReference.RootPage).Count());

			var startPageLink = ContentReference.StartPage;

			var children = contentLoader.GetChildren<SitePage>(startPageLink, CultureInfo.GetCultureInfo("en")).ToArray();
			Assert.AreEqual(6, children.Length, "The number of English top level pages should be 6.");

			children = contentLoader.GetChildren<SitePage>(startPageLink, CultureInfo.GetCultureInfo("fr")).ToArray();
			Assert.AreEqual(5, children.Length, "The number of French top level pages should be 5.");

			children = contentLoader.GetChildren<SitePage>(startPageLink, CultureInfo.GetCultureInfo("sv")).ToArray();
			Assert.AreEqual(3, children.Length, "The number of Swedish top level pages should be 3.");
		}

		protected internal virtual async Task Culture_Test(IServiceLocator serviceLocator)
		{
			if(serviceLocator == null)
				throw new ArgumentNullException(nameof(serviceLocator));

			await Task.CompletedTask;

			var languageBranchRepository = serviceLocator.GetInstance<ILanguageBranchRepository>();

			var allLanguageBranches = languageBranchRepository.ListAll();
			Assert.AreEqual(5, allLanguageBranches.Count);
			Assert.AreEqual(CultureInfo.GetCultureInfo("en"), allLanguageBranches[0].Culture);
			Assert.AreEqual(CultureInfo.InvariantCulture, allLanguageBranches[1].Culture);
			Assert.AreEqual(CultureInfo.GetCultureInfo("de"), allLanguageBranches[2].Culture);
			Assert.AreEqual(CultureInfo.GetCultureInfo("fr"), allLanguageBranches[3].Culture);
			Assert.AreEqual(CultureInfo.GetCultureInfo("sv"), allLanguageBranches[4].Culture);

			var enabledLanguageBranches = languageBranchRepository.ListEnabled();
			Assert.AreEqual(3, enabledLanguageBranches.Count);
			Assert.AreEqual(CultureInfo.GetCultureInfo("en"), enabledLanguageBranches[0].Culture);
			Assert.AreEqual(CultureInfo.GetCultureInfo("fr"), enabledLanguageBranches[1].Culture);
			Assert.AreEqual(CultureInfo.GetCultureInfo("sv"), enabledLanguageBranches[2].Culture);
		}

		[TestMethod]
		public async Task Data_Test()
		{
			Assert.IsFalse(await DatabaseHelper.EPiServerDatabaseExistsAsync());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(await DatabaseHelper.EPiServerDatabaseExistsAsync());

			await this.Culture_Test(initializationEngine.Locate.Advanced);

			await this.Content_Test(initializationEngine.Locate.Advanced);

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