using dmr.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models.Items
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ItemTemplate
    {
        public string Name;
        public string Type;
        public Stats Stats;
        public byte Level;
    }
}
