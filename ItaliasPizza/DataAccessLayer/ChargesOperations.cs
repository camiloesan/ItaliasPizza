using Database;
using System.Collections.Generic;
using System.Linq;

namespace ItaliasPizza.DataAccessLayer
{
    public class ChargesOperations
    {
        public static List<Charge> GetCharges()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Charge.ToList();
            }
        }
    }
}
