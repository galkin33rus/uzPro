using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uzPro.Utils;

namespace uzPro.Concrete
{
    public static class TemplateFacade
    {
        public static object GetTemplates()
        {

            DataTable dataTable = new DataTable();

            using (uziEntities1 db = new uziEntities1())
            {
                
                var query = db.Templates.OrderBy(x => x.Title).Select(p => new { Id = p.Id, Title =  p.Title }).AsEnumerable();

                dataTable = DateTableConverter.ToDataTable(query);

                dataTable.Columns.Add("RowNumber", typeof(int)).SetOrdinal(1);
                int i = 0;
                foreach (DataRow row in dataTable.Rows)
                    row["RowNumber"] = ++i;
            }

            return dataTable;
        }


        internal static string GetTextById(int id)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Templates.Where(x => x.Id == id).Select(x=> x.Text).FirstOrDefault();
            }
        }

        internal static Template GetTemplateById(int templateId)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Templates.Where(x => x.Id == templateId).FirstOrDefault();
            }
        }

        public static void Create(Template item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Templates.Add(item);
                db.SaveChanges();
            }
        }

        public static void Update(Template item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void Delete(int entityId)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                Template item = db.Templates.Where(x => x.Id == entityId).FirstOrDefault();
                db.Templates.Remove(item);
                db.SaveChanges();
            }
        }
    }
}
