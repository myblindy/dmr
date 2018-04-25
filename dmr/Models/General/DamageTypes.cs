

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using BaseType = System.SByte;

namespace dmr.Models.General
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AttackTemplate
    {
					public BaseType Slashing;
					public BaseType Piercing;
					public BaseType Crushing;
					public BaseType Fire;
					public BaseType Shadow;
		
		public void UpdateFromConfiguration(string value)
		{
			var m = Regex.Match(value, @"^\s*(-?\s*\d+)\s+(\w+)\s*$");

			switch(m.Groups[2].Value)
			{
									case "slashing":
						Slashing = BaseType.Parse(m.Groups[1].Value);
						break;
									case "piercing":
						Piercing = BaseType.Parse(m.Groups[1].Value);
						break;
									case "crushing":
						Crushing = BaseType.Parse(m.Groups[1].Value);
						break;
									case "fire":
						Fire = BaseType.Parse(m.Groups[1].Value);
						break;
									case "shadow":
						Shadow = BaseType.Parse(m.Groups[1].Value);
						break;
								default: throw new InvalidOperationException();
			}
		}
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ResistsTemplate
	{
					public BaseType Slashing;
					public BaseType Crushing;
					public BaseType Fire;
					public BaseType Shadow;
		
		public void UpdateFromConfiguration(string value)
		{
			var m = Regex.Match(value, @"^\s*(-?\s*\d+)\s+(\w+)\s*$");

			switch(m.Groups[2].Value)
			{
									case "slashing":
						Slashing = BaseType.Parse(m.Groups[1].Value);
						break;
									case "crushing":
						Crushing = BaseType.Parse(m.Groups[1].Value);
						break;
									case "fire":
						Fire = BaseType.Parse(m.Groups[1].Value);
						break;
									case "shadow":
						Shadow = BaseType.Parse(m.Groups[1].Value);
						break;
								default: throw new InvalidOperationException();
			}
		}
	}
}
