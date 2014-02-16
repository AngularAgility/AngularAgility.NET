using System;
using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Validators
{
	public class EmailAddressAttributeHandler :
		IHandleDataAnnotations<ValidationContainerBuilder, EmailAddressAttribute>
	{
		public void Handle(ValidationContainerBuilder builder, EmailAddressAttribute att, AgilityMetadata metadata)
		{
			var error = att.ErrorMessage != null
				? String.Format(att.ErrorMessage, metadata.DisplayName)
				: "Please enter a valid e-mail address.";
			var message = ValidationMessageFactory.BuildValidationMessage(builder.FormName, builder.InputName, "email", error);
			builder.InnerHtml += message;
		}
	}
}