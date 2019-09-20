using System;
using System.Web.Mvc;
using EPiServer;
using MyCompany.Models.Pages;
using MyCompany.MyWebApplication.Models.ViewModels;

namespace MyCompany.MyWebApplication.Controllers
{
	public class DefaultPageController : SitePageController<SitePage>
	{
		#region Methods

		protected internal virtual IPageViewModel<SitePage> CreateViewModel()
		{
			var type = typeof(PageViewModel<>).MakeGenericType(this.PageContext.Page.GetOriginalType());

			return Activator.CreateInstance(type, this.PageContext.Page) as IPageViewModel<SitePage>;
		}

		public virtual ActionResult Index()
		{
			return this.View(this.DefaultViewPath, this.CreateViewModel());
		}

		#endregion
	}
}