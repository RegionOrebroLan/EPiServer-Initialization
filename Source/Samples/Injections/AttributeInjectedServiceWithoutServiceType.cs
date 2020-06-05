using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.DependencyInjection;

namespace RegionOrebroLan.InjectedDependencies
{
	/// <summary>
	/// This is just for testing that the RegionOrebroLan.EPiServer.Initialization.ServiceScanner can handle the RegionOrebroLan.ServiceLocation.ServiceConfigurationAttribute without a ServiceType.
	/// </summary>
	[ServiceConfiguration(Lifetime = ServiceLifetime.Singleton)]
	public class AttributeInjectedServiceWithoutServiceType
	{
		#region Properties

		public virtual string Value => "Test";

		#endregion
	}
}