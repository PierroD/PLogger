using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace TestObserver.Class
{
    class PLogger
    {
        private static string _msg;
        private static string _functionPassThrough;

        #region Message Type
        public static void Infos(string message)
        {
            CreateMessage("I  [INFOS]", message);
        }

        public static void Error(string message)
        {
            CreateMessage("⚠ [ERROR]", message);
        }

        public static void Trace(string message)
        {
            CreateMessage(">> [TRACE]", message);
        }
        public static void Debug(string message)
        {
            CreateMessage("?  [DEBUG]", message);
        }
        #endregion

        #region Function passed through
        /// <summary>
        /// Add to a string theClassName.theFunctionName in paramaters
        /// </summary>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        public static void setFunctionPassedThrough([CallerFilePath] string className = "", [CallerMemberName] string functionName ="", [CallerLineNumber] int lineNumber = 0)
        {
            if (String.IsNullOrEmpty(getFunctionPassedThrough()))
                _functionPassThrough += $"{className.Split('\\').Last()}|{functionName}|ligne.{lineNumber}";
            else
                _functionPassThrough += $" => {className.Split('\\').Last()}|{functionName}|ligne.{lineNumber}";
        }
        private static string getFunctionPassedThrough()
        {
            return _functionPassThrough;
        }
        #endregion

        #region Create Message
        /// <summary>
        /// Create a generic message using parameters
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void CreateMessage(string type, string message)
        {
            if (ConfigurationManager.AppSettings.Get("detailMode") == "true" && !String.IsNullOrEmpty(getFunctionPassedThrough()))
                _msg = string.Format($"{type} {Environment.UserName} {CurrentDate()} < {CurrentTimestamp()} > ( { getFunctionPassedThrough()} ) {message}");
            else
                _msg = string.Format($"{type} {Environment.UserName} {CurrentDate()} < {CurrentTimestamp()} > {message}");

            new PLogger().whichMethodToLog(_msg);  //call a non-static function in a static function
        }
        /// <summary>
        /// <return>Current Timestamp hour:minutes:seconds:milliseconds</return>
        /// </summary>
        /// <returns></returns>
        private static string CurrentTimestamp()
        {
            return DateTime.Now.ToString("HH:mm:ss.ffff");
        }
        /// <summary>
        /// <return>Current DateTime day/month/year</return>
        /// </summary>
        /// <returns></returns>
        private static string CurrentDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        #endregion

        #region Save Log
        /// <summary>
        /// Check if .config `saveType` = "file" to save it into a file
        /// </summary>
        /// <param name="message"></param>
        private void whichMethodToLog(string message)
        {
            if (ConfigurationManager.AppSettings.Get("saveType") == "file")
            {
                writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + ConfigurationManager.AppSettings.Get("fileName") + $"_{ CurrentDate().Replace('/', '_') }") + ".log", message);
                //we call the function named writeToFile with those parameters { FilePath, TheMessage }
            }

        }
        #region savingType
        /// <summary>
        /// Check if the log allready exist, if it does it will write in it 
        /// else it will create it and write in it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        private void writeToFile(string filePath, string message)
        {
            if (File.Exists(filePath))
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
        }
        #endregion

        #endregion

    }
}
