using System;
using MyCompany.Models.Pages;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	public class PageViewModel<T> : IPageViewModel<T> where T : SitePage
	{
		#region Fields

		private ILayout _layout;

		#endregion

		#region Constructors

		public PageViewModel(T page)
		{
			this.Page = page ?? throw new ArgumentNullException(nameof(page));
		}

		#endregion

		#region Properties

		public virtual ILayout Layout => this._layout ?? (this._layout = new Layout(this.Page));
		public virtual T Page { get; }

		#endregion
	}
}