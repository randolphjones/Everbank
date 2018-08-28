using System;
using System.Collections.Generic;
using System.Linq;
using Everbank.Service.Contracts;

namespace Everbank.Web.Helpers
{
    public static class MessageHelper
    {
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
    }
}