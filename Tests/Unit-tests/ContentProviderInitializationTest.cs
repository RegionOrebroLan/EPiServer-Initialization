using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
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