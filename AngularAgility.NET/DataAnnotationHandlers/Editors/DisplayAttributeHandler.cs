using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class DisplayAttributeHandler : IHandleDataAnnotations<EditorBuilder, DisplayAttribute>
	{
		public void Handle(EditorBuilder builder, DisplayAttribute att, AgilityMetadata metadata)
		{
			if (att.Prompt != null)
				builder.Attributes.Add("placeholder", att.Prompt);
		}
	}
}