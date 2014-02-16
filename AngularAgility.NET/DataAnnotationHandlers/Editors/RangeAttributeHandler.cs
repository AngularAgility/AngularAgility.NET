using System.ComponentModel.DataAnnotations;

namespace Angular.Agility.DataAnnotationHandlers.Editors
{
	public class RangeAttributeHandler : IHandleDataAnnotations<EditorBuilder, RangeAttribute>
	{
		public void Handle(EditorBuilder tag, RangeAttribute att, AgilityMetadata metadata)
		{
			var min = att.Minimum;
			var max = att.Maximum;
			var type = att.OperandType;
			if (type == typeof(int))
			{
				if ((int)min != int.MinValue)
					tag.Attributes.Add("min", min.ToString());
				if ((int)max != int.MaxValue)
					tag.Attributes.Add("max", max.ToString());
			}
			else if (type == typeof(double))
			{
				// ReSharper disable CompareOfFloatsByEqualityOperator
				if ((double)min != double.MaxValue)
					tag.Attributes.Add("min", min.ToString());
				if ((double)max != double.MaxValue)
					tag.Attributes.Add("max", max.ToString());
				// ReSharper restore CompareOfFloatsByEqualityOperator
			}
			else
			{
				tag.Attributes.Add("min", min.ToString());
				tag.Attributes.Add("max", max.ToString());
			}
		}
	}
}