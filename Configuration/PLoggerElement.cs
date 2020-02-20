using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PLogger.Configuration
{
    public class PLoggerElement : ConfigurationElement
    {
        /// <summary>
        /// Get/Set saveType value in the app.config
        /// </summary>
        [ConfigurationProperty("saveType", IsKey = true, IsRequired = false)]
        public string SaveType
        {
            get
            {
                return (string)base["saveType"];
            }
            set
            {
                base["saveType"] = value;
            }
        }
        /// <summary>
        /// Get/Set fileName value in the app.config
        /// </summary>
        [ConfigurationProperty("fileName", DefaultValue = "PLogger", IsKey = true, IsRequired = false)]
        public string FileName
        {
            get
            {
                return (string)this["fileName"];
            }
            set
            {
                this["fileName"] = value;
            }
        }
        /// <summary>
        /// Get/Set filePath value in the app.config
        /// </summary>
        [ConfigurationProperty("filePath", IsKey = true, IsRequired = false)]
        public string FilePath
        {
            get
            {
                return (string)base["filePath"];
            }
            set
            {
                base["filePath"] = value;
            }
        }
        /// <summary>
        /// Get/Set detailMode value in the app.config
        /// </summary>
        [ConfigurationProperty("detailMode", IsKey = true, IsRequired = false)]
        public bool DetailMode
        {
            get
            {
                return (bool)base["detailMode"];
            }
            set
            {
                base["detailMode"] = value;
            }
        }
        /// <summary>
        /// Get/Set dbHost value in the app.config
        /// </summary>
        [ConfigurationProperty("dbHost", IsKey = true, IsRequired = false)]
        public string DbHost
        {
            get
            {
                return (string)base["dbHost"];
            }
            set
            {
                base["dbHost"] = value;
            }
        }
        /// <summary>
        /// Get/Set dbName value in the app.config
        /// </summary>
        [ConfigurationProperty("dbName", IsKey = true, IsRequired = false)]
        public string DbName
        {
            get
            {
                return (string)base["dbName"];
            }
            set
            {
                base["dbName"] = value;
            }
        }
        /// <summary>
        /// Get/Set dbHost value in the app.config
        /// </summary>
        [ConfigurationProperty("dbUser", IsKey = true, IsRequired = false)]
        public string DbUser
        {
            get
            {
                return (string)base["dbUser"];
            }
            set
            {
                base["dbUser"] = value;
            }
        }
        /// <summary>
        /// Get/Set dbHost value in the app.config
        /// </summary>
        [ConfigurationProperty("dbPassword", IsKey = true, IsRequired = false)]
        public string DbPassword
        {
            get
            {
                return (string)base["dbPassword"];
            }
            set
            {
                base["dbPassword"] = value;
            }
        }
        /// <summary>
        /// Get/Set minLevel value in the app.config
        /// </summary>
        [ConfigurationProperty("minLevel", IsKey = true, IsRequired = false)]
        public string MinLevel
        {
            get
            {
                return (string)base["minLevel"];
            }
            set
            {
                base["minLevel"] = value;
            }
        }
        /// <summary>
        /// Get/Set detailMode value in the app.config
        /// </summary>
        [ConfigurationProperty("activityId", IsKey = true, IsRequired = false)]
        public bool ActivityId
        {
            get
            {
                return (bool)base["activityId"];
            }
            set
            {
                base["activityId"] = value;
            }
        }
    }
}
