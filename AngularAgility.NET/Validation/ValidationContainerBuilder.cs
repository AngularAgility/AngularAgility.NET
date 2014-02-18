using Angular.Agility.DataAnnotationHandlers;
using System.Web;
using System.Web.Mvc;

namespace Angular.Agility.Validation
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

		public void AddValidationMessage(ValidationMessageData validationMessage)
		{
			this.InnerHtml += ValidationMessageFactory.BuildValidationMessage("span", this.FormName, this.InputName, validationMessage.ValidationName, validationMessage.Message);
		}
	}
}