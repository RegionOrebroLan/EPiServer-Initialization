using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	[TestClass]
	public class ContentProviderConfigurationInitializationTest : DisableableInitializationTest<ContentProviderConfigurationInitialization>
	{
		#region Methods

		[TestMethod]
		public void Disabled_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new ContentProviderConfigurationInitialization().Disabled);
		}

		[TestMethod]
		public void Disabled_Test()
		{
			foreach(var _ in this.GetApplicationSettingsToTest())
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