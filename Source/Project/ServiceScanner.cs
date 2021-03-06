﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.DependencyInjection.Extensions;
using RegionOrebroLan.EPiServer.Initialization.Configuration;
using ServiceDescriptor = EPiServer.ServiceLocation.ServiceDescriptor;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	[CLSCompliant(false)]
	public class ServiceScanner : DisableableInitialization, IConfigurableModule
	{
		#region Constructors

		public ServiceScanner() : this(new DisableableInitializationConfiguration(), AppDomain.CurrentDomain.GetAssemblies(), new ServiceConfigurationScanner()) { }

		public ServiceScanner(IDisableableInitializationConfiguration configuration, IEnumerable<Assembly> assemblies, IServiceConfigurationScanner serviceConfigurationScanner) : base(configuration)
		{
			this.Assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
			this.ServiceConfigurationScanner = serviceConfigurationScanner ?? throw new ArgumentNullException(nameof(serviceConfigurationScanner));
		}

		#endregion

		#region Properties

		protected internal virtual IEnumerable<Assembly> Assemblies { get; }
		protected internal virtual IServiceConfigurationScanner ServiceConfigurationScanner { get; }

		#endregion

		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(this.Disabled)
				return;

			context.Services.AddSingleton(this.ServiceConfigurationScanner);

			foreach(var mapping in this.ServiceConfigurationScanner.Scan(this.Assemblies.Where(this.IncludeAssembly)))
			{
				context.Services.Add(new ServiceDescriptor(mapping.Configuration.ServiceType, mapping.Type, this.GetServiceInstanceScope(mapping)));
			}
		}

		[SuppressMessage("Microsoft.Style", "IDE0010:Add missing cases")]
		protected internal virtual ServiceInstanceScope GetServiceInstanceScope(IServiceConfigurationMapping serviceConfigurationMapping)
		{
			if(serviceConfigurationMapping == null)
				throw new ArgumentNullException(nameof(serviceConfigurationMapping));

			// ReSharper disable SwitchStatementMissingSomeCases
			switch(serviceConfigurationMapping.Configuration.Lifetime)
			{
				case ServiceLifetime.Scoped:
					return ServiceInstanceScope.Hybrid;
				case ServiceLifetime.Singleton:
					return ServiceInstanceScope.Singleton;
				default:
					return ServiceInstanceScope.Transient;
			}
			// ReSharper restore SwitchStatementMissingSomeCases
		}

		protected internal virtual bool IncludeAssembly(Assembly assembly)
		{
			const string name = "RegionOrebroLan";
			var assemblyName = assembly?.GetName().Name;

			// ReSharper disable InvertIf
			if(assemblyName != null)
			{
				if(assemblyName.Equals(name, StringComparison.OrdinalIgnoreCase))
					return true;

				if(assemblyName.StartsWith(name + ".", StringComparison.OrdinalIgnoreCase))
					return true;
			}
			// ReSharper restore InvertIf

			return false;
		}

		public virtual void Initialize(InitializationEngine context) { }
		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}