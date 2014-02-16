using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular.Agility.DataAnnotationHandlers.Validators
{
	public class RequiredAttributeHandler :
		IHandleDataAnnotations<ValidationContainerBuilder, RequiredAttribute>
	{
		public void Handle(ValidationContainerBuilder builder, RequiredAttribute att, AgilityMetadata metadata)
		{
			var error = att.ErrorMessage != null ? att.ErrorMessage : String.Format("The {0} field is required.", metadata.DisplayName);
			var message = ValidationFactory.BuildValidationMessage(builder.FormName, builder.InputName, "required", error);
			builder.InnerHtml += message;
		}
	}

	public class EmailAddressAttributeHandler :
		IHandleDataAnnotations<ValidationContainerBuilder, EmailAddressAttribute>
	{
		public void Handle(ValidationContainerBuilder builder, EmailAddressAttribute att, AgilityMetadata metadata)
		{
			var error = att.ErrorMessage != null ? String.Format(att.ErrorMessage, metadata.DisplayName) : "Please enter a valid e-mail address.";
			var message = ValidationFactory.BuildValidationMessage(builder.FormName, builder.InputName, "email", error);
			builder.InnerHtml += message;
		}
	}

	static class ValidationFactory 
	{
		internal static ValidationMessageBuilder BuildValidationMessage(string form, string input, string validation, string message)
		{
			var builder = new ValidationMessageBuilder();
			var showCondition = String.Format("{0}.{1}.$error.{2}", form, input, validation);
			builder.Attributes.Add("ng-show", showCondition);
			builder.InnerHtml = message;
			return builder;
		}
	}



}
