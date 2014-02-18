using System.ComponentModel.DataAnnotations;
using Angular.Agility.Validation;

namespace Angular.Agility.DataAnnotationHandlers
{
	public class DisplayAttributeHandler : IHandleDataAnnotations<DisplayAttribute>
	{
		public void DecorateEditor(EditorBuilder builder, DisplayAttribute att, AgilityMetadata metadata)
		{
			if (att.Prompt != null)
				builder.Attributes.Add("placeholder", att.Prompt);
		}

		public ValidationMessageData GetValidationMessage(DisplayAttribute att, AgilityMetadata metadata)
		{
			return null; // None required.
		}
	}
}