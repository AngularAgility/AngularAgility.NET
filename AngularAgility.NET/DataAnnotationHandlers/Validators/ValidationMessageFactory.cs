using System;

namespace Angular.Agility.DataAnnotationHandlers.Validators
{
	internal static class ValidationMessageFactory
	{
		internal static ValidationMessageBuilder BuildValidationMessage(string form, string input, string validation,
			string message)
		{
			var builder = new ValidationMessageBuilder();
			var showCondition = String.Format("{0}.{1}.$error.{2}", form, input, validation);
			builder.Attributes.Add("ng-show", showCondition);
			builder.InnerHtml = message;
			return builder;
		}
	}
}