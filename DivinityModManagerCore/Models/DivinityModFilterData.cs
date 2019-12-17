﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivinityModManager.Models
{
	public struct DivinityModFilterData
	{
		public string FilterProperty { get; set; }
		public string FilterValue { get; set; }

		private static char[] separators = new char[1] { ' ' };

		public bool ValueContains(string val, bool separateWhitespace = false)
		{
			if(separateWhitespace && val.IndexOf(" ") > 1)
			{
				var vals = val.Split(separators, StringSplitOptions.RemoveEmptyEntries);
				var findVals = FilterValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);
				Trace.WriteLine($"Searching for '{String.Join("; ", findVals)}' in ({String.Join("; ", vals)}");
				return vals.Any(x => findVals.Any(x2 => CultureInfo.CurrentCulture.CompareInfo.IndexOf(x, x2, CompareOptions.IgnoreCase) >= 0));
			}
			else
			{
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(val, FilterValue, CompareOptions.IgnoreCase) >= 0;
			}
		}

		public bool PropertyContains(string val)
		{
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(FilterProperty, val, CompareOptions.IgnoreCase) >= 0;
		}

		public bool Match(DivinityModData mod)
		{
			if (String.IsNullOrWhiteSpace(FilterValue)) return true;

			if(PropertyContains("Author"))
			{
				if (ValueContains(mod.Author)) return true;
			}

			if (PropertyContains("version"))
			{
				if (ValueContains(mod.Version.Version)) return true;
			}

			if (PropertyContains("Mode"))
			{
				foreach (var mode in mod.Modes)
				{
					if (ValueContains(mode))
					{
						return true;
					}
				}
			}

			if (PropertyContains("Depend"))
			{
				foreach(var dependency in mod.Dependencies)
					{
					if (ValueContains(dependency.Name))
					{
						return true;
					}
				}
			}

			if (PropertyContains("Name"))
			{
				Trace.WriteLine($"Searching for '{FilterValue}' in '{mod.Name}' | {mod.Name.IndexOf(FilterValue)}");
				if (ValueContains(mod.Name)) return true;
			}

			if (PropertyContains("Desc"))
			{
				if (ValueContains(mod.Description)) return true;
			}

			if (PropertyContains("Type"))
			{
				if (ValueContains(mod.Type)) return true;
			}

			if (PropertyContains("UUID"))
			{
				if (ValueContains(mod.UUID)) return true;
			}

			if (PropertyContains("Selected"))
			{
				if (mod.IsSelected) return true;
			}

			if (PropertyContains("Editor"))
			{
				if (mod.IsEditorMod) return true;
			}

			if (PropertyContains("LastModified"))
			{
				DateTime date = DateTime.Now;
				if(DateTime.TryParse(FilterValue, out date))
				{
					if (mod.LastModified >= date) return true;
				}
			}

			/*
			 *	var propertyValue = (string)mod.GetType().GetProperty(FilterProperty).GetValue(mod, null);
				if(propertyValue != null)
				{
					return CultureInfo.CurrentCulture.CompareInfo.IndexOf(propertyValue, FilterValue, CompareOptions.IgnoreCase) >= 0;
				}
			*/
			return false;
		}
	}
}
