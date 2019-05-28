using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Capstone.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index(string searchBy, string search)
        {
            if(searchBy == "Category")
            {
                return View(db.Items.Where(i => i.Category == search || search == null).ToList()); //search by Category
            }
            else
            {
                return View(db.Items.Where(i => i.ItemName.StartsWith(search) || search == null).ToList()); //Search by Item name
            }
            //return View(db.Items.ToList());
        }

        public FileContentResult ItemPhotos()
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


        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        //GET Items/Details/5
        public ActionResult BuyersDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.ItemsId = new SelectList(db.Items, "Id", "ItemPhoto");
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemName,Price,Category,Condition,Summary,ItemPhoto")] Items items)
        {
            //if (ModelState.IsValid)
            //{
            //    byte[] imageData = null;
            //    if (Request.Files.Count > 0)
            //    {
            //        HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

            //        using (var reader = new System.IO.BinaryReader(poImgFile.InputStream))
            //        {
            //            imageData = reader.ReadBytes(poImgFile.ContentLength);
            //        }
            //    }

                string CurrentUserId = User.Identity.GetUserId(); //user thats logged in now
                Sellers CurrentSellersInfo = db.Sellers.Where(s => s.ApplicationUserId == CurrentUserId).FirstOrDefault(); //comparing that user to the ApplicationUserId thats in the database and if its the same then grab them 
                items.SellersId = CurrentSellersInfo.SellersId; //connecting the sellersId of that item to the current user thats signed in Id
                var lat = db.Sellers.Where(s => s.SellersId == items.SellersId).Select(i => i.Lat).FirstOrDefault(); //getting the lat of the seller
                var lng = db.Sellers.Where(s => s.SellersId == items.SellersId).Select(i => i.Lng).FirstOrDefault(); //getting the lng of the seller
                items.Lat = lat;
                items.Lng = lng;
                db.Items.Add(items); //add the entire item to the items database
                //items.ItemPhoto = imageData; //tie the image in too
                db.SaveChanges();
                return RedirectToAction("Index", "Sellers"); //after Seller creates a listing it should return them to a list of their items for sale
            //}

            //return View(items);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemsId,ItemName,Price,Category,UserPhoto,Condition,Summary")] Items items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(items);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Items items = db.Items.Find(id);
            db.Items.Remove(items);
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
