using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Angular.Agility.Annotations;
using Angular.Agility.Validation;
using System.Web;

namespace Angular.Agility
{
	public class AgilityHelper<TModel>
	{
		internal readonly WebPageBase _page;

		public AgilityHelper(WebPageBase webPage)
		{
			_page = webPage;
			//TODO Define a config section for Agility, and set UseNamedInputs and UseBootstrapClasses from there
			UseNamedInputs = true;
			UseBootstrapClasses = true;
		}

		private bool UseBootstrapClasses { get; set; }
		private bool UseNamedInputs { get; set; }
		public FormContext FormContext { get; set; }

		#region EditorFor

		public IBuildEditors EditorFor<TParameter>(Expression<Func<TModel, TParameter>> expression, string ngModel = null,
			object htmlAttributes = null)
		{
			RouteValueDictionary htmlAttributesDictionary = AnonymousObjectToHtmlAttributes(htmlAttributes);
			return EditorFor(expression, ngModel, htmlAttributesDictionary);
		}

		[UsedImplicitly]
		public IBuildEditors EditorFor<TParameter>(Expression<Func<TModel, TParameter>> expression, string ngModel,
			IDictionary<string, object> htmlAttributes)
		{
			AgilityMetadata metadata = AgilityMetadata.FromLambdaExpression(expression);
			EditorBuilder tag = BuildInput(metadata);

			ApplyMetadataToEditor(tag, metadata, ngModel, htmlAttributes);

			return tag;
		}

		#endregion

		#region DropDownListFor

		public IBuildEditors DropDownListFor<TParameter>(Expression<Func<TModel, TParameter>> expression, IEnumerable<SelectListItem> selectListItems,
			string ngModel = null, object htmlAttributes = null)
		{
			RouteValueDictionary htmlAttributesDictionary = AnonymousObjectToHtmlAttributes(htmlAttributes);
			return DropDownListFor(expression, selectListItems, ngModel, htmlAttributesDictionary);
		}

		[UsedImplicitly]
		public IBuildEditors DropDownListFor<TParameter>(Expression<Func<TModel, TParameter>> expression, IEnumerable<SelectListItem> selectListItems,
			string ngModel, IDictionary<string, object> htmlAttributes)
		{
			AgilityMetadata metadata = AgilityMetadata.FromLambdaExpression(expression);
			EditorBuilder tag = BuildSelectList(metadata, selectListItems);

			ApplyMetadataToEditor(tag, metadata, ngModel, htmlAttributes);

			return tag;
		}

		#endregion

		#region ValidationMessageFor

		public IBuildValidationContainers ValidationMessageFor<TParameter>(Expression<Func<TModel, TParameter>> expression)
		{
			AgilityMetadata metadata = AgilityMetadata.FromLambdaExpression(expression);
			string formName = GetUsableFormName(metadata);
			string inputName = metadata.Name;
			return ValidationMessageFor(expression, formName, inputName);
		}

		public IBuildValidationContainers ValidationMessageFor<TParameter>(Expression<Func<TModel, TParameter>> expression,
			string formName)
		{
			AgilityMetadata metadata = AgilityMetadata.FromLambdaExpression(expression);
			string inputName = metadata.Name;
			return ValidationMessageFor(expression, formName, inputName);
		}

		public IBuildValidationContainers ValidationMessageFor<TParameter>(Expression<Func<TModel, TParameter>> expression,
			string formName, string inputName)
		{
			AgilityMetadata metadata = AgilityMetadata.FromLambdaExpression(expression);
			ValidationContainerBuilder tag = BuildValidationContainer(formName, inputName);
			AgilityStartup.Config.RunAnnotations(tag, metadata.MemberAttributes, metadata);
			return tag;
		}

		#endregion

		#region ValidationSummary

		public IHtmlString ValidationSummary(string message = null, object htmlAttributes = null)
		{
			RouteValueDictionary htmlAttributesDictionary = AnonymousObjectToHtmlAttributes(htmlAttributes);
			return ValidationSummary(message, htmlAttributesDictionary);
		}

		public IHtmlString ValidationSummary(string message, IDictionary<string, object> htmlAttributes)
		{
			if (this.FormContext == null)
			{
				throw new InvalidOperationException("Cannot create validation summary without a form");
			}

			return new ValidationSummaryBuilder(this.FormContext, message);
		}


		#endregion

		#region BeginForm

		public IBuildForms<TModel> BeginForm()
		{
			return BeginForm(null, null);
		}

		public IBuildForms<TModel> BeginForm(string formName)
		{
			return BeginForm(formName, null);
		}

		public IBuildForms<TModel> BeginForm(object htmlAttributes)
		{
			RouteValueDictionary htmlAttributesDictionary = AnonymousObjectToHtmlAttributes(htmlAttributes);
			return BeginForm(null, htmlAttributesDictionary);
		}

		public IBuildForms<TModel> BeginForm(Dictionary<string, object> htmlAttributes)
		{
			return BeginForm(null, htmlAttributes);
		}

		public IBuildForms<TModel> BeginForm(string formName, object htmlAttributes)
		{
			RouteValueDictionary htmlAttributesDictionary = AnonymousObjectToHtmlAttributes(htmlAttributes);
			return BeginForm(formName, htmlAttributesDictionary);
		}

		public IBuildForms<TModel> BeginForm(string formName, IDictionary<string, object> htmlAttributes)
		{
			if (formName == null)
			{
				formName = typeof(TModel).Name + "Form";

			} 
			var tag = BuildForm(formName, htmlAttributes);
			return tag;
		}

		#endregion

		#region Privates

		//This function is based on code in the HtmlHelper source code.
		private static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
		{
			return HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
		}

		private ValidationContainerBuilder BuildValidationContainer(string formName,
			string inputName)
		{
			var builder = new ValidationContainerBuilder {InputName = inputName, FormName = formName};
			string showCondition = String.Format("{0}.{1}.$dirty && {0}.{1}.$invalid", formName, inputName);
			builder.Attributes.Add("ng-show", showCondition);
			return builder;
		}

		private EditorBuilder BuildInput(AgilityMetadata metadata)
		{
			string dataTypeName = metadata.GetDataTypeName();
			foreach (IBuildInputs inputBuilder in AgilityStartup.Config.InputBuilders)
			{
				if (inputBuilder.CanBuild(dataTypeName))
				{
					return inputBuilder.Build(metadata);
				}
			}
			throw new InvalidOperationException("Unable to create an input. There isn't any way this should be able to happen.");
		}

		private EditorBuilder BuildSelectList(AgilityMetadata metadata, IEnumerable<SelectListItem> selectListItems)
		{
			var selectListBuilder = AgilityStartup.Config.InputBuilders.OfType<IBuildSelectList>().FirstOrDefault();

			if (selectListBuilder == null)
			{
				throw new InvalidOperationException("Unable to create an select list. There isn't any way this should be able to happen.");
			}

			return selectListBuilder.BuildSelectList(metadata, selectListItems);
		}

		private void ApplyMetadataToEditor(EditorBuilder tag, AgilityMetadata metadata, string ngModel, IDictionary<string, object> htmlAttributes)
		{
			if (ngModel == null)
			{
				// If no model binding string is specified, we still need to set ng-model so validation occurs.
				// The ng-model directive itself adds a property to the parent form corresponding to its input 
				// name, so we can't use that.  So instead we attach an "AgilityBindings" object to the form and bind to that.
				ngModel = GetUsableFormName(metadata) + ".AgilityBindings." + metadata.Name;
			}

			tag.Model(ngModel);

			AgilityStartup.Config.RunAnnotations(tag, metadata.MemberAttributes, metadata, this.FormContext);

			tag.MergeAttributes(htmlAttributes);

			// TODO: Make adding the Bootstrap class optional
			if (UseBootstrapClasses)
				tag.AddCssClass("form-control");

			// TODO: Make adding the name attribute optional
			if (UseNamedInputs)
				tag.MergeAttribute("name", metadata.Name, replaceExisting: false);
		}

		private FormBuilder<TModel> BuildForm(string formName, IDictionary<string, object> htmlAttributes)
		{
			var builder = new FormBuilder<TModel>(this, formName, htmlAttributes);
			return builder;
		}

		private string GetUsableFormName(AgilityMetadata metadata)
		{
			var formContextName = FormContext == null ? null : FormContext.FormName;
			return formContextName ?? metadata.ModelType.Name;
		}

		#endregion
	}
}