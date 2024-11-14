using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
	public class ClientOperations
	{
		public static bool IsPhoneRegistered(string phone)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Client.Any(c => c.Phone == phone);
			}
		}

		public static int SaveClient(Client client, Address address)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(client);
				db.Address.Add(address);
				return db.SaveChanges();
			}
		}

		public static List<Client> GetFiftyClients()
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Client.OrderBy(c => c.LastName).Take(50).ToList();
			}
		}

		public static List<Client> GetClients()
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Client.ToList();
			}
		}

		public static Client GetClientByDeliveryOrder(DeliveryOrder deliveryOrder)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Client.FirstOrDefault(c => c.IdClient == deliveryOrder.IdClient);
			}
		}

		public static Client GetClientByPhoneNumber(string phoneNumber)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Client.FirstOrDefault(c => c.Phone == phoneNumber);
			}
		}
	}
}
