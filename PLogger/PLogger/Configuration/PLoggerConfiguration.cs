using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLogger.Configuration
{
    public class PLoggerConfiguration : ConfigurationSection
    {
        /// <summary>
        /// The name of this section in the app.config.
        /// </summary>
        public const string SectionName = "PLoggerConfiguration";

        private const string targetsCollectionName = "targets";

        [ConfigurationProperty(targetsCollectionName)]
        [ConfigurationCollection(typeof(targetsCollection), AddItemName = "add")]
        public targetsCollection targets { get { return (targetsCollection)base[targetsCollectionName]; } }
    }

    public class targetsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new targetsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((targetsElement)element).saveType;
        }
    }

    public class targetsElement : ConfigurationElement
    {
        [ConfigurationProperty("saveType", IsRequired = true)]
        public string saveType
        {
            get { return (string)this["saveType"]; }
            set { this["saveType"] = value; }
        }

        [ConfigurationProperty("filePath", IsRequired = true)]
        public string filePath
        {
            get { return (string)this["filePath"]; }
            set { this["filePath"] = value; }
        }

        [ConfigurationProperty("fileName", IsRequired = true)]
        public string fileName
        {
            get { return (string)this["fileName"]; }
            set { this["fileName"] = value; }
        }

        [ConfigurationProperty("detailMode", IsRequired = true)]
        public bool detailMode
        {
            get { return (bool)this["detailMode"]; }
            set { this["detailMode"] = value; }
        }
        [ConfigurationProperty("targets", IsRequired = true)]
        public targetsCollection Targets => (targetsCollection)this["targets"];
    }
}
