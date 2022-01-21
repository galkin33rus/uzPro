using System;
using System.Collections;
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
    public static class ClientFacade
    {
        public static object GetClients()
        {

            DataTable dataTable = new DataTable();
            
            using (uziEntities1 db = new uziEntities1())
            {
                var query = db.Clients.OrderBy(x => x.Name).Take(100).Select(p => new { p.Id, p.Name, p.BirthDate, p.Tel }).AsEnumerable();
                dataTable = DateTableConverter.ToDataTable(query);
            }

            return dataTable;
        }


        public static object GetClientsByName(string name)
        {

            DataTable dataTable = new DataTable();
            
            using (uziEntities1 db = new uziEntities1())
            {
                var query = db.Clients.Where(x=> x.Name.StartsWith(name)).OrderBy(x => x.Name).Take(100).Select(p => new { p.Id, p.Name, p.BirthDate, p.Tel }).AsEnumerable();
                dataTable = DateTableConverter.ToDataTable(query);
            }

            return dataTable;
        }

        public static void Create(Client item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Clients.Add(item);
                db.SaveChanges();
            }
        }

        public static void Update(Client item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        internal static Client GetClientById(int clietnId)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Clients.Where(x => x.Id == clietnId).FirstOrDefault();
            }
        }
    }
}
