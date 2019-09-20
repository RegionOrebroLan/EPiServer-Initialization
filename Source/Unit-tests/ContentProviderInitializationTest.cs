using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegionOrebroLan.EPiServer.Initialization.UnitTests
{
	[TestClass]
	public class ContentProviderInitializationTest
	{
		#region Methods

		[TestMethod]
		public void Disabled_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new ContentProviderInitialization().Disabled);
		}

		[TestMethod]
		public void Disabled_Test() { }

		#endregion
	}
}