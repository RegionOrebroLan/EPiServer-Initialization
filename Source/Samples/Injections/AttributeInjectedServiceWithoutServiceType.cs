using RegionOrebroLan.ServiceLocation;

namespace RegionOrebroLan.InjectedDependencies
{
	/// <summary>
	/// This is just for testing that the RegionOrebroLan.EPiServer.Initialization.ServiceScanner can handle the RegionOrebroLan.ServiceLocation.ServiceConfigurationAttribute without a ServiceType.
	/// </summary>
	[ServiceConfiguration(InstanceMode = InstanceMode.Singleton)]
	public class AttributeInjectedServiceWithoutServiceType
	{
		#region Properties

		public virtual string Value => "Test";

		#endregion
	}
}