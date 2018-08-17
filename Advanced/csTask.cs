using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Advanced
{
    class csTask
    {
        delegate T MyTaskDele<T>(T a, T b);

        //public MyTaskDele<int> deleAdd = new MyTaskDele<int>();

        public event DelegateMessage mytask;

        public csTask()
        {

        }

        public void Run()
        {
            while (true)
            {
                if (mytask != null)
                    mytask("ss");
            }
        }
    }
}
