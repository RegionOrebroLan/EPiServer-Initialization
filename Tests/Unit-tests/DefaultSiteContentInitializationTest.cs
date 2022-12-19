using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	[TestClass]
	public class DefaultSiteContentInitializationTest
	{
		#region Methods

		[TestMethod]
		public void Disabled_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new DefaultSiteContentInitialization().Disabled);
		}

		[TestMethod]
		public void Disabled_Test() { }

		#endregion
	}
}