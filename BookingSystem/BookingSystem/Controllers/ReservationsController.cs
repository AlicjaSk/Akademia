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

        private static bool checkIfDatesHaveCommonPart(Dictionary<DateTime, bool> dictionary)
        {
            var sortedDict = from entry in dictionary orderby entry.Key ascending select entry;
            if (sortedDict.ToList()[0].Value == sortedDict.ToList()[1].Value)
                return true;
            else
                return false;
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
            var tmp = Request.Form["TypesOfRooms"];
            var reservationForSpecificTypeOfRooms = db.Reservation.Where(rec => rec.Room.TypeOfRoom.IDTypeOfRoom.ToString() == tmp);
            var roomsSpecificType = db.Room
                .Where(rec => rec.TypeOfRoom.IDTypeOfRoom.ToString() == tmp);
            var rooms = new Dictionary<int, bool>();
            roomsSpecificType.ToList().ForEach(room => rooms.Add(room.IDRoom, true));

            foreach (var reserv in reservationForSpecificTypeOfRooms)
            {
               if (checkIfDatesHaveCommonPart(new Dictionary<DateTime, bool>()
                {
                    { reserv.CheckIn, true},
                    { reserv.CheckOut, true},
                    { reservation.CheckIn, false},
                    { reservation.CheckOut, false}
                }))
                {
                    rooms[reserv.Room_FK] = false;
                }
            }
            int freeRoomId = -1;
            rooms.ToList().ForEach(r => freeRoomId = (r.Value == true) ? r.Key : -1);

            
            if (freeRoomId != -1)
            {

                reservation.Room_FK = freeRoomId;
                TempData["reservation"] = reservation;  
                return RedirectToAction("ReservationOfRoom", "Rooms");
            }
            else
                return RedirectToAction("Index", "Rooms");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Reservations/ReservationOfRoom/5
        public ActionResult ReservationOfRoom(int? id)
        {
           
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            // ViewBag.reservation = reserv;
            return View(room);
        }


    }
}
