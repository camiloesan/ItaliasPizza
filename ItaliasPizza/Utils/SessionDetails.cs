using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliasPizza.Utils
{
    public sealed class SessionDetails
    {
        private SessionDetails()
        {
        }

        public static int UserId { get; set; }
        public static Guid IdEmployee { get; set; }
        public static String UserType { get; set; }

        public static void CleanSessionDetails()
        {
            UserId = 0;
            IdEmployee = Guid.Empty;
            UserType = "";
        }
    }
}
