using System;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using IntegrationTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.Models.Pages;

namespace IntegrationTests
{
	[TestClass]
	public class GeneralInitializationTest
	{
		#region Methods

		[TestCleanup]
		public void Cleanup()
		{
			Global.CleanupEachTest();
		}

		protected internal virtual void Content_Test(IServiceLocator serviceLocator)
		{
			if(serviceLocator == null)
				throw new ArgumentNullException(nameof(serviceLocator));

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

		protected internal virtual void Culture_Test(IServiceLocator serviceLocator)
		{
			if(serviceLocator == null)
				throw new ArgumentNullException(nameof(serviceLocator));

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
		public void Data_Test()
		{
			Assert.IsFalse(DatabaseHelper.EPiServerDatabaseExists());

			var initializationEngine = Global.CreateInitializationEngine();

			initializationEngine.Initialize();

			Assert.IsTrue(DatabaseHelper.EPiServerDatabaseExists());

			this.Culture_Test(initializationEngine.Locate.Advanced);

			this.Content_Test(initializationEngine.Locate.Advanced);

			initializationEngine.Uninitialize();
		}

		#endregion

		//		protected internal virtual void ContentShouldBeCorrect()
		//		{
		//			var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
		//			var culture = CultureInfo.GetCultureInfo("sv");

		//			this.ContentShouldBeCorrect(culture, "Start", contentLoader.Get<StartPage>(new ContentReference(5)), new ContentReference(1), "start");
		//			this.ContentShouldBeCorrect(culture, "First provider", contentLoader.Get<InformationPage>(new ContentReference(6)), new ContentReference(5), "first-provider");
		//			this.ContentShouldBeCorrect(culture, "Second provider", contentLoader.Get<InformationPage>(new ContentReference(7)), new ContentReference(5), "second-provider");

		//			this.ContentShouldBeCorrect(culture, "Provider content", contentLoader.Get<InformationPage>(new ContentReference(1, 0, "First-provider")), new ContentReference(6), "provider-content");
		//			this.ContentShouldBeCorrect(culture, "Provider content", contentLoader.Get<InformationPage>(new ContentReference(1, 0, "Second-provider")), new ContentReference(7), "provider-content");
		//		}

		//		[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
		//		protected internal virtual void ContentShouldBeCorrect(CultureInfo culture, string name, PageData page, ContentReference parent, string segment)
		//		{
		//			Assert.AreEqual(1, page.ExistingLanguages.Count());
		//			Assert.AreEqual(culture, page.ExistingLanguages.First());
		//			Assert.AreEqual(culture, page.Language);
		//			Assert.AreEqual(culture, page.MasterLanguage);
		//			Assert.AreEqual(name, page.Name);
		//			Assert.AreEqual(parent.ToPageReference(), page.ParentLink);
		//			Assert.AreEqual(segment, page.URLSegment);
		//		}
	}
}