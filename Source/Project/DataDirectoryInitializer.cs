using System;
using System.IO.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Internal;
using EPiServer.Logging.Compatibility;

namespace RegionOrebroLan.EPiServer.Initialization
{
	public class DataDirectoryInitializer : IDataDirectoryInitializer
	{
		#region Fields

		private static readonly ILog _logger = LogManager.GetLogger(typeof(DataDirectoryInitializer));

		#endregion

		#region Constructors

		public DataDirectoryInitializer(IApplicationDomain applicationDomain, EnvironmentOptions environmentOptions, IFileSystem fileSystem, IPhysicalPathResolver physicalPathResolver) : this(applicationDomain, environmentOptions, fileSystem, _logger, physicalPathResolver) { }

		protected internal DataDirectoryInitializer(IApplicationDomain applicationDomain, EnvironmentOptions environmentOptions, IFileSystem fileSystem, ILog logger, IPhysicalPathResolver physicalPathResolver)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.EnvironmentOptions = environmentOptions ?? throw new ArgumentNullException(nameof(environmentOptions));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.PhysicalPathResolver = physicalPathResolver ?? throw new ArgumentNullException(nameof(physicalPathResolver));
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual EnvironmentOptions EnvironmentOptions { get; }
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual ILog Logger { get; }
		protected internal virtual IPhysicalPathResolver PhysicalPathResolver { get; }

		#endregion

		#region Methods

		public virtual void Initialize()
		{
			var rebasedPath = this.PhysicalPathResolver.Rebase(this.EnvironmentOptions.BasePath);
			var fullPath = this.FileSystem.Path.GetFullPath(rebasedPath);

			this.ApplicationDomain.SetData("DataDirectory", fullPath);
		}

		#endregion
	}
}