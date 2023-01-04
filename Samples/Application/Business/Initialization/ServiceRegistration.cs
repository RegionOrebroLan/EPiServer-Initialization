using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using MyCompany.MyWebApplication.Business.Data.SqlClient;
using RegionOrebroLan.EPiServer.Data;

namespace MyCompany.MyWebApplication.Business.Initialization
{
	[InitializableModule]
	public class ServiceRegistration : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.TryAdd<IDatabaseCreator, SqlServerLocalDatabaseCreator>(ServiceInstanceScope.Singleton);
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}