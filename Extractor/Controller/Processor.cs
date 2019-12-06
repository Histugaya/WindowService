using Extractor.FileHandling;
using Extractor.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Extractor.Controller
{
    public class Processor
    {
        string _sourcePath, _destinationPath, _backupPath;
        public Processor()
        {
            _sourcePath = ConfigurationManager.AppSettings["SourcePath"];
            _destinationPath = ConfigurationManager.AppSettings["DestinationPath"];
            _backupPath = ConfigurationManager.AppSettings["BackupPath"];
        }
        public void ExtractFiles()
        {
            try
            {
                string[] files;
                files = Directory.GetFiles(_sourcePath, "*.*", SearchOption.AllDirectories);
                foreach (string sourceFile in files)
                {
                   bool extracted = CheckExtensionOfFileAndExecute(sourceFile);
                    if (extracted)
                        MoveFile(sourceFile);
                }
            }
            catch(Exception e)
            {
                Log.CatchErrors(e);
            }
        }

        private void MoveFile(string sourceFile)
        {
            try
            {
                string fileName = Path.GetFileName(sourceFile);
                string fileExtension = Path.GetExtension(sourceFile);
                string destinationFile = _destinationPath + fileName;
                string backupFile = _backupPath + fileName;
                if (File.Exists(sourceFile))
                {
                    File.Move(sourceFile, destinationFile);
                    File.Move(sourceFile, backupFile);
                    File.Delete(sourceFile);
                }
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }

        }

        private bool CheckExtensionOfFileAndExecute(string sourceFile)
        {
            bool extracted = false;
            try
            {
                string fileName = Path.GetFileName(sourceFile);
                string fileExtension = Path.GetExtension(sourceFile);
                fileExtension = fileExtension.ToUpper();

                using (FileStream fs = new FileStream(sourceFile, FileMode.Open))
                {
                    StreamReader rdr = new StreamReader(fs, System.Text.Encoding.GetEncoding(1252));
                    switch (fileExtension)
                    {
                        case ".XML":
                            extracted = new XML().ExtractData(rdr);
                            break;

                        case ".CSV":
                            extracted = new CSV().ExtractData(rdr);
                            break;
                        default: break;
                    }
                    fs.Close();
                    fs.Dispose();
                }
             
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }

            return extracted;
       }

        public void SendNotification(string message)
        {
            try
            {
                TimeZone localtime = TimeZone.CurrentTimeZone;
                NetworkCredential cred = new NetworkCredential(ConfigurationManager.AppSettings["SMTPLOGIN"],
                    ConfigurationManager.AppSettings["SMTPPASSWORD"]);
                MailMessage msg = new MailMessage();
                msg.To.Add(ConfigurationManager.AppSettings["EMAILTO"]);
                //MailAddress cc1 = new MailAddress(ConfigurationManager.AppSettings["CcEmail"]);
                //msg.CC.Add(cc1);
                msg.Subject = message; // Environment.MachineName;
                msg.Body = "Services stopped at " + DateTime.Now + "." + Environment.NewLine +
                            "TimeZone: " + localtime.StandardName + Environment.NewLine +
                            "Computer Name: " + Environment.MachineName + Environment.NewLine + Environment.NewLine +
                            "Please do not reply to this email. Thanks";
                msg.From = new MailAddress(ConfigurationManager.AppSettings["SMTPLOGIN"], ConfigurationManager.AppSettings["NAME"]);
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTP"],
                    int.Parse(ConfigurationManager.AppSettings["SMTPPORT"]));
                client.Credentials = cred;
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }


        }
    }
}
