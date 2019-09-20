using System;
using System.Globalization;
using EPiServer;
using EPiServer.Web.Mvc;
using MyCompany.Models.Pages;

namespace MyCompany.MyWebApplication.Controllers
{
	public abstract class SitePageController<T> : PageController<T> where T : SitePage
	{
		#region Fields

		private const string _defaultViewName = "Index";
		private Lazy<string> _defaultViewPath;
		private const string _viewPathFormat = "~/Views/{0}/{1}.cshtml";

		#endregion

		#region Properties

		protected internal virtual string DefaultViewName => _defaultViewName;

		protected internal virtual string DefaultViewPath
		{
			get
			{
				if(this._defaultViewPath == null)
					this._defaultViewPath = new Lazy<string>(() => this.GetViewPath(this.DefaultViewName));

				return this._defaultViewPath.Value;
			}
		}

		protected internal virtual string ViewPathFormat => _viewPathFormat;

		#endregion

		#region Methods

		/// <summary>
		/// Gets the view-path for the view-name.
		/// </summary>
		/// <param name="viewName">The view-name without the file-extension, eg "Index".</param>
		protected internal virtual string GetViewPath(string viewName)
		{
			return string.Format(CultureInfo.InvariantCulture, this.ViewPathFormat, this.PageContext.Page.GetOriginalType().Name, viewName);
		}

		#endregion
	}
}