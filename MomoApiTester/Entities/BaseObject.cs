using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoApiTester.Entities
{
    public class BaseObject
    {
        public string Uuid { get; set; }
        public string Token { get; set; }
        public string SubscriptionKey { get; set; }
    }
}
