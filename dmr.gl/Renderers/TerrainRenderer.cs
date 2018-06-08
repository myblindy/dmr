using dmr.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.gl.Renderers
{
    internal class TerrainRenderer
    {
        private Map Map;

        internal TerrainRenderer(Map map)
        {
            Map = map;
        }
    }
}
