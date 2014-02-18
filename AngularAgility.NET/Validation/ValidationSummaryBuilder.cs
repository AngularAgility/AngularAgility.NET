using Angular.Agility.DataAnnotationHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Angular.Agility.Validation
{
	public class ValidationSummaryBuilder : TagBuilder, IHtmlString
	{
		public ValidationSummaryBuilder(FormContext formContext, string message)
			: base("div")
		{
			var title = new TagBuilder("div");
			title.InnerHtml = HttpUtility.HtmlEncode(message);

			var ul = new TagBuilder("ul");
			var listItems = new StringBuilder();
			foreach (var validationMessage in formContext.ValidationMessageData)
			{
				var li = ValidationMessageFactory.BuildValidationMessage("li", formContext.FormName, validationMessage.InputName, validationMessage.ValidationName, validationMessage.Message);
				listItems.AppendLine(li);
			}
			ul.InnerHtml = listItems.ToString();

			this.InnerHtml = title.ToString() + ul.ToString();
		}

		public string ToHtmlString()
		{
			return ToString();
		}
	}
}
