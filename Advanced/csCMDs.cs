using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Advanced
{
    class csCMDs
    {
        /// <summary>
        /// read analog
        /// </summary>
        public emCommand ra = new emCommand
        {
            CMD = "ra",
            Module = "Analog",
            min = 0,
            max = 31
        };
        /// <summary>
        /// set analog
        /// </summary>
        public emCommand sa = new emCommand
        {
            CMD = "sa",
            Module = "Analog",
            min = 0, 
            max = 4095
        };
    }

    class emCommand
    {
        public string CMD;
        public string Module;
        public object Parameter;
        public int min;
        public int max;
    }
}
