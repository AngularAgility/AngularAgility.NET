using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class PhoneAttributeHandler : IHandleDataAnnotations<EditorBuilder, PhoneAttribute>
	{
		public void Handle(EditorBuilder tag, PhoneAttribute att, AgilityMetadata metadata)
		{
			tag.Attributes["type"] = "tel";
		}
	}
}