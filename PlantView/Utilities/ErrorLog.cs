using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utilities
{
    public static class ErrorLog
    {
        public static void Log(Exception exception)
        {
            try
            {
                using (var writer = new StreamWriter("errors.txt", true))
                {
                    writer.WriteLine(exception.Message + (exception.InnerException != null ? exception.InnerException.Message : string.Empty));
                } 
            }
            catch
            {
            }
        }

        public static void Log(string message)
        {
            try
            {
                using (var writer = new StreamWriter("errors.txt", true))
                {
                    writer.WriteLine(message);
                }
            }
            catch
            {
            }
        }
    }
}
