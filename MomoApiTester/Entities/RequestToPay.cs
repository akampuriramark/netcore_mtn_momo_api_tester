using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoApiTester.Entities
{
    public class RequestToPay : BaseObject
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string ExternalId { get; set; }
        public string PayerMessage { get; set; }
        public string PayeeNote { get; set; }
        public string MSISDN { get; set; }

    }
}
