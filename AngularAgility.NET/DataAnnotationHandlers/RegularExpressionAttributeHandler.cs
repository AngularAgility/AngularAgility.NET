using System.ComponentModel.DataAnnotations;
using Angular.Agility.Validation;

namespace Angular.Agility.DataAnnotationHandlers
{
	public class RegularExpressionAttributeHandler : IHandleDataAnnotations<RegularExpressionAttribute>
	{
		public void DecorateEditor(EditorBuilder builder, RegularExpressionAttribute att, AgilityMetadata metadata)
		{
			if (att.Pattern != null)
				builder.Attributes.Add("ng-pattern", ConvertRegexToJavaScript(att.Pattern));
		}

		private static string ConvertRegexToJavaScript(string pattern)
		{
			// TODO: This is obviously a naive implementation
			return "/" + pattern + "/";
		}

		public ValidationMessageData GetValidationMessage(RegularExpressionAttribute att, AgilityMetadata metadata)
		{
			return null; // TODO
		}
	}
}