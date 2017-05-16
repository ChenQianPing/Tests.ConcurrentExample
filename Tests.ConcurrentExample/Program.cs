using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ConcurrentExample
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMethod1.MockTest();
            TestMethod2.MockTest();

            TestMethod3.MockTest();
            TestMethod4.MockTest();
        }
    }
}
