using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingSystem.Models;

namespace BookingSystem.Controllers
{
    public class TypeOfRoomsController : Controller
    {
        private BookingEntities1 db = new BookingEntities1();

        // GET: TypeOfRooms
        public ActionResult Index()
        {
            return View(db.TypeOfRoom.ToList());
        }

        // GET: TypeOfRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfRoom typeOfRoom = db.TypeOfRoom.Find(id);
            if (typeOfRoom == null)
            {
                return HttpNotFound();
            }
            return View(typeOfRoom);
        }

        public List <SelectListItem> GetAllTypes()
        {
            var typesSelectList = new List<SelectListItem>();

            foreach (var type in db.TypeOfRoom)
            {
                typesSelectList.Add(new SelectListItem { Text = type.Information, Value = type.IDTypeOfRoom.ToString() });
            }
            return typesSelectList;
        }

        // GET: TypeOfRooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeOfRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTypeOfRoom,Information,NumberOfPerson")] TypeOfRoom typeOfRoom)
        {
            if (ModelState.IsValid)
            {
                db.TypeOfRoom.Add(typeOfRoom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeOfRoom);
        }

        // GET: TypeOfRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfRoom typeOfRoom = db.TypeOfRoom.Find(id);
            if (typeOfRoom == null)
            {
                return HttpNotFound();
            }
            return View(typeOfRoom);
        }

        // POST: TypeOfRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDTypeOfRoom,Information,NumberOfPerson")] TypeOfRoom typeOfRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeOfRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeOfRoom);
        }

        // GET: TypeOfRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfRoom typeOfRoom = db.TypeOfRoom.Find(id);
            if (typeOfRoom == null)
            {
                return HttpNotFound();
            }
            return View(typeOfRoom);
        }

        // POST: TypeOfRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeOfRoom typeOfRoom = db.TypeOfRoom.Find(id);
            db.TypeOfRoom.Remove(typeOfRoom);
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
