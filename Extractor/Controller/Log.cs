using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Extractor.Controller
{
   public class Log
    {
        public static void CatchErrors(Exception ex)
        {
           string logPath = ConfigurationManager.AppSettings["LogPath"];
            var st = new StackTrace(ex, true);
            //Get the first Stack frame
            var frame = st.GetFrame(0);

            //Get the file Name
            string fileName = frame.GetFileName();
            if (fileName == null)
            {
                fileName = "No Error file found. Look in Stack";
            }

            //Get the Method Name
            string methodName = frame.GetMethod().Name;

            //Get the line number from Stackframe
            string line = frame.GetFileLineNumber().ToString();
            if (line == "0")
            {
                line = "Line number doesnot exists";
            }

            //Get stacklist for multiple stack
            string stacklist = string.Empty;
            for (int i = 0; i < st.FrameCount; i++)
            {
                stacklist += st.GetFrame(i).GetMethod().ToString() + "\r\n";
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("------------------------------------------------------------------------------\r\n")
                .AppendLine(DateTime.Now.ToString())
                .AppendLine()
                .AppendLine()
                .AppendFormat("Source:\t{0}", ex.Source)
                .AppendLine()
                .AppendFormat("Message:\t{0}", ex.Message)
                .AppendLine()
                .AppendFormat("Error on File:\t{0}", fileName)
                .AppendLine()
                .AppendFormat("Method Name:\t{0}", methodName)
                .AppendLine()
                .AppendFormat("Line Number:\t{0}", line)
                .AppendLine()
                .AppendFormat("Stack:\t{0}", stacklist)
                .AppendLine();
            if (ex.InnerException != null)
            {
                builder.AppendFormat("Inner Exception:\t{0}", ex.InnerException.Message)
                .AppendLine();
            }
            string oldContent = String.Empty;

            string filePath = logPath + "ErrorLog.txt"; 

            if (File.Exists(filePath))
            {
                oldContent = File.ReadAllText(filePath);
                File.WriteAllText(filePath, builder + oldContent);
                long length = new FileInfo(filePath).Length;
                //If file size is greater than 1 MB delete log file
                if (length > 1048576)
                {
                    File.Delete(filePath);
                }
            }
            else
            {
                File.WriteAllText(filePath, builder + "");
            }
        }
    }
}
