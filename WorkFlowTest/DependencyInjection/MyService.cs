using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTest.DependencyInjection
{
    public class MyService : IMyService
    {
        public void DoTheThings()
        {
            Console.WriteLine("Doing stuff...");
        }
    }
}
