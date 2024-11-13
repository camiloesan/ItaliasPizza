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
			try {
				using (var db = new ItaliasPizzaDBEntities())
				{
					db.Address.Add(address);
					return db.SaveChanges();
				}
			} catch (Exception e)
			{
				// Log the exception
				return -1;
			}
		}

		public static List<Address> GetClientAddresses(Client client) {

			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Address.Where(a => a.IdClient == client.IdClient).ToList();
			}		
		}
	}
}
