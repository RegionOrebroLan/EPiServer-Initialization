using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyCompany.Models.Pages
{
	public abstract class SitePage : PageData
	{
		#region Properties

		[CultureSpecific]
		[Display(GroupName = SystemTabNames.Content, Order = 10)]
		public virtual XhtmlString MainBody { get; set; }

		#endregion
	}
}