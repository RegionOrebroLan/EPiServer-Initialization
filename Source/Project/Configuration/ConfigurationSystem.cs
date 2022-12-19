using System;
using System.Collections.Specialized;
using System.Configuration;

namespace RegionOrebroLan.EPiServer.Initialization.Configuration
{
	public static class ConfigurationSystem
	{
		#region Fields

		private static Func<NameValueCollection> _applicationSettings;

		#endregion

		#region Properties

		public static Func<NameValueCollection> ApplicationSettings
		{
			get => _applicationSettings ??= () => ConfigurationManager.AppSettings;
			set => _applicationSettings = value;
		}

		#endregion

		#region Methods

		public static void Reset()
		{
			_applicationSettings = null;
		}

		#endregion
	}
}