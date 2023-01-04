using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	[TestClass]
	public class ContentProviderConfigurationInitializationTest : DisableableInitializationTest<ContentProviderConfigurationInitialization>
	{
		#region Methods

		[TestMethod]
		public async Task Disabled_ShouldReturnFalseByDefault()
		{
			await Task.CompletedTask;

			Assert.IsFalse(new ContentProviderConfigurationInitialization().Disabled);
		}

		[TestMethod]
		public async Task Disabled_Test()
		{
			foreach(var _ in await this.GetApplicationSettingsToTestAsync())
			{
				//var applicationSettings = new NameValueCollection
				//{
				//	{item.Key, item.Value.ToString(CultureInfo.InvariantCulture)}
				//};

				//Assert.AreEqual(item.Value, new ContentProviderConfigurationInitialization(applicationSettings).Enabled);
			}
		}

		#endregion
	}
}