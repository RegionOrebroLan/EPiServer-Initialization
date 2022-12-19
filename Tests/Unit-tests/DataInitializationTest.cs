using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	[TestClass]
	public class DataInitializationTest
	{
		#region Methods

		[TestMethod]
		public void Disabled_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new DataInitialization().Disabled);
		}

		[TestMethod]
		public void Disabled_Test() { }

		#endregion
	}
}