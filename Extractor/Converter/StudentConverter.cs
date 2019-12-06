using Extractor.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Converter
{
 public class StudentConverter
    {
        public Db.Student convertToEntity(Model.Student model)
        {
            Db.Student entity = new Db.Student();
            try
            {
                entity.Name = model.StudentName;
                entity.Address = model.Address;
                entity.Phone = model.Phone;
            }
            catch (Exception e)
            {
                Log.CatchErrors(e);
            }
            return entity;
        }
    }
}
