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
    public class UbicacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ubicaciones
        public ActionResult Index()
        {
            return View(db.Ubicaciones.ToList());
        }


        // GET: Ubicaciones/Create
        public ActionResult CreateOrEdit(int id = 0)
        {
            if(id == 0)
            {
                return View(new Ubicaciones());
            }
            else
            {
                return View(db.Ubicaciones.Find(id));
            }
            
        }

        // POST: Ubicaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "IdUbicacion,Descripcion")] Ubicaciones ubicaciones)
        {
            if (ModelState.IsValid)
            {
                if(ubicaciones.IdUbicacion == 0)
                {
                    db.Ubicaciones.Add(ubicaciones);
                }
                else
                {
                    db.Entry(ubicaciones).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ubicaciones);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubicaciones ubicaciones = db.Ubicaciones.Find(id);
            if (ubicaciones == null)
            {
                return HttpNotFound();
            }
            return View(ubicaciones);
        }

        // POST: Ubicaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Ubicaciones ubicaciones = db.Ubicaciones.Find(id);
                db.Ubicaciones.Remove(ubicaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Error al eliminar, es posible que este registro tenga datos dependientes en otras tablas";
                return RedirectToAction("Index");
            }
            
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
