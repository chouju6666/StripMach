using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RT.Advanced
{
    class csData
    {
        public class JsonFun<T>
        { 
            public string Serialization(T DataValue)
            {
                Type t = typeof(T);
                if (t.Name.Equals("Student"))
                {
                    // do something
                }

                return null;
            }
        }

       
    }
}
