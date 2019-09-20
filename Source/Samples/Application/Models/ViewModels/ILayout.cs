using System.Globalization;

namespace MyCompany.MyWebApplication.Models.ViewModels
{
	public interface ILayout
	{
		#region Properties

		CultureInfo Culture { get; }
		string Title { get; }

		#endregion
	}
}