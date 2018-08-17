using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Advanced
{
    class csError
    {
        public event DelegateMessage myerror;

        List<emError> Errorls = new List<emError>();
        public csError()
        {
            Errorls.Add(new emError { ErrorID = 1, ErrorMessage = "Please set: wafer size, bease pressure, etc." });
            Errorls.Add(new emError { ErrorID = 2, ErrorMessage = "Hex Option changed from" });
            Errorls.Add(new emError { ErrorID = 3, ErrorMessage = "Time Format Error, TASK TERMINATED PLEASE RESET" });
            Errorls.Add(new emError { ErrorID = 71, ErrorMessage = "Overtime Task" });
            Errorls.Add(new emError { ErrorID = 75, ErrorMessage = "Unable to get End Point Calibration TASK TERMINATED PLEASE RESET" });
            Errorls.Add(new emError { ErrorID = 80, ErrorMessage = "TASK task n stack too small; RESET" });
            Errorls.Add(new emError { ErrorID = 81, ErrorMessage = "TASK task n stack too small; RESET" });
            Errorls.Add(new emError { ErrorID = 90, ErrorMessage = "Modify Flags Failed; Run Patch; TASK TERMINATED PLEASE RESET" });
            Errorls.Add(new emError { ErrorID = 91, ErrorMessage = "Attempt to remove list head" });
            Errorls.Add(new emError { ErrorID = 92, ErrorMessage = "No Operator Mode with Shuttle Down or Error Status" });
            Errorls.Add(new emError { ErrorID = 93, ErrorMessage = "Attempt to unhold task not held" });
            Errorls.Add(new emError { ErrorID = 94, ErrorMessage = "Attempt to unnohold task not nohold" });
            Errorls.Add(new emError { ErrorID = 95, ErrorMessage = "ATTACH TIMER FAILED; Reply ER R to continue OR ER A to Abort" });
            Errorls.Add(new emError { ErrorID = 96, ErrorMessage = "Attempt to nohold already hold task" });
            Errorls.Add(new emError { ErrorID = 97, ErrorMessage = "UTX free Task = 0 Main" });
            Errorls.Add(new emError { ErrorID = 98, ErrorMessage = "required Utility Tasks are Busy Try again later" });
            Errorls.Add(new emError { ErrorID = 99, ErrorMessage = "Needed Utilities are Busy, Try again later" });

            Errorls.Add(new emError { ErrorID = 100, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 101, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 102, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 103, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 104, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 111, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 112, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 113, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 114, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 115, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 116, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 117, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 118, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 119, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 126, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 127, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 128, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 129, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 130, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 131, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 132, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 133, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 134, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 135, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 147, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 148, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 149, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 151, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 160, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 161, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 165, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 166, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 167, ErrorMessage = "" });
            Errorls.Add(new emError { ErrorID = 168, ErrorMessage = "" });
        }      

        public List<emError> FindError(string key)
        {
            return Errorls.Where(r => r.ErrorMessage.Contains(key)).ToList();
        }
    }

    class emError
    {
        public int ErrorID;
        public string ErrorMessage;
        public string Explanation;
    }
}
