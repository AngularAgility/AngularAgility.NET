using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Angular.Agility.DataAnnotationHandlers;
using Angular.Agility.Validation;

namespace Angular.Agility
{
	public class AgilityConfiguration
	{
		private readonly Dictionary<Type, AnnotationHandlerRunner> annotationHandlers = new Dictionary<Type, AnnotationHandlerRunner>();
		private bool configurationIsFinalized;
		private IList<IBuildInputs> inputBuilders = new List<IBuildInputs>();

		public AgilityConfiguration()
		{
			RegisterAnnotationHandler<RequiredAttribute, RequiredAttributeHandler>();
			RegisterAnnotationHandler<DisplayAttribute, DisplayAttributeHandler>();
			RegisterAnnotationHandler<RegularExpressionAttribute, RegularExpressionAttributeHandler>();
			RegisterAnnotationHandler<RangeAttribute, RangeAttributeHandler>();
			RegisterAnnotationHandler<EmailAddressAttribute, EmailAddressAttributeHandler>();
			RegisterAnnotationHandler<PhoneAttribute, PhoneAttributeHandler>();
		}

		public IList<IBuildInputs> InputBuilders
		{
			get { return inputBuilders; }
		}

		internal void FinalizeConfiguration()
		{
			inputBuilders.Add(new DefaultHtml5InputBuilder());

			inputBuilders = (inputBuilders as List<IBuildInputs> ?? new List<IBuildInputs>()).AsReadOnly();
			configurationIsFinalized = true;
		}

		public void RegisterAnnotationHandler<TAttribute, THandler>()
			where THandler : IHandleDataAnnotations<TAttribute>, new()
			where TAttribute : Attribute
		{
			var runner = new AnnotationHandlerRunner<TAttribute, THandler>();
			annotationHandlers[typeof(TAttribute)] = runner;
		}

		internal void RunAnnotations(EditorBuilder builder, IEnumerable<Attribute> attributes, AgilityMetadata metadata, FormContext formContext)
		{
			foreach (var att in attributes)
			{
				AnnotationHandlerRunner runner;
				if (annotationHandlers.TryGetValue(att.GetType(), out runner))
				{
					runner.DecorateEditor(builder, att, metadata);
					
					if (formContext != null)
					{
						var validationMessage = runner.GetValidationMessage(att, metadata);

						if (validationMessage != null)
						{
							validationMessage.InputName = metadata.Name;
							formContext.ValidationMessageData.Add(validationMessage);
						}
					}
				}
			}
		}

		internal void RunAnnotations(ValidationContainerBuilder builder, IEnumerable<Attribute> attributes, AgilityMetadata metadata)
		{
			foreach (var att in attributes)
			{
				AnnotationHandlerRunner runner;
				if (annotationHandlers.TryGetValue(att.GetType(), out runner))
				{
					var validationMessage = runner.GetValidationMessage(att, metadata);
					builder.AddValidationMessage(validationMessage);
				}
			}
		}

		public void RegisterInputBuilder(IBuildInputs inputBuilder)
		{
			ThrowIfConfigIsFinalized();
			inputBuilders.Add(inputBuilder);
		}

		private void ThrowIfConfigIsFinalized()
		{
			if (configurationIsFinalized)
				throw new InvalidOperationException("AgilityConfiguration cannot be modified once finalized.");
		}

		private abstract class AnnotationHandlerRunner
		{
			internal abstract void DecorateEditor(EditorBuilder builder, Attribute att, AgilityMetadata metadata);

			internal abstract ValidationMessageData GetValidationMessage(Attribute att, AgilityMetadata metadata);
		}

		private class AnnotationHandlerRunner<TAttribute, THandler> : AnnotationHandlerRunner
			where THandler : IHandleDataAnnotations<TAttribute>, new()
			where TAttribute : Attribute
		{
			private readonly IHandleDataAnnotations<TAttribute> handler;

			internal AnnotationHandlerRunner()
			{
				handler = new THandler();
			}

			internal override void DecorateEditor(EditorBuilder builder, Attribute att, AgilityMetadata metadata)
			{
				handler.DecorateEditor(builder, att as TAttribute, metadata);
			}

			internal override ValidationMessageData GetValidationMessage(Attribute att, AgilityMetadata metadata)
			{
				return handler.GetValidationMessage(att as TAttribute, metadata);
			}
		}
	}
}