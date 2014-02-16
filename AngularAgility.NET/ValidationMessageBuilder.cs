using System.Web;
using System.Web.Mvc;

namespace Angular.Agility
{
	public interface IBuildValidationMessages : IHtmlString
	{
	}

	public class ValidationMessageBuilder : TagBuilder, IBuildValidationMessages
	{
		public ValidationMessageBuilder() : base("span")
		{
		}

		public AgilityMetadata Metadata { get; set; }

		public string ToHtmlString()
		{
			return ToString();
		}
	}
}