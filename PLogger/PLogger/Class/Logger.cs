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
using PLogger.Configuration;

namespace PLogger.Class
{
    class Logger
    {
        private static string _msg;
        private static string _functionPassThrough;

        #region Message Type
        public static void Trace(string message)
        {
            CreateMessage(">> [TRACE]", message);
        }
        public static void Debug(string message)
        {
            CreateMessage("?  [DEBUG]", message);
        }
        public static void Infos(string message)
        {
            CreateMessage("I  [INFOS]", message);
        }
        public static void Warn(string message)
        {
            CreateMessage("W  [WARNS]", message);
        }
        public static void Error(string message)
        {
            CreateMessage("⚠ [ERROR]", message);
        }
        public static void Fatal(string message)
        {
            CreateMessage("F  [FATAL]", message);
        }
        //Internal is for internal PLogger Error
        private static string InternalError(string message)
        {
            return "IN [INTERNAL ERROR] " + message;
        }
        #endregion

        #region Function passed through
        /// <summary>
        /// Add to a string theClassName.theFunctionName in paramaters
        /// </summary>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        public static void setFunctionPassedThrough([CallerFilePath] string className = "", [CallerMemberName] string functionName = "", [CallerLineNumber] int lineNumber = 0)
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
            try
            {
                foreach (PLoggerElement target in GetConfig().PLoggerInstances)
                {
                    if (target.DetailMode == true && !String.IsNullOrEmpty(getFunctionPassedThrough()))
                        _msg = string.Format($"{type} {Environment.UserName} {CurrentDate()} < {CurrentTimestamp()} > ( { getFunctionPassedThrough()} ) {message}");
                    else
                        _msg = string.Format($"{type} {Environment.UserName} {CurrentDate()} < {CurrentTimestamp()} > {message}");
                    new Logger().whichMethodToLog(target, _msg);  //call a non-static function in a static function
                }
            }
            catch (Exception e)
            {
                new Logger().whichMethodToLog(null, "", " Your App.config is malformed");  //call a non-static function in a static function
            }
        }
        #region Timestamp & Date
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

        #endregion

        #region Save Logs
        /// <summary>
        /// Check if .config `saveType` = "file" to save it into a file
        /// </summary>
        /// <param name="message"></param>
        private void whichMethodToLog(PLoggerElement target, string message, string error = "")
        {
            try
            {
                switch (target.SaveType)
                {
                    case "file":
                        {
                            if (String.IsNullOrEmpty(target.FilePath))
                                writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '-') }") + ".log", message);
                            else
                                writeToFile(string.Format(target.FilePath + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '_') }") + ".log", message);
                            break;
                            //we call the function named writeToFile with those parameters { FilePath, TheMessage }
                        }
                    default:
                        {
                            writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '_') }") + ".log", InternalError("No writting log method"));
                            break;
                        }
                }

            }
            catch (Exception e) //if target.FileName doesn't exist
            {
                if (String.IsNullOrEmpty(error))
                    writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + "ExceptionErrorPLogger" + $"_{ CurrentDate().Replace('/', '_') }") + ".log", InternalError(e.ToString()));
                else
                    writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + "ExceptionErrorPLogger" + $"_{ CurrentDate().Replace('/', '_') }") + ".log", InternalError(error));

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

        #region Config
        /// <summary>
        /// Get the elements stored in the App.config
        /// </summary>
        /// <returns></returns>
        private static PLoggerConfig GetConfig()
        {
            return (PLoggerConfig)ConfigurationManager.GetSection("PLogger");
        }
        #endregion

    }
}
