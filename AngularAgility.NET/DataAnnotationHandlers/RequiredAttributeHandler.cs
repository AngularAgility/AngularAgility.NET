using System.ComponentModel.DataAnnotations;
using Angular.Agility.Validation;

namespace Angular.Agility.DataAnnotationHandlers
{
	public class RequiredAttributeHandler : IHandleDataAnnotations<RequiredAttribute>
	{
		public void DecorateEditor(EditorBuilder builder, RequiredAttribute att, AgilityMetadata metadata)
		{
			var error = att.ErrorMessage ?? string.Format("The {0} field is required.", metadata.DisplayName);
			builder.Attributes.Add("required", error);
		}

		public ValidationMessageData GetValidationMessage(RequiredAttribute att, AgilityMetadata metadata)
		{
			return new ValidationMessageData()
			{
				ValidationName = "required",
				Message = att.ErrorMessage ?? string.Format("The {0} field is required.", metadata.DisplayName)
			};
		}
	}
}