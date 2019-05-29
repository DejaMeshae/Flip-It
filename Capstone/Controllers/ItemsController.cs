using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
                return View(db.Items.Where(i => i.ItemName.Contains(search) || search == null).ToList()); //Search by Item name
            }
            //return View(db.Items.ToList());
        }

        public ActionResult SellersItems()
        {
            //var sellers = db.Sellers.Include(s => s.ApplicationUser); display a list of users
            string CurrentUserID = User.Identity.GetUserId(); //User ID thats logged in now
            var CurrentSeller = db.Sellers.Where(e => e.ApplicationUserId == CurrentUserID).FirstOrDefault(); //comparing the user thats signed in ID to the ID in the database  
            var SellerItems = db.Items.Where(i => i.SellersId == CurrentSeller.SellersId).ToList();
            //List<Items> SellerItems = db.Items.Where(i => i.SellersId == CurrentSeller.SellersId).ToList();
            //var yourItemForSale = db.Sellers.Where(i => i.SellersId == CurrentSeller.SellersId).ToList(); //comparing the sellers id of the item for sale to the id of the person thats logged in
            return View(SellerItems);
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.Items.Include(i => i.Files).SingleOrDefault(j => j.ItemsId == id);
            //Items items = db.Items.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        //To View Sellers Profile
        public ActionResult SellerDetails(int? id)
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

        //GET For the potential buyer to see details of an item
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
            ViewBag.ItemsId = new SelectList(db.Items, "Id");
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemName,Price,Status,Category,Condition,Summary")] Items items, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var avatar = new Models.File //had to add Model in front instead of just File like in UploadPhoto 
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        items.Files = new List<Models.File> { avatar };
                    }
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
                    return RedirectToAction("SellersItems", "Items"); //after Seller creates a listing it should return them to a list of their items for sale
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(items);

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
        //public ActionResult Edit([Bind(Include = "ItemName,Price,Status,Category,Condition,Summary")] Items items)
        public ActionResult Edit(Items items)

        {
            if (ModelState.IsValid)
            //{
            //    db.Entry(items).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            {
                //query by id (primary key) again to grab all of that superhero info
                var itemsToEdit = db.Items.Where(h => h.ItemsId == items.ItemsId).SingleOrDefault();
                itemsToEdit.Status = items.Status;
                itemsToEdit.Price = items.Price;
                db.SaveChanges();
                return RedirectToAction("SellersItems");
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
            return RedirectToAction("SellersItems");
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
