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
    public static class ContractValuesFacade
    {
        public static DataTable GetValuesByContractId(int contractId)
        {
            DataTable dataTable = new DataTable();

            using (uziEntities1 db = new uziEntities1())
            {
                var query = db.ContractValues.Where(x => x.ContractID == contractId).Select(p => new { p.Kod, p.Name, p.CostOne, p.Count, p.CostAll, p.Sale, p.Total }).AsEnumerable();
                dataTable = DateTableConverter.ToDataTable(query);
            }

            return dataTable;
        }


        internal static void Create(ContractValue item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.ContractValues.Add(item);
                db.SaveChanges();
            }
        }

        public static void Update(ContractValue item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static ContractValue GetContractValuesById(int id)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.ContractValues.Where(x => x.id == id).FirstOrDefault();
            }
        }

        public static void DeleteContractValuesByContractId(int id) {
            using (uziEntities1 db = new uziEntities1())
            {
                List<ContractValue> list = db.ContractValues.Where(x => x.ContractID == id).ToList();
                foreach(ContractValue item in list){
                    db.ContractValues.Remove(item);
                }
               
                db.SaveChanges();
            }
        }

    }
}
