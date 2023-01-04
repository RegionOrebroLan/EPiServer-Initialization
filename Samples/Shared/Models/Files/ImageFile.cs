using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Shared.Models.Files
{
	[ContentType(GUID = "7ae6e0cc-1213-41e4-9797-44cda4b3a8cb")]
	[MediaDescriptor(ExtensionString = "gif,jpg,png")]
	public class ImageFile : ImageData
	{
		#region Properties

		[CultureSpecific]
		public virtual string AlternativeText { get; set; }

		[CultureSpecific]
		public virtual string Description { get; set; }

		#endregion
	}
}