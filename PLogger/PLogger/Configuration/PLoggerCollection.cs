using System.Configuration;

namespace PLogger.Configuration
{
    public class PLoggerCollection : ConfigurationElementCollection
    {
        // Create a property that lets us access an element in the
        // collection with the int index of the element
        /// <summary>
        /// Get : the PLoggerElement at the specified index in the collection
        /// Set : Check if a PLoggerElement exists at the specified index and delete it if it does
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PLoggerElement this[int index]
        {
            get
            {
                return (PLoggerElement)BaseGet(index);
            }
            set
            {

                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                // Add the new PLoggerElement at the specified
                // index
                BaseAdd(index, value);
            }
        }
        /// <summary>
        ///  Get : the PLoggerElement where the name matches the string key specified
        ///  Set : Checks if a PLoggerElement exists with the specified name and deletes it if it does
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new PLoggerElement this[string key]
        {
            get
            {
                return (PLoggerElement)BaseGet(key);
            }
            set
            {

                if (BaseGet(key) != null)
                    BaseRemoveAt(BaseIndexOf(BaseGet(key)));

                // Adds the new PLoggerElement
                BaseAdd(value);
            }
        }
        /// <summary>
        /// Method that must be overriden to create a new element that can be stored in the collection
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PLoggerElement();
        }
        /// <summary>
        ///  Method that must be overriden to get the key of a specified element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PLoggerElement)element).SaveType;
        }
    }
}
