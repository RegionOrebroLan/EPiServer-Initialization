using System.Diagnostics.CodeAnalysis;
using RegionOrebroLan.EPiServer.Framework.Initialization;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
	public interface IDatabaseInitializer : IInitializer { }
}