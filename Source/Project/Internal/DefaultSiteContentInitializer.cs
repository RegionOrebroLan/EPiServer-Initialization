using System;
using System.IO;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Framework;
using EPiServer.Framework.Internal;
using EPiServer.Logging;
using EPiServer.Web;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DefaultSiteContentInitializer : IDefaultSiteContentInitializer
	{
		#region Fields

		private static readonly Uri _url = new("http://localhost/");

		#endregion

		#region Constructors

		public DefaultSiteContentInitializer(IContentLoader contentLoader, IDataImporter dataImporter, EnvironmentOptions environmentOptions, ILanguageBranchRepository languageBranchRepository, ILoggerFactory loggerFactory, IPhysicalPathResolver physicalPathResolver, ISiteDefinitionRepository siteDefinitionRepository)
		{
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.DataImporter = dataImporter ?? throw new ArgumentNullException(nameof(dataImporter));
			this.EnvironmentOptions = environmentOptions ?? throw new ArgumentNullException(nameof(environmentOptions));
			this.LanguageBranchRepository = languageBranchRepository ?? throw new ArgumentNullException(nameof(languageBranchRepository));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).Create(this.GetType().FullName);
			this.PhysicalPathResolver = physicalPathResolver ?? throw new ArgumentNullException(nameof(physicalPathResolver));
			this.SiteDefinitionRepository = siteDefinitionRepository ?? throw new ArgumentNullException(nameof(siteDefinitionRepository));
		}

		#endregion

		#region Properties

		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual IDataImporter DataImporter { get; }
		protected internal virtual EnvironmentOptions EnvironmentOptions { get; }
		protected internal virtual ILanguageBranchRepository LanguageBranchRepository { get; }
		protected internal virtual ILogger Logger { get; }
		protected internal virtual IPhysicalPathResolver PhysicalPathResolver { get; }
		protected internal virtual ISiteDefinitionRepository SiteDefinitionRepository { get; }
		protected internal virtual Uri Url => _url;

		#endregion

		#region Methods

		protected internal virtual SiteDefinition CreateSiteDefinition(ContentReference siteAssetsRootLink, ContentReference startPageLink)
		{
			var siteUrl = this.Url;

			var siteDefinition = new SiteDefinition
			{
				Name = "Default",
				SiteAssetsRoot = siteAssetsRootLink,
				SiteUrl = siteUrl,
				StartPage = startPageLink
			};

			siteDefinition.Hosts.Add(new HostDefinition { Name = "*" });
			siteDefinition.Hosts.Add(new HostDefinition { Name = siteUrl.Authority, Type = HostDefinitionType.Primary });

			this.SiteDefinitionRepository.Save(siteDefinition);

			return siteDefinition;
		}

		protected internal virtual ContentReference GetSiteAssetsRootLink(ContentReference startPageLink)
		{
			return this.ContentLoader
				.GetChildren<ContentFolder>(startPageLink, new LoaderOptions { LanguageLoaderOption.MasterLanguage() })
				.FirstOrDefault(contentFolder => string.Equals("SysSiteAssets", contentFolder.RouteSegment, StringComparison.OrdinalIgnoreCase))?.ContentLink;
		}

		protected internal virtual ContentReference Import(string packagePath)
		{
			using(var stream = File.OpenRead(packagePath))
			{
				var importOptions = new ImportOptions
				{
					EnsureContentNameUniqueness = false
				};

				var transferLog = this.DataImporter.Import(stream, ContentReference.RootPage, importOptions);

				if(transferLog.Warnings.Any() && this.Logger.IsWarningEnabled())
				{
					foreach(var warning in transferLog.Warnings)
					{
						this.Logger.Warning(warning);
					}
				}

				// ReSharper disable InvertIf
				if(transferLog.Errors.Any())
				{
					if(this.Logger.IsErrorEnabled())
					{
						foreach(var error in transferLog.Errors)
						{
							this.Logger.Error(error);
						}
					}

					throw new InvalidOperationException($"Tried to import default content \"{packagePath}\" but failed: {string.Join(", ", transferLog.Errors)}");
				}
				// ReSharper restore InvertIf

				return this.DataImporter.Status.ImportedRoot;
			}
		}

		public virtual void Initialize()
		{
			try
			{
				if(this.SiteDefinitionRepository.List().Any())
					return;

				if(this.ContentLoader.GetChildren<PageData>(ContentReference.RootPage).Any(content => content.ContentLink != ContentReference.WasteBasket))
					return;

				var contentPackagePath = Path.Combine(this.PhysicalPathResolver.Rebase(this.EnvironmentOptions.BasePath), "DefaultSiteContent.episerverdata");

				if(!File.Exists(contentPackagePath))
					return;

				var importedRootLink = this.Import(contentPackagePath);

				SiteDefinition.Current = this.CreateSiteDefinition(this.GetSiteAssetsRootLink(importedRootLink), importedRootLink);
			}
			catch(Exception exception)
			{
				const string message = "Could not import default-site-content.";

				if(this.Logger.IsErrorEnabled())
					this.Logger.Error(message, exception);

				throw new InvalidOperationException(message, exception);
			}
		}

		#endregion
	}
}