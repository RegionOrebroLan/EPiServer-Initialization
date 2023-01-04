using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
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

		protected internal virtual async Task<IDictionary<NameValueCollection, bool>> GetApplicationSettingsToTestAsync()
		{
			if(_applicationSettingsKey == null)
				throw new InvalidOperationException();

			return await Task.FromResult(new Dictionary<NameValueCollection, bool>());
		}

		#endregion
	}
	// ReSharper restore PossibleNullReferenceException
}