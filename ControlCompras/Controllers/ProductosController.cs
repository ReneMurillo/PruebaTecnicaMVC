using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControlCompras.Models;

namespace ControlCompras.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Productos
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categoria).Include(p => p.Marca).Include(p => p.Proveedor);
            return View(productos.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: Productos/Create
        public ActionResult CreateOrEdit(int id = 0)
        {
            if(id == 0)
            {
                ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre");
                ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Descripcion");
                ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "RazonSocial");
                return View(new Productos());
            }
            else
            {
                var producto = db.Productos.Find(id);
                ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre", producto.CategoriaId);
                ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Descripcion", producto.MarcaId);
                ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "RazonSocial", producto.ProveedorId);
                return View(producto);
            }
            
        }

        // POST: Productos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "IdProducto,BarCode,Nombre,Stock,Precio,Descripcion,FechaRegistro,ProveedorId,CategoriaId,MarcaId")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                if(productos.IdProducto == 0)
                {
                    productos.FechaRegistro = DateTime.Now;
                    db.Productos.Add(productos);
                }
                else
                {
                    db.Entry(productos).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre", productos.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Descripcion", productos.MarcaId);
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "RazonSocial", productos.ProveedorId);
            return View(productos);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
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
