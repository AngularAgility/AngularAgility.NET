using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Angular.Agility
{
	public interface IBuildInputs
	{
		bool CanBuild(string dataType);
		EditorBuilder Build(AgilityMetadata metadata);
	}

	public interface IBuildSelectList
	{
		EditorBuilder BuildSelectList(AgilityMetadata metadata, IEnumerable<SelectListItem> selectListItems);
	}

	public class DefaultHtml5InputBuilder : IBuildInputs, IBuildSelectList
	{
		public bool CanBuild(string dataType)
		{
			return true;
		}

		public EditorBuilder Build(AgilityMetadata metadata)
		{
			EditorBuilder tag;
			switch (metadata.GetDataTypeName())
			{
				case "DateTime":
					tag = new EditorBuilder("input", metadata, new {type = "datetime"});
					break;
				case "Date":
					tag = new EditorBuilder("input", metadata, new { type = "date" });
					break;
				case "Time":
					tag = new EditorBuilder("input", metadata, new { type = "time" });
					break;
				case "Html":
				case "MultilineText":
					tag = new EditorBuilder("textarea", metadata);
					break;
				case "PhoneNumber":
					tag = new EditorBuilder("input", metadata, new { type = "tel" });
					break;
				case "Currency":
				case "Number":
					tag = new EditorBuilder("input", metadata, new { type = "number" });
					break;
				case "EmailAddress":
					tag = new EditorBuilder("input", metadata, new { type = "email" });
					break;
				case "Password":
					tag = new EditorBuilder("input", metadata, new { type = "password" });
					break;
				case "Url":
				case "ImageUrl":
					tag = new EditorBuilder("input", metadata, new { type = "url" });
					break;
					//case "Text":
					//case "Duration":
					//case "Default":
				default:
					tag = new EditorBuilder("input", metadata, new { type = "text" });
					break;
			}

			return tag;
		}

		public EditorBuilder BuildSelectList(AgilityMetadata metadata, IEnumerable<SelectListItem> selectListItems)
		{
			// Partially reflectored from MVC source code
			var stringBuilder = new StringBuilder();
			foreach (var item in selectListItems)
			{
				TagBuilder tagBuilder = new TagBuilder("option");
				tagBuilder.InnerHtml = HttpUtility.HtmlEncode(item.Text);
				if (item.Value != null)
				{
					tagBuilder.Attributes["value"] = item.Value;
				}
				if (item.Selected)
				{
					tagBuilder.Attributes["selected"] = "selected";
				}
				stringBuilder.AppendLine(tagBuilder.ToString());
			}

			var selectBuilder = new EditorBuilder("select", metadata);
			selectBuilder.InnerHtml = stringBuilder.ToString();
			return selectBuilder;
		}
	}
}