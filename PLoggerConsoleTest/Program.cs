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
            Log.Infos("Informational Test");
            TestDebugFunction();
            TestErrorFunction();
            TestReturnException();
        }

        private static void TestDebugFunction()
        {
            Log.setFunctionPassedThrough();
            Log.Debug("Debug Test");
        }
        private static void TestErrorFunction()
        {
            Log.setFunctionPassedThrough();
            Log.Error("Error Test");
        }
        private static void TestReturnException()
        {
            try
            {
                int y = 0;
                int x = 100 / y;
            }
            catch(Exception e)
            {
                Log.Error(e, " Division by zero : ");
            }
        }
    }
}
