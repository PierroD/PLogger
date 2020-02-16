using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLogger;

namespace PLoggerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Infos("Informational Test");
            TestDebugFunction();
            TestErrorFunction();
        }

        private static void TestDebugFunction()
        {
            Logger.setFunctionPassedThrough();
            Logger.Debug("Debug Test");
        }
        private static void TestErrorFunction()
        {
            Logger.setFunctionPassedThrough();
            Logger.Error("Error Test");
        }
    }
}
