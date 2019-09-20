using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MyCompany.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	[SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
	public class Layout : ILayout
	{
		#region Constructors

		public Layout(SitePage page)
		{
			this.Page = page ?? throw new ArgumentNullException(nameof(page));
		}

		#endregion

		#region Properties

		public virtual CultureInfo Culture => this.Page.Language;
		protected internal virtual SitePage Page { get; }
		public virtual string Title => this.Page.Name + " • My company";

		#endregion
	}
}