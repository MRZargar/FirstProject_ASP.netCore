using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace Mehr.Classes
{
    public static class Extention {
        public static void SetViewMessage(this Controller controller, string message, WebMessageType type,
                bool WithCloseBtn = true)
        {
            controller.TempData["Message"] = WebMessage.Get(message, type, WithCloseBtn);
        }
    }
}
