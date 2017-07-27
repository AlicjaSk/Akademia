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
    public class ReservationsController : Controller
    {
        private BookingEntities1 db = new BookingEntities1();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservation = db.Reservation.Include(r => r.Person).Include(r => r.Room).Include(r => r.StatusOfReservation);
            return View(reservation.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.Person_FK = new SelectList(db.Person, "IDPerson", "FirstName");
            ViewBag.Room_FK = new SelectList(db.Room, "IDRoom", "IDRoom");
            ViewBag.Status_FK = new SelectList(db.StatusOfReservation, "IDStatus", "Status");
            ViewBag.TypesOfRooms = new TypeOfRoomsController().GetAllTypes();
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDReservation,CheckIn,CheckOut,Room_FK,Status_FK,Cost,Person_FK")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservation.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Person_FK = new SelectList(db.Person, "IDPerson", "FirstName", reservation.Person_FK);
            ViewBag.Room_FK = new SelectList(db.Room, "IDRoom", "IDRoom", reservation.Room_FK);
            ViewBag.Status_FK = new SelectList(db.StatusOfReservation, "IDStatus", "Status", reservation.Status_FK);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Person_FK = new SelectList(db.Person, "IDPerson", "FirstName", reservation.Person_FK);
            ViewBag.Room_FK = new SelectList(db.Room, "IDRoom", "IDRoom", reservation.Room_FK);
            ViewBag.Status_FK = new SelectList(db.StatusOfReservation, "IDStatus", "Status", reservation.Status_FK);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDReservation,CheckIn,CheckOut,Room_FK,Status_FK,Cost,Person_FK")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Person_FK = new SelectList(db.Person, "IDPerson", "FirstName", reservation.Person_FK);
            ViewBag.Room_FK = new SelectList(db.Room, "IDRoom", "IDRoom", reservation.Room_FK);
            ViewBag.Status_FK = new SelectList(db.StatusOfReservation, "IDStatus", "Status", reservation.Status_FK);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservation.Find(id);
            db.Reservation.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CheckAvailabilityOfRoom([Bind(Include = "IDReservation,CheckIn,CheckOut,Room_FK,Status_FK,Cost,Person_FK")] Reservation reservation)
        {
            var tmp = Request.Form["TypesOfRooms"].ToString();


            var reservationForSpecificTypeOfRooms = db.Reservation.Where(rec => rec.Room.TypeOfRoom.IDTypeOfRoom == int.Parse(tmp)).ToList();
            var roomsSpecificType = db.Room
                .Where(rec => rec.TypeOfRoom.IDTypeOfRoom == int.Parse(tmp));


            int IDRoomForReservation = -1;
            foreach (var reserv in reservationForSpecificTypeOfRooms)
            {
                if (!roomsSpecificType.ToList().Select(rec=>rec.IDRoom).Contains(reserv.Room.IDRoom)){
                    IDRoomForReservation = reserv.Room.IDRoom;
                    break;
                }
            }

            if (IDRoomForReservation != -1)
            {
                return RedirectToAction("Details", "Rooms", new { id = IDRoomForReservation });
            }

            //var reservedRooms = new List<>();



            //bool reservedInSelectedTime=false;
            //foreach (var reserv in db.Reservation)
            //{
            //   if(reservation.CheckIn >= reserv.CheckIn && reservation.CheckIn <= reserv.CheckOut)
            //    {
            //        reservedInSelectedTime = true;
            //    }
            //    if (reservation.CheckOut >= reserv.CheckIn && reservation.CheckOut <= reserv.CheckOut)
            //    {
            //        reservedInSelectedTime = true;
            //    }

            //    if (reservedInSelectedTime)
            //    {
            //        reservedRooms.Add(reserv);
            //        reservedInSelectedTime = false;
            //    }
            //}


            //var rooms = db.Room.Where(record=>record.TypeOfRoom.IDTypeOfRoom == Int32.Parse(tmp)); 
            //reservationForSpecificRoom

            //foreach(var r in reservedRooms)
            //{
            //    if (r.Room.TypeOfRoom.IDTypeOfRoom == Int32.Parse(tmp)) {
            //        rooms.Remove(r.Room);
            //    }
            //    else
            //    {
            //        rooms.Remove(r.Room);
            //    }
            //}



            return View();
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
