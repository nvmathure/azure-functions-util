using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace CloudNDevOps.AzureFunctionUtil
{
    /// <summary>
    /// Provides helper functions for processing Service Bus
    /// </summary>
    public static  class ServiceBusHelper
    {
        /// <summary>
        /// Deserializes body of message into Object
        /// </summary>
        /// <typeparam name="TBodyType">Type of object to use for serialization</typeparam>
        /// <param name="message">Service Bus Message to serialize</param>
        /// <param name="encodingFunction">Function pointer used for converting Byte array to string</param>
        /// <param name="settings">JSON Serialization Settings</param>
        /// <returns>Instance of TBodyType after serialization</returns>
        public static TBodyType GetBody<TBodyType>(
            this Message message, 
            Func<byte[], string> encodingFunction,
            JsonSerializerSettings settings = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (encodingFunction == null) throw new ArgumentNullException(nameof(encodingFunction));

            return JsonConvert.DeserializeObject<TBodyType>(
                encodingFunction(message.Body),
                settings);
        }

        /// <summary>
        /// Deserializes body of message into Object using UTF8 Encoder
        /// </summary>
        /// <typeparam name="TBodyType"></typeparam>
        /// <param name="message"></param>
        /// <param name="settings">JSON Serialization Settings</param>
        /// <returns></returns>
        public static TBodyType GetBody<TBodyType>(
            this Message message,
            JsonSerializerSettings settings = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            return GetBody<TBodyType>(message, Encoding.UTF8.GetString, settings);
        }

        /// <summary>
        /// Copies user properties from one message to another with matching regular expression
        /// </summary>
        /// <param name="destinationMessage">Instance of <see cref="Message"/> to which user properties needs to be copied</param>
        /// <param name="sourceMessage">Instance of <see cref="Message"/> from which user properties needs to be copied</param>
        /// <param name="propertyMatchRegex">Regular expression to filter user properties</param>
        public static void CopyUserPropertiesFrom(
            this Message destinationMessage,
            Message sourceMessage,
            Regex propertyMatchRegex = null)
        {
            if (destinationMessage == null) throw new ArgumentNullException(nameof(destinationMessage));
            if (sourceMessage == null) throw new ArgumentNullException(nameof(sourceMessage));

            foreach (var userProperty in sourceMessage.UserProperties)
            {
                if (propertyMatchRegex == null || propertyMatchRegex.IsMatch(userProperty.Key))
                    destinationMessage.UserProperties.Add(
                        new KeyValuePair<string, object>(
                            userProperty.Key, userProperty.Value));
            }
        }
    }
}
