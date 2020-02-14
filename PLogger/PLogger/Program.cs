using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLogger.Class;



namespace PLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Infos("Info Test");
            test();
            void test()
            {
                Logger.setFunctionPassedThrough();
                Logger.Error("Error Test");
            }
        }
    }
}
