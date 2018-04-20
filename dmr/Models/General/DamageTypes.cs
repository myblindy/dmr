

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using AttackBaseType = System.Single;

namespace dmr.Models.General
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AttackTemplate
    {
					public AttackBaseType SlashingMin, SlashingMax;
					public AttackBaseType PiercingMin, PiercingMax;
					public AttackBaseType CrushingMin, CrushingMax;
		
		public void UpdateFromConfiguration(string value)
		{
			var m = Regex.Match(value, @"^\s*(\d+)\s*(?:-\s*(\d+))?\s+(\w+)\s*$");

			switch(m.Groups[3].Value)
			{
									case "slashing":
						SlashingMin = AttackBaseType.Parse(m.Groups[1].Value);
						SlashingMax = m.Groups[2].Success ? AttackBaseType.Parse(m.Groups[2].Value)	
							: SlashingMin;
						break;
									case "piercing":
						PiercingMin = AttackBaseType.Parse(m.Groups[1].Value);
						PiercingMax = m.Groups[2].Success ? AttackBaseType.Parse(m.Groups[2].Value)	
							: PiercingMin;
						break;
									case "crushing":
						CrushingMin = AttackBaseType.Parse(m.Groups[1].Value);
						CrushingMax = m.Groups[2].Success ? AttackBaseType.Parse(m.Groups[2].Value)	
							: CrushingMin;
						break;
							}
		}
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ResistsTemplate
	{
					public AttackBaseType Slashing;
					public AttackBaseType Crushing;
		
		public void UpdateFromConfiguration(string value)
		{
			var m = Regex.Match(value, @"^\s*(\d+)\s+(\w+)\s*$");

			switch(m.Groups[2].Value)
			{
									case "slashing":
						Slashing = AttackBaseType.Parse(m.Groups[1].Value);
						break;
									case "crushing":
						Crushing = AttackBaseType.Parse(m.Groups[1].Value);
						break;
							}
		}
	}
}
