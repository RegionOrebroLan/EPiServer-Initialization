using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using EPiServer.Core;
using EPiServer.Core.Internal;
using EPiServer.Data;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace RegionOrebroLan.EPiServer.Initialization
{
	[InitializableModule]
	public class ContentProviderConfigurationInitialization : IConfigurableModule
	{
		#region Methods

		public virtual void ConfigureContainer(ServiceConfigurationContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			context.Services.AddSingleton<IDictionary<string, ContentProviderBuilder>>(new Dictionary<string, ContentProviderBuilder>(StringComparer.OrdinalIgnoreCase));
		}

		protected internal virtual IDictionary<string, string> CreateContentProviderSettings(ConnectionStringOptions connectionSetting)
		{
			if(connectionSetting == null)
				throw new ArgumentNullException(nameof(connectionSetting));

			var contentProviderSettings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach(var keyValuePair in connectionSetting.ConnectionString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = keyValuePair.Split(new[] {'='}, 2);
				contentProviderSettings.Add(parts[0], parts[1]);
			}

			return contentProviderSettings;
		}

		protected internal virtual IEnumerable<ConnectionStringOptions> GetContentProviderConnectionSettings(DataAccessOptions dataAccessOptions)
		{
			if(dataAccessOptions == null)
				throw new ArgumentNullException(nameof(dataAccessOptions));

			return dataAccessOptions.ConnectionStrings.Where(connectionSetting => string.Equals(typeof(ContentProvider).FullName, connectionSetting.ProviderName, StringComparison.OrdinalIgnoreCase));
		}

		protected internal virtual string GetContentProviderName(IDictionary<string, string> contentProviderSettings)
		{
			if(contentProviderSettings == null)
				throw new ArgumentNullException(nameof(contentProviderSettings));

			try
			{
				const string nameKey = "name";

				var name = contentProviderSettings[nameKey];

				contentProviderSettings.Remove(nameKey);

				return name;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not get the name from content-provider-settings.", exception);
			}
		}

		protected internal virtual Type GetContentProviderType(IDictionary<string, string> contentProviderSettings)
		{
			if(contentProviderSettings == null)
				throw new ArgumentNullException(nameof(contentProviderSettings));

			try
			{
				const string typeKey = "type";

				var type = Type.GetType(contentProviderSettings[typeKey], true);

				contentProviderSettings.Remove(typeKey);

				return type;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not get the type from content-provider-settings.", exception);
			}
		}

		public virtual void Initialize(InitializationEngine context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			var contentOptions = context.Locate.Advanced.GetInstance<ContentOptions>();
			var customContentProviders = context.Locate.Advanced.GetInstance<IDictionary<string, ContentProviderBuilder>>();
			var dataAccessOptions = context.Locate.Advanced.GetInstance<DataAccessOptions>();

			foreach(var key in contentOptions.Providers.Keys.ToArray())
			{
				var contentProviderBuilder = contentOptions.Providers[key];

				if(contentProviderBuilder.ProviderType == typeof(DefaultContentProvider))
					continue;

				customContentProviders.Add(key, contentProviderBuilder);

				contentOptions.Providers.Remove(key);
			}

			foreach(var contentProviderConnectionSetting in this.GetContentProviderConnectionSettings(dataAccessOptions))
			{
				try
				{
					var contentProviderSettings = this.CreateContentProviderSettings(contentProviderConnectionSetting);

					var name = this.GetContentProviderName(contentProviderSettings);
					var type = this.GetContentProviderType(contentProviderSettings);

					var contentProviderBuilder = new ContentProviderBuilder(name, type).Configure<NameValueCollection>(nameValueCollection =>
					{
						foreach(var parameter in contentProviderSettings)
						{
							nameValueCollection.Add(parameter.Key, parameter.Value);
						}
					});

					contentProviderBuilder.AutoCorrectInvalidName = true;

					customContentProviders.Add(name, contentProviderBuilder);
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not create content-provider-builder from connection-string \"{0}\", \"{1}\", \"{2}\".", contentProviderConnectionSetting.Name, contentProviderConnectionSetting.ConnectionString, contentProviderConnectionSetting.ProviderName), exception);
				}
			}
		}

		public virtual void Uninitialize(InitializationEngine context) { }

		#endregion
	}
}