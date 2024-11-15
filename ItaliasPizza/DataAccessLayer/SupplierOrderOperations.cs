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
                            IdSupplier = g.Select(x => x.s.IdSupplier).FirstOrDefault(),
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
                      IdSupplier = x.IdSupplier,
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

        public static List<OrderedSupplyDetails> GetOrderedSupplyDetailsByOrderIdentifier(Guid orderIdentifier)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return (from os in db.OrderedSupply
                        join s in db.Supply on os.IdSupply equals s.IdSupply
                        join mu in db.MeasurementUnit on os.IdMeasurementUnit equals mu.IdMeasurementUnit
                        where os.OrderIdentifier == orderIdentifier
                        select new OrderedSupplyDetails
                        {
                            IdSupply = os.IdSupply,
                            SupplyName = s.Name,
                            SupplyAmount = os.Quantity,
                            MeasurementUnit = mu.MeasurementUnit1,
                            IdMeasurementUnit = os.IdMeasurementUnit
                        })
                        .ToList();
            }
        }

        public static int DeleteSupplierOrdersAndOrderedSupplies(Guid orderIdentifier)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var orderedSupplies = db.OrderedSupply
                    .Where(os => os.OrderIdentifier == orderIdentifier)
                    .ToList();

                var supplierOrderIds = orderedSupplies
                    .Select(os => os.IdSupplierOrder)
                    .Distinct()
                    .ToList();

                db.OrderedSupply.RemoveRange(orderedSupplies);

                var supplierOrders = db.SupplierOrder
                    .Where(so => supplierOrderIds.Contains(so.IdSupplierOrder))
                    .ToList();

                db.SupplierOrder.RemoveRange(supplierOrders);

                return db.SaveChanges();
            }
        }


        public static int UpdateSupplierOrdersStatus(Guid orderIdentifier, int status)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var existingOrders = db.SupplierOrder
                    .Join(db.OrderedSupply,
                          so => so.IdSupplierOrder,
                          os => os.IdSupplierOrder,
                          (so, os) => new { SupplierOrder = so, OrderedSupply = os })
                    .Where(joined => joined.OrderedSupply.OrderIdentifier == orderIdentifier)
                    .Select(joined => joined.SupplierOrder)
                    .ToList();

                foreach (var order in existingOrders)
                {
                    if (order != null)
                    {
                        order.IdOrderStatus = status;
                    }
                }

                return db.SaveChanges();
            }
        }

        //public static list<guid> getsupplierordersidbyorderidentifier(guid orderidentifier)
        //{
        //    using (var db = new italiaspizzadbentities())
        //    {
        //        return db.orderedsupply.where(os => os.orderidentifier == orderidentifier)
        //            .select(id => id.idsupplierorder)
        //            .tolist();
        //    }
        //}
    }
}
