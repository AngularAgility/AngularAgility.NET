using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Angular.Agility
{
	public class FormGroupBuilder : IHtmlString
	{
		private readonly EditorBuilder editorHtml;
		private readonly AgilityMetadata metadata;

		internal FormGroupBuilder(EditorBuilder editorHtml, AgilityMetadata metadata)
		{
			this.editorHtml = editorHtml;
			this.metadata = metadata;
		}

		public string ToHtmlString()
		{
			var div = new TagBuilder("div");
			div.AddCssClass("form-group");

			var sb = new StringBuilder();
			sb.AppendFormat("<label class=\"control-label col-md-{0}\" for=\"{1}\">{2}</label>", 2, editorHtml.Id,
				metadata.DisplayName);
			sb.AppendFormat("<div class=\"col-md-{0}\">", 10);
			sb.Append((editorHtml as IHtmlString).ToHtmlString());
			sb.Append("</div>");

			div.InnerHtml = sb.ToString();
			return div.ToString();
		}
	}
}