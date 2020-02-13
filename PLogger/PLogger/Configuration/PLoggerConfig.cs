using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PLogger.Configuration
{
    public class PLoggerConfig : ConfigurationSection
    {
        // Create a property that lets us access the collection of PLoggerElement
        // name of the element used for the property (refer to <targets> in the App.config)
        [ConfigurationProperty("targets")]
        // type of element find in PLoggerCollection
        [ConfigurationCollection(typeof(PLoggerCollection))]
        public PLoggerCollection PLoggerInstances
        {
            get
            {
                // get the collection and parse it
                return (PLoggerCollection)this["targets"];
            }
        }
    }
}
