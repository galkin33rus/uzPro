using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uzPro.Concrete
{
    public static class ContractFacade
    {
        internal static void Create(Contract item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Contracts.Add(item);
                db.SaveChanges();
            }
        }

        public static void Update(Contract item)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        internal static Contract GetContractById(int id)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Contracts.Where(x => x.id == id).FirstOrDefault();
            }
        }

        internal static Contract GetContractByProtocolId(int _protocolId)
        {
            using (uziEntities1 db = new uziEntities1())
            {
                return db.Contracts.Where(x => x.ProtocolID == _protocolId).FirstOrDefault();
            }
        }
    }
}
