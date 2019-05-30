using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Braintree;
using Capstone.Models;
using Microsoft.AspNet.Identity;

namespace Capstone.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public ActionResult Index()
        {
            var buyers = db.Buyers.Include(m => m.Sellers);
            return View(buyers.ToList());
            //string CurrentUserID = User.Identity.GetUserId(); //User ID thats logged in now
            //var CurrentSeller = db.Sellers.Where(e => e.ApplicationUserId == CurrentUserID).FirstOrDefault(); //comparing the user thats signed in ID to the ID in the database  
            //var SellerMessages = db.Buyers.Where(i => i.SellersId == CurrentSeller.SellersId).ToList();
            //return View(SellerMessages);

        }

        public ActionResult PaypalCheckout()
        {

            return View("PayPalCheckout");
        }

        // GET: Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Buyers.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Message/Create
        public ActionResult Create() 
        {
            ViewBag.SellersId = new SelectList(db.Sellers, "SellersId", "Firstname");
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,MessageBox,SellersId")] Message message)
        {
            if (ModelState.IsValid)
            {
                string CurrentUserId = User.Identity.GetUserId(); //user thats logged in now
                Sellers CurrentSellersInfo = db.Sellers.Where(s => s.ApplicationUserId == CurrentUserId).FirstOrDefault(); //comparing that user to the ApplicationUserId thats in the database and if its the same then grab them 
                message.SellersId = CurrentSellersInfo.SellersId; //connecting the sellersId of that item to the message Sellers ID
                db.Buyers.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SellersId = new SelectList(db.Sellers, "SellersId", "Firstname", message.SellersId);
            return View(message);
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Buyers.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellersId = new SelectList(db.Sellers, "SellersId", "Firstname", message.SellersId);
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,MessageBox,SellersId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SellersId = new SelectList(db.Sellers, "SellersId", "Firstname", message.SellersId);
            return View(message);
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Buyers.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Buyers.Find(id);
            db.Buyers.Remove(message);
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
