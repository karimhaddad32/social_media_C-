using Profile.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.Controllers
{

    [LoginFilters]
    public class NotificationController : Controller
    {

        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        List<Notification> notificationList = new List<Notification>();
        public ActionResult Index()
        {
            int profile_id = int.Parse(Session["user_id"].ToString());
            var profile = db.Profiles.SingleOrDefault(p => p.profile_id == profile_id);

            // Query the database for all relevant notifications
            List<Models.Comment> pictureComments = (db.Comments.Where(c => c.Picture.profile_id == profile_id && c.profile_id != profile_id)).ToList();
            List<Models.Picture_Reaction> pictureReactions = (db.Picture_Reaction.Where(r => r.Picture.profile_id == profile_id && r.profile_id != profile_id)).ToList();
            List<Models.Comment_Reaction> commentReactions = (db.Comment_Reaction.Where(r => r.Comment.profile_id == profile_id && r.profile_id != profile_id)).ToList();
            List<Models.Friend> receivedFriendRequests = (db.Friends.Where(f => f.receiver_id == profile_id && f.accepted == false)).ToList();
            List<Models.Friend> acceptedFriendRequests = (db.Friends.Where(f => f.sender_id == profile_id && f.accepted == true)).ToList();

            List<Models.Tag> tags = (db.Tags.Where(t => t.tagged_id == profile_id && t.tagger_id != profile_id)).ToList();

            // Add all the notifications in a single mega list

            foreach (var item in pictureComments)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.NewPictureComment, item.time_stamp, item.seen));
                item.seen = true;
            }
            foreach (var item in pictureReactions)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.NewPictureReaction, item.time_stamp, item.seen));
                item.seen = true;
            }
            foreach (var item in commentReactions)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.NewCommentReaction, item.time_stamp, item.seen));
                item.seen = true;
            }
            foreach (var item in receivedFriendRequests)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.FriendReceived, item.time_stamp, item.seen));
                item.seen = true;
            }
            foreach (var item in acceptedFriendRequests)
            {
                // Here we send in the other profile because you are the one who sent the request
                notificationList.Add(new Notification(item.Profile1, NotificationType.FriendAccepted, item.time_stamp, item.seen));
                item.seen = true;
            }

            foreach (var item in tags)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.CommentTag, item.time_stamp, item.seen));
                item.seen = true;
            }

            db.SaveChanges();

            // Sort the list in descending order and add it to the viewbag
            notificationList = notificationList.OrderByDescending(n => n.GetTimestamp()).ToList();
            return View(notificationList);

           
        }
        public ActionResult GetCount()
        {
            int profile_id = int.Parse(Session["user_id"].ToString());
            var profile = db.Profiles.SingleOrDefault(p => p.profile_id == profile_id);

            // Query the database for all relevant notifications
            List<Models.Comment> pictureComments = (db.Comments.Where(c => c.Picture.profile_id == profile_id && c.profile_id != profile_id && c.seen == false)).ToList();
            List<Models.Picture_Reaction> pictureReactions = (db.Picture_Reaction.Where(r => r.Picture.profile_id == profile_id && r.profile_id != profile_id && r.seen == false)).ToList();
            List<Models.Comment_Reaction> commentReactions = (db.Comment_Reaction.Where(r => r.Comment.profile_id == profile_id && r.profile_id != profile_id && r.seen == false)).ToList();
            List<Models.Friend> receivedFriendRequests = (db.Friends.Where(f => f.receiver_id == profile_id && f.accepted == false && f.seen == false)).ToList();
            List<Models.Friend> acceptedFriendRequests = (db.Friends.Where(f => f.sender_id == profile_id && f.accepted == true && f.seen == false)).ToList();

            List<Models.Tag> tags = (db.Tags.Where(t => t.tagged_id == profile_id && t.tagger_id != profile_id)).ToList();

            // Add all the notifications in a single mega list

            foreach (var item in pictureComments)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.NewPictureComment, item.time_stamp, item.seen));
            }
            foreach (var item in pictureReactions)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.NewPictureReaction, item.time_stamp, item.seen));
            }
            foreach (var item in commentReactions)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.NewCommentReaction, item.time_stamp, item.seen));
            }
            foreach (var item in receivedFriendRequests)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.FriendReceived, item.time_stamp, item.seen));
            }
            foreach (var item in acceptedFriendRequests)
            {
                // Here we send in the other profile because you are the one who sent the request
                notificationList.Add(new Notification(item.Profile1, NotificationType.FriendAccepted, item.time_stamp, item.seen));
            }

            foreach (var item in tags)
            {
                notificationList.Add(new Notification(item.Profile, NotificationType.CommentTag, item.time_stamp, item.seen));
            }

            // Sort the list in descending order and add it to the viewbag
            notificationList = notificationList.OrderByDescending(n => n.GetTimestamp()).ToList();

             return Json(new { count = notificationList.Count()}, JsonRequestBehavior.AllowGet);

        }

    }
}