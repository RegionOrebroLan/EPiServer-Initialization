using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegionOrebroLan.EPiServer.Initialization.UnitTests
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
			foreach(var item in this.GetApplicationSettingsToTest())
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