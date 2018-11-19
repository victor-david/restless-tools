using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides object extension methods to serialize
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        /// Serializes the specified object to XML.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="value">The object</param>
        /// <returns>An XML string that represents the serialized object.</returns>
        public static string Serialize<T>(this T value)
        {
            try
            {
                if (value != null)
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    using (var stringWriter = new StringWriter())
                    {
                        using (var writer = XmlWriter.Create(stringWriter))
                        {
                            xmlSerializer.Serialize(writer, value);
                            return stringWriter.ToString();
                        }
                    }
                }
            }
            catch
            {
            }

            return String.Empty;
        }

        /// <summary>
        /// Deserializes the specified XML to its corresponding object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="value">The XML string</param>
        /// <returns>The object of type T.</returns>
        public static T Deserialize<T>(this string value) where T : new()
        {
            try
            {
                if (!String.IsNullOrEmpty(value))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    using (var stringReader = new StringReader(value))
                    {
                        return (T)xmlSerializer.Deserialize(stringReader);
                    }
                }
            }
            catch
            {
            }

            return new T();
        }
    }
}
