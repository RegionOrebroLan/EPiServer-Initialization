using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	[TestClass]
	public class ContentProviderInitializationTest
	{
		#region Methods

		[TestMethod]
		public async Task Disabled_ShouldReturnFalseByDefault()
		{
			await Task.CompletedTask;

			Assert.IsFalse(new ContentProviderInitialization().Disabled);
		}

		[TestMethod]
		public async Task Disabled_Test()
		{
			await Task.CompletedTask;
		}

		#endregion
	}
}