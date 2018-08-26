using System;
using System.Collections.Generic;

namespace Everbank.Service.Contracts
{
    public class ServiceResponse
    {
        public object ResponseObject { get; set; }
        public List<Message> Messages { get; set; }
    }
}