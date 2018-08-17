using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Interface
{
    enum FunctionCode : int
    {
        /// <summary>
        /// Immediatelly abort process of processing chamber, include close all vaccum and gas and power, shuttle.
        /// </summary>
        ABORT = 1, 
        /// <summary>
        /// the system will complete the recipe for the wafer being processed and stop at end of processing with 
        /// the wafer in chamber, onyl the current step of any mechanical motion, which is in progress, will be complete.
        /// </summary>
        HOLD = 2, 
        /// <summary>
        /// 
        /// </summary>
        CONT = 3, 
        /// <summary>
        /// 
        /// </summary>
        STOP = 4,

    }
}
