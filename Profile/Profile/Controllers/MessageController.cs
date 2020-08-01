using Profile.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.Controllers
{
    [LoginFilters]
    public class MessageController : Controller
    {
        // GET: Message
        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        public ActionResult Index(int id)
        {
            //Show messages
         
            var profile = db.Profiles.Single(p => p.profile_id == id);

            var groupedMessages1 = profile.Messages;
            var groupedMessages2 = profile.Messages1;

             var group = groupedMessages1.Union(groupedMessages2);

            List<Models.Message> messages = new List<Models.Message>();

            foreach (Models.Message message in group.OrderByDescending(m => m.time_stamp))
            {
                if(!messages.Exists( m => (m.sender_id == message.sender_id && m.receiver_id == message.receiver_id) || (m.sender_id == message.receiver_id && m.receiver_id == message.sender_id)))
                {
                    messages.Add(message);
                }
            }

            return View(messages);
        }


        public ActionResult Details(int id)//Message Id
        {
            var message = db.Messages.SingleOrDefault(p => p.message_id == id);
            int sender_id = int.Parse(Session["user_id"].ToString());
            int receiver_id = (message.sender_id == sender_id ? message.receiver_id : message.sender_id);

            // Set all sender's messages to seen when entering the conversation
            foreach(var msg in db.Messages.Where(m => m.receiver_id == sender_id && m.sender_id == receiver_id))
            {
                msg.seen = true;
            }
            db.SaveChanges();

            return RedirectToAction("MessageDetails", new { id = receiver_id });
        }

        // GET: Message/Edit/5
        public ActionResult MessageDetails(int id)//receiver Id
        {
            int current_id = int.Parse(Session["user_id"].ToString());

            var receiver = db.Profiles.Single(p => p.profile_id == id);
          
            ViewBag.receiver = id;
            ViewBag.receiver_name = receiver.user_name;
            if(receiver.Pictures.SingleOrDefault(pic => pic.picture_id == receiver.profile_picture) != null)
            {
                ViewBag.receiver_image_path = receiver.Pictures.SingleOrDefault(pic => pic.picture_id == receiver.profile_picture).path;
            }
            else
            {
                ViewBag.receiver_image_path = "Default.png";
            }

            return View(db.Messages.Where(p => (p.sender_id == current_id && p.receiver_id == id) || (p.sender_id == id && p.receiver_id == current_id)));
        }

        // POST: Message/Edit/5
        [HttpPost]
        public ActionResult Send(Models.Message msg) // receiver id
        {
            try
            {
                // TODO: Add update logic here

                if (!String.IsNullOrWhiteSpace(msg.message1))
                {
                    Models.Message newMessage = new Models.Message()
                    {
                        sender_id = msg.sender_id,
                        receiver_id = msg.receiver_id,
                        message1 = msg.message1,
                        time_stamp = DateTime.Now
                    };

                    db.Messages.Add(newMessage);
                    db.SaveChanges();
                    return Json(new { timestamp = newMessage.time_stamp.ToString("HH:mm") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null , JsonRequestBehavior.AllowGet);
                }

               
    

                
            }
            catch(Exception e)
            {
                return Content(e.Message);
            }
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Message/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetCount()
        {
            int profile_id = int.Parse(Session["user_id"].ToString());

            List<Models.Message> unreadMessages = (db.Messages.Where(m => m.receiver_id == profile_id && m.seen == false)).ToList();
            return Json(new { count = unreadMessages.Count() }, JsonRequestBehavior.AllowGet);

        }
    }
}
