using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Angular.Agility
{
	public abstract class AgilityPage<TModel> : WebViewPage<TModel>
	{
		public AgilityHelper<TModel> Angular { get; set; }

		public AgilityPage()
		{
				Angular = new AgilityHelper<TModel>(this);
		}
	}
}
