using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Profile.App_Start;
using System.Web.Helpers;

namespace Profile.Controllers
{
    [LoginFilters]
    public class HomeController : Controller
    {
        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        // GET: Home
      
        public ActionResult Index(int id)
        {

            int profile_id = id;
            return View(db.Profiles.Where(p => p.profile_id != profile_id));

        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(db.Profiles.SingleOrDefault(p => p.profile_id == id));
        }
        public ActionResult Search(FormCollection collection)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int profile_id = int.Parse(Session["user_id"].ToString());
            string searchText = collection["text"];
            return View("Index", db.Profiles
                                .Where(p => p.profile_id != profile_id && (
                                            p.first_name.Contains(searchText) ||
                                            p.last_name.Contains(searchText) ||
                                            p.phone_number.Contains(searchText) ||
                                            p.user_name.Contains(searchText) ||
                                            p.email_address.Contains(searchText) ||
                                            p.website.Contains(searchText)))
                                .ToList());
        }
        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
         
          
            try
            {
              
                



                // TODO: Add insert logic here
                Models.Profile newProfile = new Models.Profile()
                {
                    profile_id = int.Parse(Session["user_id"].ToString()),
                    first_name = collection["first_name"],
                    last_name = collection["last_name"],
                    user_name = collection["user_name"],
                    biography = collection["biography"],
                    email_address = collection["email_address"],
                    gender = collection["gender"],
                    phone_number = collection["phone_number"],
                    website = collection["website"]
                };

                db.Profiles.Add(newProfile);
                db.SaveChanges();

                return RedirectToAction("Index", "Wall", null);
            }
            catch(Exception e)
            {
            
               return Content(e.Message);
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(db.Profiles.SingleOrDefault(p => p.profile_id == id));
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                // TODO: Add update logic here
                Models.Profile profile = db.Profiles.SingleOrDefault(p => p.profile_id == id);

                profile.first_name = collection["first_name"];
                profile.last_name = collection["last_name"];
                profile.user_name = collection["user_name"];
                profile.website = collection["website"];
                profile.email_address = collection["email_address"];
                profile.biography = collection["biography"];
                profile.gender = collection["gender"];
                profile.phone_number = collection["phone_number"];
                //profile.User.password = Crypto.HashPassword(collection["password"]);
                db.SaveChanges();

                return RedirectToAction("Details", new {id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(db.Profiles.SingleOrDefault(p => p.profile_id == id));
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                // TODO: Add delete logic here

                Models.Profile profile = db.Profiles.SingleOrDefault(p => p.profile_id == id);
                var profilePictures = db.Pictures.Where(p => p.profile_id == profile.profile_id).ToList();

                db.Pictures.RemoveRange(profile.Pictures);
                db.SaveChanges();

                db.Addresses.RemoveRange(profile.Addresses);
                db.SaveChanges();

                db.Profiles.Remove(profile);
                db.SaveChanges();

                string startupPath = Server.MapPath("~/Images/");
                foreach (Models.Picture p in profilePictures)
                {
                    System.IO.File.Delete(startupPath + p.path);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

      
        public ActionResult AddFriend(int id)
        {
            int current_id = int.Parse(Session["user_id"].ToString());
            try
            {
                // TODO: Add delete logic here
                Models.Friend newFriend = new Models.Friend()
                {
                    sender_id = current_id,
                    receiver_id = id,
                    relationship = "Pending",
                    time_stamp = DateTime.Now,
                    accepted = false
                };
                db.Friends.Add(newFriend);
                db.SaveChanges();


                return RedirectToAction("Index", new { id = current_id });
            }
            catch (Exception e)
            {
                return Content("Add friend request error" + e.Message);
            }
        }

       
        public ActionResult AcceptFriend(int id)
        {
            int current_id = int.Parse(Session["user_id"].ToString());
            try
            {
                int profile_id = current_id;
                Models.Friend friend = db.Friends.SingleOrDefault(f => f.sender_id == id && f.receiver_id == profile_id);
                friend.accepted = true;
                friend.relationship = "Friends";
                db.SaveChanges();
                return RedirectToAction("Index", new {id = current_id });
            }
            catch (Exception e)
            {
                return Content("Accept friend request error" + e.Message);
            }
        }


        public ActionResult RemoveFriend(int id)
        {
            int current_id = int.Parse(Session["user_id"].ToString());

            try
            {

                Models.Friend friend = db.Friends.SingleOrDefault(f => (f.sender_id == current_id && f.receiver_id == id) || (f.sender_id == id && f.receiver_id == current_id));
                db.Friends.Remove(friend);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = current_id });
            }
            catch (Exception e)
            {
                return Content("Remove friend request error" + e.Message);
            }
        }

        public ActionResult EditPassword(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(db.Users.SingleOrDefault(u => u.user_id == id));
        }

        [HttpPost]
        public ActionResult EditPassword(int id, FormCollection collection)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Models.User user = db.Users.SingleOrDefault(u => u.user_id == id);

            string currentPassword = collection["current_password"];
            string newPassword = collection["new_password"];
            string confirmPassword = collection["confirm_password"];

            if (Crypto.VerifyHashedPassword(user.password, currentPassword) &&
                newPassword.Equals(confirmPassword))
            {
                user.password = Crypto.HashPassword(newPassword);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("EditPassword", new { id = id });
            }
        }
    }
}
