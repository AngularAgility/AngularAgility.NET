namespace Angular.Agility
{
	public interface IBuildInputs
	{
		bool CanBuild(string dataType);
		EditorBuilder Build(string dataType);
	}

	public class DefaultHtml5InputBuilder : IBuildInputs
	{
		public bool CanBuild(string dataType)
		{
			return true;
		}

		public EditorBuilder Build(string dataType)
		{
			EditorBuilder tag;
			switch (dataType)
			{
				case "DateTime":
					tag = new EditorBuilder("input", new {type = "datetime"});
					break;
				case "Date":
					tag = new EditorBuilder("input", new {type = "date"});
					break;
				case "Time":
					tag = new EditorBuilder("input", new {type = "time"});
					break;
				case "Html":
				case "MultilineText":
					tag = new EditorBuilder("textarea");
					break;
				case "PhoneNumber":
					tag = new EditorBuilder("input", new { type = "tel" });
					break;
				case "Currency":
				case "Number":
					tag = new EditorBuilder("input", new {type = "number"});
					break;
				case "EmailAddress":
					tag = new EditorBuilder("input", new {type = "email"});
					break;
				case "Password":
					tag = new EditorBuilder("input", new {type = "password"});
					break;
				case "Url":
				case "ImageUrl":
					tag = new EditorBuilder("input", new {type = "url"});
					break;
					//case "Text":
					//case "Duration":
					//case "Default":
				default:
					tag = new EditorBuilder("input", new {type = "text"});
					break;
			}

			return tag;
		}
	}
}