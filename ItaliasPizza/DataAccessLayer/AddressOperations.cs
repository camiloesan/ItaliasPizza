using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
	public class AddressOperations
	{
		public static int SaveAddress(Address address)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Address.Add(address);
				return db.SaveChanges();
			}
		}
	}
}
