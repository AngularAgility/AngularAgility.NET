using Angular.Agility.Validation;
using System;
using System.Web.Mvc;

namespace Angular.Agility.DataAnnotationHandlers
{
	public interface IHandleDataAnnotations<in TAttribute>
		where TAttribute : Attribute
	{
		void DecorateEditor(EditorBuilder builder, TAttribute att, AgilityMetadata metadata);

		ValidationMessageData GetValidationMessage(TAttribute att, AgilityMetadata metadata);
	}
}