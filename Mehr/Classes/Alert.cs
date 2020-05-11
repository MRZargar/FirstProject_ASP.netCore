using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mehr.Classes
{
    public class Alert
    {
        public bool Type { get; }
        public string Message { get; }

        public Alert(bool type, string message)
        {
            Type = type;

            if (type)
            {
                Message = "<strong>Success: </strong>" + message;
            }
            else
            {
                Message = "<strong>Fail: </strong>" + message;
            }
        }
    }
}
