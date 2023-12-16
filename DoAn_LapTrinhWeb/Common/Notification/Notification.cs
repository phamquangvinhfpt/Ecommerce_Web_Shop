using System.Web;

namespace DoAn_LapTrinhWeb
{
    public class Notification
    {
        public static bool has_flash()
        {
            if (HttpContext.Current.Application["Notification"].Equals("")) return false;
            return true;
        }
        public static void setNotification1_5s(string msg1_5s, string msgType1_5s)
        {
            var tb = new ModelNotification();
            tb.msg1_5s = msg1_5s;
            tb.msgType1_5s = msgType1_5s;
            HttpContext.Current.Application["Notification"] = tb;
        }
        public static void setNotification3s(string msg3s, string msgType3s)
        {
            var tb = new ModelNotification();
            tb.msg3s = msg3s;
            tb.msgType3s = msgType3s;
            HttpContext.Current.Application["Notification"] = tb;
        }

        public static void setNotification5s(string msg5s, string msgType5s)
        {
            var tb = new ModelNotification();
            tb.msg5s = msg5s;
            tb.msgType5s = msgType5s;
            HttpContext.Current.Application["Notification"] = tb;
        }
        public static ModelNotification get_flash()
        {
            var Notifi = (ModelNotification)HttpContext.Current.Application["Notification"];
            HttpContext.Current.Application["Notification"] = "";
            return Notifi;
        }
    }
}