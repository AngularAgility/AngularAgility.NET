using System;
using System.Web.Mvc;

namespace Angular.Agility.DataAnnotationHandlers
{
	public interface IHandleDataAnnotations<in TBuilder, in TAttribute>
		where TBuilder : TagBuilder
		where TAttribute : Attribute
	{
		void Handle(TBuilder builder, TAttribute att, AgilityMetadata metadata);
	}
}