using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Logging;
using EPiServer.Web;
using RegionOrebroLan.Extensions;

namespace RegionOrebroLan.EPiServer.Initialization.Internal
{
	public class DefaultSiteContentInitializer : IDefaultSiteContentInitializer
	{
		#region Fields

		private static readonly Uri _url = new Uri("http://localhost/");

		#endregion

		#region Constructors

		public DefaultSiteContentInitializer(IApplicationDomain applicationDomain, IContentLoader contentLoader, IDataImporter dataImporter, IFileSystem fileSystem, ILanguageBranchRepository languageBranchRepository, ILoggerFactory loggerFactory, ISiteDefinitionRepository siteDefinitionRepository)
		{
			this.ApplicationDomain = applicationDomain ?? throw new ArgumentNullException(nameof(applicationDomain));
			this.ContentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
			this.DataImporter = dataImporter ?? throw new ArgumentNullException(nameof(dataImporter));
			this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			this.LanguageBranchRepository = languageBranchRepository ?? throw new ArgumentNullException(nameof(languageBranchRepository));

			if(loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			this.Logger = loggerFactory.Create(this.GetType().FullName);
			this.SiteDefinitionRepository = siteDefinitionRepository ?? throw new ArgumentNullException(nameof(siteDefinitionRepository));
		}

		#endregion

		#region Properties

		protected internal virtual IApplicationDomain ApplicationDomain { get; }
		protected internal virtual IContentLoader ContentLoader { get; }
		protected internal virtual IDataImporter DataImporter { get; }
		protected internal virtual IFileSystem FileSystem { get; }
		protected internal virtual ILanguageBranchRepository LanguageBranchRepository { get; }
		protected internal virtual ILogger Logger { get; }
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

		[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
		public virtual void Initialize()
		{
			try
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