using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class RequiredAttributeHandler : IHandleDataAnnotations<EditorBuilder, RequiredAttribute>
	{
		public void Handle(EditorBuilder builder, RequiredAttribute att, AgilityMetadata metadata)
		{
			builder.Attributes.Add("required", null);
		}
	}
}