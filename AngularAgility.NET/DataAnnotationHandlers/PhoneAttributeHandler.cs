using System.ComponentModel.DataAnnotations;
using Angular.Agility.Validation;

namespace Angular.Agility.DataAnnotationHandlers
{
	public class PhoneAttributeHandler : IHandleDataAnnotations<PhoneAttribute>
	{
		public void DecorateEditor(EditorBuilder tag, PhoneAttribute att, AgilityMetadata metadata)
		{
			tag.Attributes["type"] = "tel";
		}

		public ValidationMessageData GetValidationMessage(PhoneAttribute att, AgilityMetadata metadata)
		{
			return null; // TODO
		}
	}
}