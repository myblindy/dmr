using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.tests
{
    internal static class Extensions
    {
        internal static MemoryStream ToMemoryStream(this string s) =>
            new MemoryStream(Encoding.UTF8.GetBytes(s));
    }
}
