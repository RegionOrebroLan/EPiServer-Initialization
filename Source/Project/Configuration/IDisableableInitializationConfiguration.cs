namespace RegionOrebroLan.EPiServer.Initialization.Configuration
{
	public interface IDisableableInitializationConfiguration
	{
		#region Methods

		bool? IsDisabled(string initializationKey);

		#endregion
	}
}