using Database;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class SupplierOrderOperations
    {
        public static int SaveSupplierOrder(SupplierOrder supplierOrder)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.SupplierOrder.Add(supplierOrder);
                return db.SaveChanges();
            }
        }

        public static int SaveOrderedSupply(List<OrderedSupply> orderedSupplies)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                foreach (var orderedSupply in orderedSupplies)
                {
                    db.OrderedSupply.Add(orderedSupply);
                }
                return db.SaveChanges();
            }
        }

        public static List<SupplierOrderDetails> GetSupplierOrderDetails()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return (from os in db.OrderedSupply
                        join so in db.SupplierOrder on os.IdSupplierOrder equals so.IdSupplierOrder
                        join s in db.Supplier on so.IdSupplier equals s.IdSupplier
                        join sup in db.Supply on os.IdSupply equals sup.IdSupply
                        join osStatus in db.OrderStatus on so.IdOrderStatus equals osStatus.IdOrderStatus
                        group new { os, so, s, sup, osStatus } by os.OrderIdentifier into g
                        select new
                        {
                            OrderIdentifier = g.Key,
                            SupplierName = g.Select(x => x.s.Name).FirstOrDefault(),
                            SuppliesList = g.Select(x => x.sup.Name).Distinct(),
                            OrderDate = g.Select(x => x.so.OrderDate).FirstOrDefault(),
                            ExpectedDate = g.Select(x => x.so.ExpectedDate).FirstOrDefault(),
                            ArrivalDate = g.Select(x => x.so.ArrivalDate).FirstOrDefault(),
                            IdOrderStatus = g.Select(x => x.so.IdOrderStatus).FirstOrDefault(),
                            Status = g.Select(x => x.osStatus.Status).FirstOrDefault()
                        })
                  .AsEnumerable()
                  .Select(x => new SupplierOrderDetails
                  {
                      OrderIdentifier = x.OrderIdentifier,
                      SupplierName = x.SupplierName,
                      Supplies = string.Join(", ", x.SuppliesList),
                      OrderDate = x.OrderDate.ToString("dd-MM-yyyy"),
                      ExpectedDate = x.ExpectedDate.ToString("dd-MM-yyyy"),
                      ArrivalDate = x.IdOrderStatus == 2
                          ? x.ArrivalDate.ToString("dd-MM-yyyy")
                          : "No ha sido entregado",
                      Status = x.Status
                  })
                  .ToList();
            }
        }
    }
}
