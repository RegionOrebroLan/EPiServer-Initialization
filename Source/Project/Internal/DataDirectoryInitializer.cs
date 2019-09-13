using System;
using System.IO.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Internal;
using EPiServer.Logging;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DataDirectoryInitializer : IDataDirectoryInitializer
	{
		#region Constructors

		public DataDirectoryInitializer(IApplicationDomain applicationDomain, EnvironmentOptions environmentOptions, IFileSystem fileSystem, ILoggerFactory loggerFactory, IPhysicalPathResolver physicalPathResolver)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.EnvironmentOptions = environmentOptions ?? throw new ArgumentNullException(nameof(environmentOptions));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

			if(loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			this.Logger = loggerFactory.Create(this.GetType().FullName);
			this.PhysicalPathResolver = physicalPathResolver ?? throw new ArgumentNullException(nameof(physicalPathResolver));
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual EnvironmentOptions EnvironmentOptions { get; }
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual ILogger Logger { get; }
		protected internal virtual IPhysicalPathResolver PhysicalPathResolver { get; }

		#endregion

		#region Methods

		public virtual void Initialize()
		{
			try
			{
				var rebasedPath = this.PhysicalPathResolver.Rebase(this.EnvironmentOptions.BasePath);
				var fullPath = this.FileSystem.Path.GetFullPath(rebasedPath);

				this.ApplicationDomain.SetData("DataDirectory", fullPath);
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