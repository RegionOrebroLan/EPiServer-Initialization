using System;
using System.IO;
using RegionOrebroLan.EPiServer.Data;

namespace IntegrationTests.Helpers
{
	public static class AppDomainHelper
	{
		#region Methods

		public static object GetDataDirectory()
		{
			return AppDomain.CurrentDomain.GetData(DataDirectory.Key);
		}

		public static void ResetDataDirectory()
		{
			SetDataDirectory(null);
		}

		public static void SetDataDirectory(object data)
		{
			AppDomain.CurrentDomain.SetData(DataDirectory.Key, data);
		}

		public static void SetDefaultDataDirectory()
		{
			SetDataDirectory(Path.Combine(Global.ProjectDirectoryPath, "App_Data"));
		}

		#endregion
	}
}