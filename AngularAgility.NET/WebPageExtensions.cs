using System.Web.WebPages;

namespace Angular.Agility
{
	public static class WebPageExtensions
	{
		public static AgilityHelper<TModel> AgilityFor<TModel>(this WebPageBase webPage)
		{
			return new AgilityHelper<TModel>(webPage);
		}
	}
}