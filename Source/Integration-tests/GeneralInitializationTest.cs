using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Models.Pages;

namespace RegionOrebroLan.EPiServer.Initialization.IntegrationTests
{
	[TestClass]
	public class GeneralInitializationTest
	{
		#region Methods

		protected internal virtual void ContentShouldBeCorrect()
		{
			var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
			var culture = CultureInfo.GetCultureInfo("sv");

			this.ContentShouldBeCorrect(culture, "Start", contentLoader.Get<StartPage>(new ContentReference(5)), new ContentReference(1), "start");
			this.ContentShouldBeCorrect(culture, "First provider", contentLoader.Get<InformationPage>(new ContentReference(6)), new ContentReference(5), "first-provider");
			this.ContentShouldBeCorrect(culture, "Second provider", contentLoader.Get<InformationPage>(new ContentReference(7)), new ContentReference(5), "second-provider");

			this.ContentShouldBeCorrect(culture, "Provider content", contentLoader.Get<InformationPage>(new ContentReference(1, 0, "First-provider")), new ContentReference(6), "provider-content");
			this.ContentShouldBeCorrect(culture, "Provider content", contentLoader.Get<InformationPage>(new ContentReference(1, 0, "Second-provider")), new ContentReference(7), "provider-content");
		}

		[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
		protected internal virtual void ContentShouldBeCorrect(CultureInfo culture, string name, PageData page, ContentReference parent, string segment)
		{
			Assert.AreEqual(1, page.ExistingLanguages.Count());
			Assert.AreEqual(culture, page.ExistingLanguages.First());
			Assert.AreEqual(culture, page.Language);
			Assert.AreEqual(culture, page.MasterLanguage);
			Assert.AreEqual(name, page.Name);
			Assert.AreEqual(parent.ToPageReference(), page.ParentLink);
			Assert.AreEqual(segment, page.URLSegment);
		}

		protected internal virtual void CulturesShouldBeCorrect()
		{
			var languageBranchRepository = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>();

			var cultures = languageBranchRepository.ListAll();
			Assert.AreEqual(3, cultures.Count);
			Assert.AreEqual(CultureInfo.GetCultureInfo("sv"), cultures[0].Culture);
			Assert.AreEqual(CultureInfo.InvariantCulture, cultures[1].Culture);
			Assert.AreEqual(CultureInfo.GetCultureInfo("en"), cultures[2].Culture);

			cultures = languageBranchRepository.ListEnabled();
			Assert.AreEqual(1, cultures.Count);
			Assert.AreEqual(CultureInfo.GetCultureInfo("sv"), cultures.First().Culture);
		}

		[TestMethod]
		public void Initialization_ShouldWorkProperly()
		{
			InitializationModule.FrameworkInitialization(HostType.TestFramework);

			this.ContentShouldBeCorrect();
			this.CulturesShouldBeCorrect();
		}

		#endregion
	}
}