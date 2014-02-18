using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Angular.Agility
{
	public class AgilityMetadata
	{
		private AgilityMetadata()
		{
		}

		public string Name { get; private set; }
		public string DisplayName { get; private set; }
		public Type ModelType { get; private set; }
		public Type MemberType { get; private set; }
		public IList<Attribute> MemberAttributes { get; private set; }
		public DataAnnotations Annotations { get; private set; }

		public static AgilityMetadata FromLambdaExpression<TModel, TParameter>(Expression<Func<TModel, TParameter>> expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var exprText = ExpressionHelper.GetExpressionText(expression);

			switch (expression.Body.NodeType)
			{
				//case ExpressionType.ArrayIndex:
				//	// ArrayIndex always means a single-dimensional indexer; multi-dimensional indexer is a method call to Get()
				//	legalExpression = true;
				//	break;

				//case ExpressionType.Call:
				//	// Only legal method call is a single argument indexer/DefaultMember call
				//	legalExpression = ExpressionHelper.IsSingleArgumentIndexer(expression.Body);
				//	break;

				case ExpressionType.MemberAccess:
					// Property/field access is always legal
					var memberExpression = (MemberExpression)expression.Body;
					//propertyName = memberExpression.Member is PropertyInfo ? memberExpression.Member.Name : null;
					//containerType = memberExpression.Expression.Type;
					return FromReflectedMember<TModel, TParameter>(memberExpression.Member, exprText);

				//case ExpressionType.Parameter:
				//	// Parameter expression means "model => model", so we delegate to FromModel
				//	return FromModel(viewData, metadataProvider);

				default:
					throw new ArgumentException("Unsupported lambda expression.", "expression");
			}
		}

		public static AgilityMetadata FromReflectedMember<TModel, TParameter>(MemberInfo info, string name)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			var md = new AgilityMetadata
			{
				ModelType = typeof(TModel),
				MemberType = typeof(TParameter),
				MemberAttributes = info.GetCustomAttributes(true).Cast<Attribute>().ToList().AsReadOnly()
			};

			md.Annotations = new DataAnnotations(md.MemberAttributes);

			md.Name = name;
			if (md.Annotations.Display != null && md.Annotations.Display.Name != null)
			{
				md.DisplayName = md.Annotations.Display.Name;
			}
			else if (md.Annotations.DisplayName != null)
			{
				md.DisplayName = md.Annotations.DisplayName.DisplayName;
			}
			else
			{
				md.DisplayName = info.Name;
			}

			return md;
		}

		public string GetDataTypeName()
		{
			if (Annotations.DataType != null)
				return Annotations.DataType.CustomDataType ?? Annotations.DataType.DataType.ToString();

			if (MemberType == typeof(string))
				return "String";

			if (IsNumericType(MemberType))
				return "Number";

			return "Default";
		}

		private static bool IsNumericType(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}
	}

	public class DataAnnotations
	{
		public DataAnnotations(IEnumerable<Attribute> attributes)
		{
			var lookup = attributes.ToLookup(a => a.GetType());

			DataType = lookup[typeof(DataTypeAttribute)].Cast<DataTypeAttribute>().FirstOrDefault();
			Display = lookup[typeof(DisplayAttribute)].Cast<DisplayAttribute>().FirstOrDefault();
			DisplayColumn = lookup[typeof(DisplayColumnAttribute)].Cast<DisplayColumnAttribute>().FirstOrDefault();
			DisplayFormat = lookup[typeof(DisplayFormatAttribute)].Cast<DisplayFormatAttribute>().FirstOrDefault();
			DisplayName = lookup[typeof(DisplayNameAttribute)].Cast<DisplayNameAttribute>().FirstOrDefault();
			Editable = lookup[typeof(EditableAttribute)].Cast<EditableAttribute>().FirstOrDefault();
			EmailAddress = lookup[typeof(EmailAddressAttribute)].Cast<EmailAddressAttribute>().FirstOrDefault();
			HiddenInput = lookup[typeof(HiddenInputAttribute)].Cast<HiddenInputAttribute>().FirstOrDefault();
			ReadOnly = lookup[typeof(ReadOnlyAttribute)].Cast<ReadOnlyAttribute>().FirstOrDefault();
			Required = lookup[typeof(RequiredAttribute)].Cast<RequiredAttribute>().FirstOrDefault();
			ScaffoldColumn = lookup[typeof(ScaffoldColumnAttribute)].Cast<ScaffoldColumnAttribute>().FirstOrDefault();
			UIHint = lookup[typeof(UIHintAttribute)].Cast<UIHintAttribute>().FirstOrDefault();
			RegularExpression = lookup[typeof(RegularExpressionAttribute)].Cast<RegularExpressionAttribute>().FirstOrDefault();
			Range = lookup[typeof(RangeAttribute)].Cast<RangeAttribute>().FirstOrDefault();
		}

		public DataTypeAttribute DataType { get; private set; }
		public DisplayAttribute Display { get; private set; }
		public DisplayColumnAttribute DisplayColumn { get; private set; }
		public DisplayFormatAttribute DisplayFormat { get; private set; }
		public DisplayNameAttribute DisplayName { get; private set; }
		public EditableAttribute Editable { get; private set; }
		public EmailAddressAttribute EmailAddress { get; private set; }
		public HiddenInputAttribute HiddenInput { get; private set; }
		public ReadOnlyAttribute ReadOnly { get; private set; }
		public RequiredAttribute Required { get; private set; }
		public ScaffoldColumnAttribute ScaffoldColumn { get; private set; }
		public RangeAttribute Range { get; private set; }
		// ReSharper disable once InconsistentNaming
		public UIHintAttribute UIHint { get; private set; }
		public RegularExpressionAttribute RegularExpression { get; private set; }
	}
}