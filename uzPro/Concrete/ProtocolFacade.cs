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
    public static class ProtocolFacade
    {
        public static object GetProtocolByClietnId(int clientId)
        {
            DataTable dataTable = new DataTable();

            using (uziEntities1 db = new uziEntities1())
            {
                var query = db.Protocols.Where(x => x.ClientId == clientId).OrderByDescending(x => x.Date).Select(p => new { p.Id, p.Date, p.Title }).AsEnumerable();
                dataTable = DateTableConverter.ToDataTable(query);
            }

            return dataTable;
        }

        internal static void Create(Protocol item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Protocols.Add(item);
                db.SaveChanges();
            }
        }

        public static void Update(Protocol item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        internal static Protocol GetProtocolById(int id)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Protocols.Where(x => x.Id == id).FirstOrDefault();
            }
        }
    }
}
