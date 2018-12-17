using System;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Logging.Compatibility;
using EPiServer.Web;
using RegionOrebroLan.Extensions;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DefaultSiteContentInitializer : IDefaultSiteContentInitializer
	{
		#region Fields

		private static readonly ILog _logger = LogManager.GetLogger(typeof(DefaultSiteContentInitializer));
		private static readonly Uri _url = new Uri("http://localhost/");

		#endregion

		#region Constructors

		public DefaultSiteContentInitializer(IApplicationDomain applicationDomain, IContentLoader contentLoader, IDataImporter dataImporter, IFileSystem fileSystem, ILanguageBranchRepository languageBranchRepository, ISiteDefinitionRepository siteDefinitionRepository) : this(applicationDomain, contentLoader, dataImporter, fileSystem, languageBranchRepository, _logger, siteDefinitionRepository) { }

		protected internal DefaultSiteContentInitializer(IApplicationDomain applicationDomain, IContentLoader contentLoader, IDataImporter dataImporter, IFileSystem fileSystem, ILanguageBranchRepository languageBranchRepository, ILog logger, ISiteDefinitionRepository siteDefinitionRepository)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.DataImporter = dataImporter ?? throw new ArgumentNullException(nameof(dataImporter));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			this.LanguageBranchRepository = languageBranchRepository ?? throw new ArgumentNullException(nameof(languageBranchRepository));
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.SiteDefinitionRepository = siteDefinitionRepository ?? throw new ArgumentNullException(nameof(siteDefinitionRepository));
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual IDataImporter DataImporter { get; }
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual ILanguageBranchRepository LanguageBranchRepository { get; }
		protected internal virtual ILog Logger { get; }
		protected internal virtual ISiteDefinitionRepository SiteDefinitionRepository { get; }
		protected internal virtual Uri Url => _url;

		#endregion

		#region Methods

		protected internal virtual SiteDefinition CreateSiteDefinition(ContentReference startPageLink)
		{
			var siteUrl = this.Url;

			var siteDefinition = new SiteDefinition
			{
				Name = "Default",
				SiteUrl = siteUrl,
				StartPage = startPageLink
			};

			siteDefinition.Hosts.Add(new HostDefinition {Name = "*"});
			siteDefinition.Hosts.Add(new HostDefinition {Name = siteUrl.Authority, Type = HostDefinitionType.Primary});

			this.SiteDefinitionRepository.Save(siteDefinition);

			return siteDefinition;
		}

		protected internal virtual ContentReference Import(string packagePath)
		{
			using(var stream = this.FileSystem.File.OpenRead(packagePath))
			{
				var importOptions = new ImportOptions
				{
					EnsureContentNameUniqueness = false
				};

				var transferLog = this.DataImporter.Import(stream, ContentReference.RootPage, importOptions);

				if(transferLog.Warnings.Any() && this.Logger.IsWarnEnabled)
				{
					foreach(var warning in transferLog.Warnings)
					{
						this.Logger.Warn(warning);
					}
				}

				// ReSharper disable InvertIf
				if(transferLog.Errors.Any())
				{
					if(this.Logger.IsErrorEnabled)
					{
						foreach(var error in transferLog.Errors)
						{
							this.Logger.Error(error);
						}
					}

					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Tried to import default content \"{0}\" but failed: {1}", packagePath, string.Join(", ", transferLog.Errors)));
				}
				// ReSharper restore InvertIf

				return this.DataImporter.Status.ImportedRoot;
			}
		}

		public virtual void Initialize()
		{
			if(this.SiteDefinitionRepository.List().Any())
				return;

			if(this.ContentLoader.GetChildren<PageData>(ContentReference.RootPage).Any(content => content.ContentLink != ContentReference.WasteBasket))
				return;

			var contentPackagePath = this.FileSystem.Path.Combine(this.ApplicationDomain.GetDataDirectoryPath(), "DefaultSiteContent.episerverdata");

			if(!this.FileSystem.File.Exists(contentPackagePath))
				return;

			var importedRootLink = this.Import(contentPackagePath);

			SiteDefinition.Current = this.CreateSiteDefinition(importedRootLink);
		}

		#endregion
	}
}