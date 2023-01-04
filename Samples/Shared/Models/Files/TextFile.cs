using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Shared.Models.Files
{
	[ContentType(GUID = "4bcb4d00-0adf-4f90-805d-88fb3f849819")]
	[MediaDescriptor(ExtensionString = "txt")]
	public class TextFile : MediaData
	{
		#region Properties

		[CultureSpecific]
		public virtual string Description { get; set; }

		#endregion
	}
}