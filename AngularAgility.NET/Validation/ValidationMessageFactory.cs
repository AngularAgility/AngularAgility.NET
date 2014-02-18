using System;
using System.Web;
using System.Web.Mvc;

namespace Angular.Agility.Validation
{
	internal static class ValidationMessageFactory
	{
		internal static string BuildValidationMessage(string tagName, string form, string input, string validation,
			string message)
		{
			var builder = new TagBuilder(tagName);
			var showCondition = String.Format("{0}['{1}'].$error.{2}", form, input, validation);
			builder.Attributes.Add("ng-show", showCondition);
			builder.InnerHtml = HttpUtility.HtmlEncode(message);
			return builder.ToString();
		}
	}
}