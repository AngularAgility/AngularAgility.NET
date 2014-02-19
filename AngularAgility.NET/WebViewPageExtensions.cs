using System.Web.Mvc;

namespace Angular.Agility
{
	public static class WebViewPageExtensions
	{
		public static AgilityHelper<TModel> AgilityFor<TModel>(this WebViewPage<TModel> webPage)
		{
			return new AgilityHelper<TModel>(webPage);
		}
	}
}