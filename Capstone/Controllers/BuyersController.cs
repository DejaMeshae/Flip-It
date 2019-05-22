using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class BuyersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Buyers
        public ActionResult Index()
        {
            var buyers = db.Buyers.Include(b => b.ApplicationUser);
            return View(buyers.ToList());
        }

        // GET: Buyers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buyers buyers = db.Buyers.Find(id);
            if (buyers == null)
            {
                return HttpNotFound();
            }
            return View(buyers);
        }

        // GET: Buyers/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Buyers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BuyersId,Firstname,Lastname,Address,City,State,ZipCode,UserPhoto,Lat,Lng,ApplicationUserId")] Buyers buyers)
        {
            if (ModelState.IsValid)
            {
                db.Buyers.Add(buyers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", buyers.ApplicationUserId);
            return View(buyers);
        }

        // GET: Buyers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buyers buyers = db.Buyers.Find(id);
            if (buyers == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", buyers.ApplicationUserId);
            return View(buyers);
        }

        // POST: Buyers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BuyersId,Firstname,Lastname,Address,City,State,ZipCode,UserPhoto,Lat,Lng,ApplicationUserId")] Buyers buyers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buyers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", buyers.ApplicationUserId);
            return View(buyers);
        }

        // GET: Buyers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buyers buyers = db.Buyers.Find(id);
            if (buyers == null)
            {
                return HttpNotFound();
            }
            return View(buyers);
        }

        // POST: Buyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Buyers buyers = db.Buyers.Find(id);
            db.Buyers.Remove(buyers);
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
