using Extractor.Controller;
using Extractor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.FileHandling
{
  public class CSV
    {
        public bool ExtractData(StreamReader rdr)
        {
            bool extracted = false;
            try
            {
                List<Student> list = new List<Student>();
              

                string Line = rdr.ReadLine();

                if (!String.IsNullOrEmpty(Line))
                {
                    string[] file = Line.Split('/');

                    if (file != null)
                    {
                        foreach (var data in file)
                        {
                            string[] details = data.Split(';');
                            {
                                Student model = new Student();
                                foreach ( var lineToWork in details  )
                                {
                                    string[] raw = lineToWork.Split(':');

                                    if (lineToWork.Contains('N'))
                                        model.StudentName = raw[1];
                                    if (lineToWork.Contains('A'))
                                        model.Address = !String.IsNullOrEmpty(raw[1]) ? raw[1] : "No Address";
                                    if (lineToWork.Contains('P'))
                                        model.Phone = !String.IsNullOrEmpty(raw[1]) ? raw[1] : "No Data";
                                }
                                list.Add(model);
                            }
                        }
                    }
                    extracted = new LogData().AddStudent(list);
                }
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }

            return extracted;
        }
    }
}
