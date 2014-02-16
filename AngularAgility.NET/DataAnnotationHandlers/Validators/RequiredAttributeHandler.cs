using System;
using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Validators
{
	public class RequiredAttributeHandler :
		IHandleDataAnnotations<ValidationContainerBuilder, RequiredAttribute>
	{
		public void Handle(ValidationContainerBuilder builder, RequiredAttribute att, AgilityMetadata metadata)
		{
			var error = att.ErrorMessage ?? String.Format("The {0} field is required.", metadata.DisplayName);
			var message = ValidationMessageFactory.BuildValidationMessage(builder.FormName, builder.InputName, "required", error);
			builder.InnerHtml += message;
		}
	}
}