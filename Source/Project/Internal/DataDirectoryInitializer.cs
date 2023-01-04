using System;
using System.IO;
using EPiServer.Framework;
using EPiServer.Framework.Internal;
using EPiServer.Logging;
using RegionOrebroLan.EPiServer.Data;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DataDirectoryInitializer : IDataDirectoryInitializer
	{
		#region Constructors

		public DataDirectoryInitializer(EnvironmentOptions environmentOptions, ILoggerFactory loggerFactory, IPhysicalPathResolver physicalPathResolver)
		{
			this.EnvironmentOptions = environmentOptions ?? throw new ArgumentNullException(nameof(environmentOptions));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).Create(this.GetType().FullName);
			this.PhysicalPathResolver = physicalPathResolver ?? throw new ArgumentNullException(nameof(physicalPathResolver));
		}

		#endregion

		#region Properties

		protected internal virtual EnvironmentOptions EnvironmentOptions { get; }
		protected internal virtual ILogger Logger { get; }
		protected internal virtual IPhysicalPathResolver PhysicalPathResolver { get; }

		#endregion

		#region Methods

		public virtual void Initialize()
		{
			try
			{
				var rebasedPath = this.PhysicalPathResolver.Rebase(this.EnvironmentOptions.BasePath);
				var fullPath = Path.GetFullPath(rebasedPath);

				AppDomain.CurrentDomain.SetData(DataDirectory.Key, fullPath);
			}
			catch(Exception exception)
			{
				var message = $"Could not set data-directory for base-path \"{this.EnvironmentOptions.BasePath}\".";

				if(this.Logger.IsErrorEnabled())
					this.Logger.Error(message, exception);

				throw new InvalidOperationException(message, exception);
			}
		}

		#endregion
	}
}