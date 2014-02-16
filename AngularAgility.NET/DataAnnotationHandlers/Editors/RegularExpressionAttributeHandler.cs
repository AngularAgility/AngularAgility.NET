using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class RegularExpressionAttributeHandler : IHandleDataAnnotations<EditorBuilder, RegularExpressionAttribute>
	{
		public void Handle(EditorBuilder builder, RegularExpressionAttribute att, AgilityMetadata metadata)
		{
			if (att.Pattern != null)
				builder.Attributes.Add("ng-pattern", ConvertRegexToJavaScript(att.Pattern));
		}

		private static string ConvertRegexToJavaScript(string pattern)
		{
			// TODO: This is obviously a naive implementation
			return "/" + pattern + "/";
		}
	}
}