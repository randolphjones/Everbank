using System;

namespace Everbank.Service.Contracts
{
    public class Message
    {
        public string Text { get; set; }
        public MessageType Type { get; set; }
    }

    public enum MessageType { INFO, SUCCESS, WARN, ERROR }
}
