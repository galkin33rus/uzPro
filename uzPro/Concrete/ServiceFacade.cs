using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using uzPro.Utils;

namespace uzPro.Concrete
{
    public static class ServiceFacade
    {
        public static object GetServices()
        {

            DataTable dataTable = new DataTable();
                       
            using (uziEntities1 db = new uziEntities1())
            {                
                var query = db.Services.OrderBy(x => x.Id).Select(p => new { p.Id, p.Kod, p.Name, p.Price }).AsEnumerable();
                dataTable = DateTableConverter.ToDataTable(query);
            }

            return dataTable;
        }


        internal static Service GetServiceById(int serviceId)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Services.Where(x => x.Id == serviceId).FirstOrDefault();
            }
        }

        public static void Create(Service item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Services.Add(item);
                db.SaveChanges();
            }
        }

        public static void Update(Service item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void Delete(int serviceId)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                Service service =  db.Services.Where(x => x.Id == serviceId).FirstOrDefault();
                db.Services.Remove(service);
                db.SaveChanges();
            }
        }
    }
}
