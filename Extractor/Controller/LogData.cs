using Extractor.Converter;
using Extractor.Db;
using Extractor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Controller
{
   public class LogData
    {
       public bool AddStudent(List<Model.Student> list)
        {
            bool result = false;
            try
            {
                using(var db =new WindowServiceEntities())
                {
                    foreach (var model in list)
                    {
                        Db.Student student=new StudentConverter().convertToEntity(model);
                        db.Students.Add(student);
                        db.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }
            return result;
        }

    }
}
