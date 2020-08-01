
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Profile.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login
        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        public ActionResult Index()
        {
            Session["user_id"] = null;
            Session["username"] = null;
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string username = collection["username"];
                string password = collection["password"];
                var user = db.Users.SingleOrDefault(p => p.username == username);
                if(user != null && Crypto.VerifyHashedPassword(user.password, password))
                {
                    Session["user_id"] = user.user_id;
                    Session.Timeout = 60; 
                    Session["username"] = username;
                    if(db.Profiles.SingleOrDefault(p => p.profile_id == user.user_id) == null)
                    {
                        return RedirectToAction("Create","Home");
                    }
                    return RedirectToAction("Index","Wall");
                }        
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return Content(e.Message);
            }
        }

        // GET: Login/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string username = collection["username"];
                var user = db.Users.SingleOrDefault(p => p.username == username);
                if(user != null)
                {
                    ViewBag.Message = username + " is already taken! please try a different Username";
                    return View();
                }


                Models.User newUser = new Models.User()
                {
                    username = collection["username"],
                    password = Crypto.HashPassword(collection["password"])

                };
                
                db.Users.Add(newUser);
                db.SaveChanges();

                Session["user_id"] = newUser.user_id;
                Session["username"] = newUser.username;

                return RedirectToAction("Create","Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }


    }
}
