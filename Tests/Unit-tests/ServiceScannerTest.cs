using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	[TestClass]
	public class ServiceScannerTest
	{
		#region Methods

		[TestMethod]
		public void Disabled_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new ServiceScanner().Disabled);
		}

		[TestMethod]
		public void Disabled_Test() { }

		#endregion
	}
}