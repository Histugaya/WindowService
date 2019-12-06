using Extractor.Controller;
using Extractor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Extractor.FileHandling
{
   public class XML
    {
        public bool ExtractData(StreamReader rdr)
        {
            bool extracted = false;
            try
            {
                List<Student> list = new List<Student>();
                string file = rdr.ReadToEnd();
                XDocument xDoc = XDocument.Parse(file);
                if (xDoc.Root != null)
                {
                    IEnumerable<XElement> eventList = xDoc.Root.Descendants("student").ToList();
                    foreach (var elem in eventList)
                    {
                        Student model = new Student();
                        model.StudentName = elem.Element("Name").Value;
                        model.Address = elem.Element("Address").Value;
                        model.Phone = elem.Element("Phone").Value;
                        list.Add(model);
                    }
                }
                extracted = new LogData().AddStudent(list);
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }
            return extracted;
        }
    }
}
