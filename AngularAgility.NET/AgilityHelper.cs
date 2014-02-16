using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Angular.Agility.Annotations;

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
		public string FormName { get; set; }

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
			if (ngModel != null)
				tag.Model(ngModel);

			AgilityStartup.Config.RunAnnotations(tag, metadata.MemberAttributes, metadata);

			tag.MergeAttributes(htmlAttributes);

			// TODO: Make adding the Bootstrap class optional
			if (UseBootstrapClasses)
				tag.AddCssClass("form-control");

			// TODO: Make adding the name attribute optional
			if (UseNamedInputs)
				tag.Attributes.Add("name", metadata.Name);

			return tag;
		}

		#endregion

		#region ValidationMessageFor

		public IBuildValidationContainers ValidationMessageFor<TParameter>(Expression<Func<TModel, TParameter>> expression)
		{
			AgilityMetadata metadata = AgilityMetadata.FromLambdaExpression(expression);
			string formName = FormName ?? metadata.ModelType.Name;
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
					EditorBuilder builder = inputBuilder.Build(dataTypeName);
					builder.Metadata = metadata;
					return builder;
				}
			}
			throw new InvalidOperationException("Unable to create an input. There isn't any way this should be able to happen.");
		}

		private FormBuilder<TModel> BuildForm(string formName, IDictionary<string, object> htmlAttributes)
		{
			var builder = new FormBuilder<TModel>(this, formName, htmlAttributes);
			return builder;
		}

		#endregion
	}
}