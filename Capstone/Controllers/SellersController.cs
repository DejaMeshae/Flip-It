using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Capstone.Models;
using System.Data.Entity.Infrastructure;

namespace Capstone.Controllers
{
    public class SellersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sellers
        public ActionResult Index()
        {
            //var sellers = db.Sellers.Include(s => s.ApplicationUser); display a list of users
            //string CurrentUserID = User.Identity.GetUserId(); //User ID thats logged in now
            //var CurrentSeller = db.Sellers.Where(e => e.ApplicationUserId == CurrentUserID).FirstOrDefault(); //comparing the user thats signed in ID to the ID in the database  
            //var SellerItems = db.Items.Where(i => i.SellersId == CurrentSeller.SellersId).ToList();
            //List<Items> SellerItems = db.Items.Where(i => i.SellersId == CurrentSeller.SellersId).ToList();
            //var yourItemForSale = db.Sellers.Where(i => i.SellersId == CurrentSeller.SellersId).ToList(); //comparing the sellers id of the item for sale to the id of the person thats logged in
            //return View("Index", SellerItems); //IF NOT WORKING (ERROR LINE 26) MAKE SURE THAT SELLER CREATED A LISTING IN THE FIRST PLACE
            return RedirectToAction("SellersItems", "Items"); //or redirect to details of the user
        }


        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/Question_Mark.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image /png");

                }
                // to get the user details to load user Image 
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/Question_Mark.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }



        // GET: Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Sellers sellers = db.Sellers.Where(s => s.SellersId == id).Include(a => a.ApplicationUser).FirstOrDefault();
            //Sellers sellers = db.Sellers.Include(s => s.Files).SingleOrDefault(s => s.SellersId == id);
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            return View(sellers);
        }

        // GET: Sellers/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Sellers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SellersId,Email,Firstname,Lastname,Address,City,State,ZipCode")] Sellers sellers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sellers.ApplicationUserId = User.Identity.GetUserId();
                    string address = (sellers.Address + "+" + sellers.City + "+" + sellers.State + "+" + sellers.ZipCode);
                    GeoLocationController geoLocation = new GeoLocationController();
                    geoLocation.SendRequest(address);
                    sellers.Lat = geoLocation.latitude;
                    sellers.Lng = geoLocation.longitude;
                    db.Sellers.Add(sellers);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Items");
                    //return RedirectToAction("Details", new { id = sellers.SellersId }); //or redirect to details of the user
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", sellers.ApplicationUserId);
            return View(sellers);
        }

        // GET: Sellers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", sellers.ApplicationUserId);
            return View(sellers);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SellersId,Firstname,Lastname,Address,City,State,ZipCode")] Sellers sellers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = sellers.SellersId }); 
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", sellers.ApplicationUserId);
            return View(sellers);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            return View(sellers);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sellers sellers = db.Sellers.Find(id);
            db.Sellers.Remove(sellers);
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
