using System.Web;
using System.Web.Mvc;

namespace Angular.Agility
{
	public interface IBuildValidationContainers : IHtmlString
	{
	}

	public class ValidationContainerBuilder : TagBuilder, IBuildValidationContainers
	{
		public ValidationContainerBuilder() : base("div")
		{
		}

		public string InputName { get; set; }

		public string FormName { get; set; }

		public string ToHtmlString()
		{
			return ToString();
		}
	}
}