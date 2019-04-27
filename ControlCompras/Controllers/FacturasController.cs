using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControlCompras.Models;
using ControlCompras.ViewModels;

namespace ControlCompras.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class FacturasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Facturas
        [Authorize(Roles = "Administrador,Básico")]
        public ActionResult Index()
        {
            var facturas = db.Facturas.Include(f => f.Ubicacion);
            return View(facturas.ToList());
        }

        // GET: Facturas/Details/5
        [Authorize(Roles = "Administrador,Básico")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

        public ActionResult ComprarProducto()
        {
            var facturaView = new FacturasView();
            facturaView.Productos = new List<OrdenProductoView>();
            Session["orderView"] = facturaView;
            return View(facturaView);
        }

        [HttpPost]
        public ActionResult ComprarProducto(FacturasView orderView)
        {
            orderView = Session["orderView"] as FacturasView;

            if (orderView.Productos.Count == 0)
            {
                ViewBag.Error = "Debe ingresar detalle";
                return View(orderView);
            }
            var ubicacionId = db.Ubicaciones.ToList().Select(o => o.IdUbicacion).Max();

            int orderID = 0;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Facturas
                    {
                        NumeroFactura = "000",
                        FechaCompra = DateTime.Now,
                        FechaRegistro = DateTime.Now,
                        UbicacionId = ubicacionId
                    };

                    db.Facturas.Add(order);
                    db.SaveChanges();


                    orderID = db.Facturas.ToList().Select(o => o.IdFactura).Max();

                    foreach (var item in orderView.Productos)
                    {
                        var orderDetail = new DetalleFactura
                        {
                            ProductoId = item.IdProducto,
                            Total = item.Valor,
                            PrecioCompra = item.Precio,
                            FacturaId = orderID,
                            Cantidad = Convert.ToInt32(item.Cantidad)
                        };
                        db.DetalleFacturas.Add(orderDetail);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }


            return RedirectToAction("Index");
        }

        public ActionResult AddProducto()
        {
            var list = db.Productos.ToList();
            list.Add(new OrdenProductoView { IdProducto = 0, Nombre = "[Seleccione un producto...]" });
            list = list.OrderBy(p => p.Nombre).ToList();
            ViewBag.IdProducto = new SelectList(list, "IdProducto", "Nombre");

            return View();
        }

        [HttpPost]
        public ActionResult AddProducto(OrdenProductoView productOrder)
        {
            var orderView = Session["orderView"] as FacturasView;

            var productID = int.Parse(Request["IdProducto"]);

            if (productID == 0)
            {
                var list = db.Productos.ToList();
                list.Add(new OrdenProductoView { IdProducto = 0, Nombre = "[Seleccione un producto...]" });
                list = list.OrderBy(p => p.Nombre).ToList();
                ViewBag.ProductID = new SelectList(list, "IdProducto", "Nombre");
                ViewBag.Error = "Debe seleccionar un producto";
                return View(productOrder);
            }

            var product = db.Productos.Find(productID);

            if (product == null)
            {
                var list = db.Productos.ToList();
                list.Add(new OrdenProductoView { IdProducto = 0, Nombre = "[Seleccione un producto...]" });
                list = list.OrderBy(p => p.Nombre).ToList();
                ViewBag.ProductID = new SelectList(list, "IdProducto", "Nombre");
                ViewBag.Error = "El producto no existe";
                return View(productOrder);
            }

            productOrder = orderView.Productos.Find(p => p.IdProducto == productID);

            if (productOrder == null)
            {
                productOrder = new OrdenProductoView
                {
                    Nombre = product.Nombre,
                    Precio = product.Precio,
                    IdProducto = product.IdProducto,
                    Cantidad = float.Parse(Request["Cantidad"])
                };
                orderView.Productos.Add(productOrder);
            }
            else
            {
                productOrder.Cantidad += float.Parse(Request["Cantidad"]);
            }

            return View("ComprarProducto", orderView);

        }

        // GET: Facturas/Create
        public ActionResult Create()
        {
            ViewBag.UbicacionId = new SelectList(db.Ubicaciones, "IdUbicacion", "Descripcion");
            return View();
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFactura,NumeroFactura,Total,FechaCompra,FechaRegistro,UbicacionId")] Facturas facturas)
        {
            if (ModelState.IsValid)
            {
                db.Facturas.Add(facturas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UbicacionId = new SelectList(db.Ubicaciones, "IdUbicacion", "Descripcion", facturas.UbicacionId);
            return View(facturas);
        }

        // GET: Facturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            ViewBag.UbicacionId = new SelectList(db.Ubicaciones, "IdUbicacion", "Descripcion", facturas.UbicacionId);
            return View(facturas);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFactura,NumeroFactura,Total,FechaCompra,FechaRegistro,UbicacionId")] Facturas facturas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facturas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UbicacionId = new SelectList(db.Ubicaciones, "IdUbicacion", "Descripcion", facturas.UbicacionId);
            return View(facturas);
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facturas facturas = db.Facturas.Find(id);
            db.Facturas.Remove(facturas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
