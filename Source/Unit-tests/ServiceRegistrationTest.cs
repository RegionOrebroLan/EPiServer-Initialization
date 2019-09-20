using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegionOrebroLan.EPiServer.Initialization.UnitTests
{
	[TestClass]
	public class ServiceRegistrationTest
	{
		#region Methods

		[TestMethod]
		public void Disabled_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new ServiceRegistration().Disabled);
		}

		[TestMethod]
		public void Disabled_Test() { }

		#endregion
	}
}