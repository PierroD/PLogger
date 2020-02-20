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
using MySql.Data.MySqlClient;
using PLogger.Configuration;
using Newtonsoft.Json;

namespace PLogger
{
    public class Log
    {
        private static string _msg;
        private static string _functionPassThrough;
        private static string _type;
        private static int _level;
        private static Guid _UId;

        #region Message Type
        #region single parameter
        public static void Trace(string message)
        {
            _type = ">> [TRACE]";
            _msg = message;
            _level = 0;
            whichMethodToLog();
        }
        public static void Debug(string message)
        {
            _type = "?? [DEBUG]";
            _msg = message;
            _level = 1;
            whichMethodToLog();
        }
        public static void Infos(string message)
        {
            _type = "I  [INFOS]";
            _msg = message;
            _level = 2;
            whichMethodToLog();
        }
        public static void Warns(string message)
        {
            _type = "W  [WARNS]";
            _msg = message;
            _level = 3;
            whichMethodToLog();
        }
        public static void Error(string message)
        {
            _type = "!! [ERROR]";
            _msg = message;
            _level = 4;
            whichMethodToLog();
        }
        public static void Fatal(string message)
        {
            _type = "F  [FATAL]";
            _msg = message;
            _level = 5;
            whichMethodToLog();
        }
        //Internal is for internal PLogger Error
        private static string InternalError(string message)
        {
            return $"IE [INTERNAL ERROR] {Environment.UserName}  {CurrentDate()} < {CurrentTimestamp()} > " + message;
        }
        #endregion

        #region multiple parameters
        public static void Trace(object obj, string message = null)
        {
            if(obj.GetType() != typeof(String))
            {
                _type = ">> [TRACE]";
                _msg = (String.IsNullOrEmpty(message))? obj.ToString() : message + " | " + obj.ToString();
                _level = 0;
                whichMethodToLog();
            }
        }
        public static void Debug(object obj, string message = null)
        {
            if (obj.GetType() != typeof(String))
            {
                _type = "?? [DEBUG]";
                _msg = (String.IsNullOrEmpty(message)) ? obj.ToString() : message + " | " + obj.ToString();
                _level = 1;
                whichMethodToLog();
            }
        }
        public static void Infos(object obj, string message = null)
        {
            if (obj.GetType() != typeof(String))
            {
                _type = "I  [INFOS]";
                _msg = (String.IsNullOrEmpty(message)) ? obj.ToString() : message + " | " + obj.ToString();
                _level = 2;
                whichMethodToLog();
            }
        }
        public static void Warns(object obj, string message = null)
        {
            if (obj.GetType() != typeof(String))
            {
                _type = "W  [WARNS]";
                _msg = (String.IsNullOrEmpty(message)) ? obj.ToString() : message + " | " + obj.ToString();
                _level = 3;
                whichMethodToLog();
            }
        }
        public static void Error(object obj, string message = null)
        {
            if (obj.GetType() != typeof(String))
            {
                _type = "!! [ERROR]";
                _msg = (String.IsNullOrEmpty(message)) ? obj.ToString() : message + " | " + obj.ToString();
                _level = 4;
                whichMethodToLog();
            }
        }
        public static void Fatal(object obj, string message = null)
        {
            if (obj.GetType() != typeof(String))
            {
                _type = "F  [FATAL]";
                _msg = (String.IsNullOrEmpty(message)) ? obj.ToString() : message + " | " + obj.ToString();
                _level = 5;
                whichMethodToLog();
            }
        }
        private static string InternalError(object obj, string message = null)
        {
            return String.Format($"IE [INTERNAL ERROR] {Environment.UserName}  {CurrentDate()} < {CurrentTimestamp()} > {((String.IsNullOrEmpty(message)) ? obj.ToString() : message + " | " + obj.ToString())}");  
        }
        #endregion
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

        #region ActivityId
        public static void setActivityId()
        {
            _UId = Guid.NewGuid();
        }
        private static string getActivityId()
        {
            if (_UId != Guid.Empty && _UId != null)
            {
                return _UId.ToString();
            }
            else
            {
                setActivityId();
                return _UId.ToString();
            }
        }
        #endregion

        #region Create Message
        /// <summary>
        /// Create a generic message using parameters
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private static string CreateMessage(PLoggerElement target)
        {
            try
            {
                string temp_msg;
                temp_msg = String.Format($"{_type} {Environment.UserName} {CurrentDate()} < {CurrentTimestamp()} > ");

                if (target.ActivityId == true)
                    temp_msg += "{" + getActivityId() + "} ";

                if (target.DetailMode == true && !String.IsNullOrEmpty(getFunctionPassedThrough()))
                    temp_msg += string.Format($"( { getFunctionPassedThrough()} ) ");

                _msg = temp_msg + _msg; 

                
            }

            catch (Exception e)
            {
                _msg = InternalError(e, " Your App.config is malformed ");  //call a non-static function in a static function
            }
            return _msg;
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

        #region Check Message Level
        private static int CheckMessageLevel(PLoggerElement target)
        {
            if (target.MinLevel == "Trace")
                return 0;
            else if (target.MinLevel == "Debug")
                return 1;
            else if (target.MinLevel == "Infos")
                return 2;
            else if (target.MinLevel == "Warns")
                return 3;
            else if (target.MinLevel == "Error")
                return 4;
            else if (target.MinLevel == "Fatal")
                return 5;
            else 
                return -1;
        }
        #endregion

        #region Save Logs

        #region Saving Method to choose
        /// <summary>
        /// Check if .config `saveType` = "file" to save it into a file
        /// </summary>
        /// <param></param>
        private static void whichMethodToLog()
        {
            try
            {
                    foreach (PLoggerElement target in GetConfig().PLoggerInstances)
                    {
                    if (CheckMessageLevel(target) <= _level)
                    {
                        switch (target.SaveType)
                        {
                            case "mysql":
                                {
                                    string connection = String.Format($"SERVER={target.DbHost};DATABASE={target.DbName};UID={target.DbUser};PASSWORD={target.DbPassword};");
                                    writeToDatabase(new MySqlConnection(connection), target.DetailMode, target.ActivityId);
                                    break;
                                }
                            case "json":
                                {
                                    MessageJson message = new MessageJson();
                                    message.unique_id = (target.ActivityId) ? getActivityId() : null ;
                                    message.type = _type.Substring(3).Replace(" ", String.Empty);
                                    message.username = Environment.UserName;
                                    message.message = _msg;
                                    message.date = CurrentDate();
                                    message.created_at = CurrentTimestamp();
                                    message.passed_through = (target.DetailMode) ? _functionPassThrough : null;

                                    if (String.IsNullOrEmpty(target.FilePath))
                                        writeToJson(string.Format(Directory.GetCurrentDirectory() + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '-') }") + ".json", message, target);
                                    else
                                        writeToJson(string.Format(target.FilePath + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '-') }") + ".json", message, target);

                                    break;
                                }
                            case "file":
                                {
                                    if (String.IsNullOrEmpty(target.FilePath))
                                        writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '-') }") + ".log", CreateMessage(target));
                                    else
                                        writeToFile(string.Format(target.FilePath + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '-') }") + ".log", CreateMessage(target));
                                    break;
                                    //we call the function named writeToFile with those parameters { FilePath, TheMessage }
                                }
                            default:
                                {
                                    writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + target.FileName + $"_{ CurrentDate().Replace('/', '-') }") + ".log", InternalError("Verify your saveType in the app.config"));
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception e) //if target.FileName doesn't exist
            {
                writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + "ExceptionErrorPLogger" + $"_{ CurrentDate().Replace('/', '-') }") + ".log", InternalError(e));
            }
        }
        #endregion

        #region savingType

        #region file
        /// <summary>
        /// Check if the log allready exist, if it does it will write in it 
        /// else it will create it and write in it
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        private static void writeToFile(string filePath, string message)
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

        #region database

        private static void writeToDatabase(MySqlConnection connection, bool detail, bool activityId)
        {
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.CommandText = "INSERT INTO Log(activity_id, type, username, message, passed_through) VALUES(@activity_id, @type, @username, @message, @passed_through)";
                query.Parameters.AddWithValue("@activity_id", (activityId) ? getActivityId() : null);
                query.Parameters.AddWithValue("@type", _type.Substring(3).Replace(" ", String.Empty));
                query.Parameters.AddWithValue("@username", Environment.UserName);
                query.Parameters.AddWithValue("@message", _msg);
                query.Parameters.AddWithValue("@passed_through", (detail) ? _functionPassThrough : null);
                query.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                writeToFile(string.Format(Directory.GetCurrentDirectory() + "\\" + "ExceptionErrorPLogger" + $"_{ CurrentDate().Replace('/', '-') }") + ".log", InternalError(e));
            }
        }
        #endregion

        #region Json
        private static void writeToJson(string filePath, MessageJson msgJson, PLoggerElement target)
        {
            if (File.Exists(filePath))
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();
                File.WriteAllLines(filePath, lines.GetRange(0, lines.Count - 3).ToArray());
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(",");
                    sw.WriteLine(JsonConvert.SerializeObject(msgJson));
                    sw.WriteLine("]");
                    sw.WriteLine("}");
                    sw.WriteLine("}");
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("{ " + "\""+target.FileName+"\" : ");
                    sw.WriteLine("{ " + "\"Logs\" : [ ");
                    sw.WriteLine(JsonConvert.SerializeObject(msgJson));
                    sw.WriteLine("]");
                    sw.WriteLine("}");
                    sw.WriteLine("}");
                }
            }
        }
        #endregion

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
