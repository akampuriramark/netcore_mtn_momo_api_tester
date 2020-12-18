using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoApiTester.Entities
{
    public class Response
    {
        public string StatusCode { get; set; }
        public bool? IsSuccessStatusCode { get; set; }
        public string Content { get; set; }
        public string ReasonPhrase { get; set; }
        public string Request { get; set; }
    }
}
