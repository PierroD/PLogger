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
        [ConfigurationProperty("saveType", IsKey = true, IsRequired = true)]
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
        [ConfigurationProperty("fileName", DefaultValue = "PLogger", IsKey = true, IsRequired = true)]
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
        [ConfigurationProperty("filePath", IsKey = true, IsRequired = true)]
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
        [ConfigurationProperty("detailMode", IsKey = true, IsRequired = true)]
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
    }
}
