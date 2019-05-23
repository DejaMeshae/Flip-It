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
    public class SellersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sellers
        public ActionResult Index()
        {
            //var sellers = db.Sellers.Include(s => s.ApplicationUser); display a list of users
            //string CurrentUserID = User.Identity.GetUserId(); //User ID thats logged in now
            var CurrentSeller = db.Items.Select(e => e.SellersId).FirstOrDefault(); // 
            var yourItemForSale = db.Items.Select(i => i.SellersId == CurrentSeller); //
            //var yourItemForSale = db.Sellers
            return View(yourItemForSale.ToList());
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
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SellersId,Email,Firstname,Lastname,Address,City,State,ZipCode")] Sellers sellers)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }

                db.Sellers.Add(sellers);
                sellers.UserPhoto = imageData;
                db.SaveChanges();
                return RedirectToAction("Details");
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
        public ActionResult Edit([Bind(Include = "SellersId,Firstname,Lastname,Address,City,State,ZipCode,UserPhoto,Lat,Lng,ApplicationUserId")] Sellers sellers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
