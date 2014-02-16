using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class EmailAddressAttributeHandler : IHandleDataAnnotations<EditorBuilder, EmailAddressAttribute>
	{
		public void Handle(EditorBuilder tag, EmailAddressAttribute att, AgilityMetadata metadata)
		{
			tag.Attributes["type"] = "email";
		}
	}
}