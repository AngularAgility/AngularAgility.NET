using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Angular.Agility.DataAnnotationHandlers;
using Angular.Agility.DataAnnotationHandlers.Editors;

namespace Angular.Agility
{
	public class AgilityConfiguration
	{
		private readonly Dictionary<AnnotationHandlerKey, AnnotationHandlerRunner> annotationHandlers =
			new Dictionary<AnnotationHandlerKey, AnnotationHandlerRunner>();
		private bool configurationIsFinalized;
		private IList<IBuildInputs> inputBuilders = new List<IBuildInputs>();

		public AgilityConfiguration()
		{
			RegisterAnnotationHandler<EditorBuilder, RequiredAttribute, RequiredAttributeHandler>();
			RegisterAnnotationHandler<EditorBuilder, DisplayAttribute, DisplayAttributeHandler>();
			RegisterAnnotationHandler<EditorBuilder, RegularExpressionAttribute, RegularExpressionAttributeHandler>();
			RegisterAnnotationHandler<EditorBuilder, RangeAttribute, RangeAttributeHandler>();
			RegisterAnnotationHandler<EditorBuilder, EmailAddressAttribute, EmailAddressAttributeHandler>();
			RegisterAnnotationHandler<EditorBuilder, PhoneAttribute, PhoneAttributeHandler>();
			RegisterAnnotationHandler
				<ValidationContainerBuilder, RequiredAttribute, DataAnnotationHandlers.Validators.RequiredAttributeHandler>();
			RegisterAnnotationHandler
				<ValidationContainerBuilder, EmailAddressAttribute, DataAnnotationHandlers.Validators.EmailAddressAttributeHandler>();
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

		public void RegisterAnnotationHandler<TBuilder, TAttribute, THandler>()
			where THandler : IHandleDataAnnotations<TBuilder, TAttribute>, new()
			where TBuilder : TagBuilder
			where TAttribute : Attribute
		{
			var runner = new AnnotationHandlerRunner<TBuilder, TAttribute, THandler>();
			var key = new AnnotationHandlerKey(typeof(TBuilder), typeof(TAttribute));
			annotationHandlers[key] = runner;
		}

		internal void RunAnnotations(TagBuilder builder, IEnumerable<Attribute> attributes, AgilityMetadata metadata)
		{
			foreach (var att in attributes)
			{
				AnnotationHandlerRunner runner;
				var key = new AnnotationHandlerKey(builder.GetType(), att.GetType());
				if (annotationHandlers.TryGetValue(key, out runner))
					runner.Run(builder, att, metadata);
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

		private class AnnotationHandlerKey : Tuple<Type, Type>
		{
			public AnnotationHandlerKey(Type builderType, Type attributeType)
				: base(builderType, attributeType)
			{
			}
		}

		private abstract class AnnotationHandlerRunner
		{
			internal abstract void Run(TagBuilder builder, Attribute att, AgilityMetadata metadata);
		}

		private class AnnotationHandlerRunner<TBuilder, TAttribute, THandler> : AnnotationHandlerRunner
			where THandler : IHandleDataAnnotations<TBuilder, TAttribute>, new()
			where TAttribute : Attribute
			where TBuilder : TagBuilder
		{
			private readonly IHandleDataAnnotations<TBuilder, TAttribute> handler;

			internal AnnotationHandlerRunner()
			{
				handler = new THandler();
			}

			internal override void Run(TagBuilder builder, Attribute att, AgilityMetadata metadata)
			{
				handler.Handle(builder as TBuilder, att as TAttribute, metadata);
			}
		}
	}
}