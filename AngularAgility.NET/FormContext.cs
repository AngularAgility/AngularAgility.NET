using Angular.Agility.DataAnnotationHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Angular.Agility.Validation;

namespace Angular.Agility
{
	public class FormContext
	{
		public string FormName { get; set; }

		public List<ValidationMessageData> ValidationMessageData { get; private set; }

		public FormContext()
		{
			this.ValidationMessageData = new List<ValidationMessageData>();
		}
	}
}
