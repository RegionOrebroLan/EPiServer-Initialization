using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EPiServer.Construction;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using Shared.Models.Pages;

namespace Shared.ContentProviders
{
	public class FakedContentProvider : ContentProvider
	{
		#region Fields

		private ContentReference _onlyContentReference;

		#endregion

		#region Constructors

		public FakedContentProvider(IContentFactory contentFactory, IContentTypeRepository contentTypeRepository)
		{
			this.ContentFactory = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
			this.ContentTypeRepository = contentTypeRepository ?? throw new ArgumentNullException(nameof(contentTypeRepository));
		}

		#endregion

		#region Properties

		protected internal new virtual IContentFactory ContentFactory { get; }
		protected internal new virtual IContentTypeRepository ContentTypeRepository { get; }

		protected internal virtual ContentReference OnlyContentReference
		{
			get
			{
				if(this._onlyContentReference == null)
				{
					this._onlyContentReference = new ContentReference
					{
						ID = 1,
						ProviderName = this.Name
					};
				}

				return this._onlyContentReference;
			}
		}

		#endregion

		#region Methods

		protected override IList<GetChildrenReferenceResult> LoadChildrenReferencesAndTypes(ContentReference contentLink, string languageID, out bool languageSpecific)
		{
			var loadChildrenReferencesAndTypes = base.LoadChildrenReferencesAndTypes(contentLink, languageID, out languageSpecific);

			if(this.EntryPoint.CompareToIgnoreWorkID(contentLink))
				loadChildrenReferencesAndTypes.Add(new GetChildrenReferenceResult { ContentLink = this.OnlyContentReference, IsLeafNode = true, ModelType = typeof(InformationPage) });

			return loadChildrenReferencesAndTypes;
		}

		[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
		protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
		{
			if(languageSelector == null)
				throw new ArgumentNullException(nameof(languageSelector));

			if(!this.OnlyContentReference.CompareToIgnoreWorkID(contentLink))
				throw new ContentNotFoundException(contentLink);

			var contentType = this.ContentTypeRepository.Load<InformationPage>();
			var content = (InformationPage)this.ContentFactory.CreateContent(contentType);

			content.SetDefaultValues(contentType);

			content.ContentLink = contentLink;
			content.ExistingLanguages = new[] { languageSelector.Language };
			content.Language = content.MasterLanguage = languageSelector.Language;
			content.LinkType = PageShortcutType.Normal;
			content.Name = "Provider content";
			content.ParentLink = this.EntryPoint.ToPageReference();
			content.Status = VersionStatus.Published;
			content.URLSegment = content.Name.ToLowerInvariant().Replace(" ", "-");

			var contentSecurityDescriptor = content.GetContentSecurityDescriptor();
			contentSecurityDescriptor.IsInherited = true;
			contentSecurityDescriptor.ContentLink = content.ContentLink;

			return content;
		}

		#endregion
	}
}