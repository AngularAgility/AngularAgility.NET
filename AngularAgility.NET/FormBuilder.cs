using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Angular.Agility
{
	public interface IBuildForms<TModel> : IDisposable
	{
		AgilityHelper<TModel> AgilityHelper { get; set; }
	}

	public class FormBuilder<TModel> : IBuildForms<TModel>
	{
		public FormBuilder(AgilityHelper<TModel> agilityHelper, string formName, IDictionary<string, object> htmlAttributes)
		{
			AgilityHelper = agilityHelper;
			AgilityHelper.FormContext = new FormContext() { FormName = formName };

			var tag = new TagBuilder("form");
			tag.MergeAttributes(htmlAttributes);
			tag.MergeAttribute("name", formName);


			AgilityHelper._page.Output.Write(tag.ToString(TagRenderMode.StartTag));
		}

		public AgilityHelper<TModel> AgilityHelper { get; set; }

		public void Dispose()
		{
			AgilityHelper._page.Output.Write("</form>");
			AgilityHelper.FormContext = null;
		}
	}
}