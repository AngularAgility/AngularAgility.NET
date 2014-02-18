using System.ComponentModel.DataAnnotations;
using Angular.Agility.Validation;

namespace Angular.Agility.DataAnnotationHandlers
{
	public class EmailAddressAttributeHandler : IHandleDataAnnotations<EmailAddressAttribute>
	{
		public void DecorateEditor(EditorBuilder tag, EmailAddressAttribute att, AgilityMetadata metadata)
		{
			tag.Attributes["type"] = "email";
		}

		public ValidationMessageData GetValidationMessage(EmailAddressAttribute att, AgilityMetadata metadata)
		{
			var message = att.ErrorMessage != null
				? string.Format(att.ErrorMessage, metadata.DisplayName)
				: "Please enter a valid e-mail address.";

			return new ValidationMessageData()
			{
				ValidationName = "email",
				Message = message
			};
		}
	}
}