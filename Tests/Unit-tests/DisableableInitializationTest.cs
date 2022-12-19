using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RegionOrebroLan.EPiServer.Initialization;

namespace UnitTests
{
	// ReSharper disable PossibleNullReferenceException
	public abstract class DisableableInitializationTest<T> where T : DisableableInitialization
	{
		#region Fields

		private static readonly string _assemblyName = typeof(T).Assembly.GetName().Name;
		private static readonly string _applicationSettingsKey = _assemblyName + ":" + typeof(T).FullName.Substring(_assemblyName.Length + 1);

		#endregion

		#region Methods

		protected internal virtual IDictionary<NameValueCollection, bool> GetApplicationSettingsToTest()
		{
			if(_applicationSettingsKey == null)
				throw new InvalidOperationException();

			return new Dictionary<NameValueCollection, bool>();
			//{
			//	{ new NameValueCollection {},  }
			//	{Guid.NewGuid().ToString(), false}
			//};
		}

		#endregion
	}
	// ReSharper restore PossibleNullReferenceException
}