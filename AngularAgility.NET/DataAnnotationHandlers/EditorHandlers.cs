using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class RequiredAttributeHandler : IHandleDataAnnotations<EditorBuilder, RequiredAttribute>
	{
		public void Handle(EditorBuilder builder, RequiredAttribute att, AgilityMetadata metadata)
		{
			builder.Attributes.Add("required", null);
		}
	}

	public class DisplayAttributeHandler : IHandleDataAnnotations<EditorBuilder, DisplayAttribute>
	{
		public void Handle(EditorBuilder builder, DisplayAttribute att, AgilityMetadata metadata)
		{
			if(att.Prompt != null)
				builder.Attributes.Add("placeholder", att.Prompt);
		}
	}

	public class RegularExpressionAttributeHandler : IHandleDataAnnotations<EditorBuilder, RegularExpressionAttribute>
	{
		public void Handle(EditorBuilder builder, RegularExpressionAttribute att, AgilityMetadata metadata)
		{
			if(att.Pattern != null)
				builder.Attributes.Add("ng-pattern", ConvertRegexToJavaScript(att.Pattern));
		}

		private static string ConvertRegexToJavaScript(string pattern)
		{
			// TODO: This is obviously a naive implementation
			return "/" + pattern + "/";
		}
	}

	public class RangeAttributeHandler : IHandleDataAnnotations<EditorBuilder, RangeAttribute>
	{
		public void Handle(EditorBuilder tag, RangeAttribute att, AgilityMetadata metadata)
		{
			object min = att.Minimum;
			object max = att.Maximum;
			var type = att.OperandType;
			if (type == typeof(int))
			{
				if ((int)min != int.MinValue)
					tag.Attributes.Add("min", min.ToString());
				if ((int)max != int.MaxValue)
					tag.Attributes.Add("max", max.ToString());
			}
			else if (type == typeof(double))
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if ((double)min != double.MaxValue)
					tag.Attributes.Add("min", min.ToString());
				if ((double)max != double.MaxValue)
					tag.Attributes.Add("max", max.ToString());
				// ReSharper restore CompareOfFloatsByEqualityOperator
			}
			else
			{
				tag.Attributes.Add("min", min.ToString());
				tag.Attributes.Add("max", max.ToString());
			}
		}
	}

	public class EmailAddressAttributeHandler : IHandleDataAnnotations<EditorBuilder, EmailAddressAttribute>
	{
		public void Handle(EditorBuilder tag, EmailAddressAttribute att, AgilityMetadata metadata)
		{
			tag.Attributes["type"] = "email";
		}
	}
}
