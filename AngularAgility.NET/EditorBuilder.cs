using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Angular.Agility
{
	public interface IBuildEditors : IHtmlString
	{
		IBuildEditors Model(string angularModelExpression);
		IBuildEditors Directive(string bareDirectiveName);
		IBuildEditors Directive(string directiveName, string directiveValue);
		FormGroupBuilder InFormGroup();
	}

	public class EditorBuilder : TagBuilder, IBuildEditors
	{
		private static readonly HashSet<string> SelfClosingTags;

		static EditorBuilder()
		{
			SelfClosingTags = new HashSet<string>(new[]
			{
				"area",
				"base",
				"br",
				"col",
				"command",
				"embed",
				"hr",
				"img",
				"input",
				"keygen",
				"link",
				"meta",
				"param",
				"source",
				"track",
				"wbr"
			}, StringComparer.InvariantCultureIgnoreCase);
		}

		public EditorBuilder(string tagName)
			: this(tagName, null)
		{
		}

		public EditorBuilder(string tagName, object htmlAttributes)
			: base(tagName)
		{
			if (htmlAttributes != null)
				MergeAttributes(new RouteValueDictionary(htmlAttributes));
		}

		public AgilityMetadata Metadata { get; internal set; }

		public string Id
		{
			get
			{
				EnsureId();
				return Attributes["id"];
			}
		}

		string IHtmlString.ToHtmlString()
		{
			EnsureId();

			var renderMode = IsSelfClosingTag()
				? TagRenderMode.SelfClosing
				: TagRenderMode.Normal;

			return ToString(renderMode);
		}

		public IBuildEditors Model(string angularModelExpression)
		{
			Attributes.Add("ng-model", angularModelExpression);
			return this;
		}

		public IBuildEditors Directive(string bareDirectiveName)
		{
			Attributes.Add(bareDirectiveName, null);
			return this;
		}

		public IBuildEditors Directive(string directiveName, string directiveValue)
		{
			Attributes.Add(directiveName, directiveValue);
			return this;
		}

		public FormGroupBuilder InFormGroup()
		{
			return new FormGroupBuilder(this, Metadata);
		}

		private void EnsureId()
		{
			if (!Attributes.ContainsKey("id"))
				Attributes.Add("id", Metadata.Name);
		}

		private bool IsSelfClosingTag()
		{
			if (!String.IsNullOrEmpty(InnerHtml))
				return false;

			return SelfClosingTags.Contains(TagName);
		}
	}
}