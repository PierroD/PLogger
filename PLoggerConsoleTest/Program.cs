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
            // Not necessary
            Log.setActivityId();
            Log.Infos("Informational Test", "which is really usefull");
            Log.setActivityId();
            Log.Infos("New activity id");
            Log.setPreviousActivityId();
            Log.Infos("SetPrevious Id");
            TestDebugFunction();
            TestErrorFunction();
          // TestReturnException();
        }

        private static void TestDebugFunction()
        {
            Log.setFunctionPassedThrough();
            Log.Debug("Debug Test", "another parameter", 1, "and another one");
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
