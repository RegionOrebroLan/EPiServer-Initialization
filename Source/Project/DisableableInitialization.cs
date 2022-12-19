using System;
using RegionOrebroLan.EPiServer.Initialization.Configuration;

namespace RegionOrebroLan.EPiServer.Initialization
{
	public abstract class DisableableInitialization
	{
		#region Fields

		private bool? _disabled;
		private string _initializationKey;

		#endregion

		#region Constructors

		protected DisableableInitialization(IDisableableInitializationConfiguration configuration)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		#endregion

		#region Properties

		protected internal virtual IDisableableInitializationConfiguration Configuration { get; }

		protected internal virtual bool Disabled
		{
			get
			{
				this._disabled ??= this.Configuration.IsDisabled(this.InitializationKey) ?? false;

				// ReSharper disable PossibleInvalidOperationException
				return this._disabled.Value;
				// ReSharper restore PossibleInvalidOperationException
			}
		}

		protected internal virtual string InitializationKey => this._initializationKey ??= this.GetType().FullName;

		#endregion

		#region Methods

		protected internal virtual string CreateInitializationKey(string memberName)
		{
			return this.InitializationKey + ":" + memberName;
		}

		#endregion
	}
}