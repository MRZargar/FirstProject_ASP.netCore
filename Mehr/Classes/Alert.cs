using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mehr.Classes
{
    public enum WebMessageType
    {
        Success, Danger, Warning, Info, Default, Primary
    }
    public static class WebMessage
    {
        public static string Get(string message, WebMessageType type, bool WithCloseBtn = false)
        {
            string cssclass = $"alert alert-{type.ToString().ToLower()}";
            if (WithCloseBtn)
            {
                cssclass += " alert-dismissible";
            }
            string closeBtn =
                "<button type='button' class='close' data-dismiss='alert' aria-label='بستن'><span aria-hidden='true'>&times;</span></button>";
            string messageStr = $@"<div class='{cssclass}' role='alert'>{(!WithCloseBtn ? "" : closeBtn)}{message}</div>";
            return messageStr;
        }

    }
}
