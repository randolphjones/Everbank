using System;
using System.Collections.Generic;
using System.Linq;
using Everbank.Service.Contracts;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Everbank.Web.Helpers
{
    public static class MessageHelper
    {
        public static readonly string MESSAGES_KEY = "Messages";
        ///<summary>
        /// Returns a css class to match a message type
        ///</summary>
        public static string MessageTypeToClassName(MessageType messageType)
        {
            switch(messageType)
            {
                case MessageType.INFO:
                    return "alert-info";
                case MessageType.SUCCESS:
                        return "alert-success";
                case MessageType.WARN:
                        return "alert-warning";
                case MessageType.ERROR:
                        return "alert-danger";
                default:
                    return "alert-info";
            }
        }

        ///<summary>
        /// Appends the message collection with those from the provided response
        ///</summary>
        public static void AppendResponseMessages(List<Message> messages, ServiceResponse response)
        {
            if (response.Messages != null && response.Messages.Count > 0)
            {
                messages.AddRange(response.Messages);
            }
        }

        public static void AppendMessagesFromSession(List<Message> messages, HttpContext httpContext)
        {
            if (httpContext.Session.Keys.Contains(MESSAGES_KEY))
            {
                string serializedMessages = httpContext.Session.GetString(MESSAGES_KEY);
                List<Message> storedMessages = JsonConvert.DeserializeObject<List<Message>>(serializedMessages);
                if (messages == null)
                {
                    messages = storedMessages;
                }
                else
                {
                    messages.AddRange(storedMessages);
                }
                httpContext.Session.Remove(MESSAGES_KEY);
            }
        }
        public static void AddMessagesToSession(List<Message> messages, HttpContext httpContext)
        {
            if (messages != null && messages.Count > 0)
            {
                string serializedMessages = JsonConvert.SerializeObject(messages);
                httpContext.Session.SetString(MESSAGES_KEY, serializedMessages);
            }
        }
    }
}