using Profile.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.Controllers
{
    [LoginFilters]
    public class CommentController : Controller
    {
         Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection)
        {

            if (String.IsNullOrWhiteSpace(collection["comment"]))
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            try
            {
                Models.Comment newComment = new Models.Comment()
                {
                    profile_id = int.Parse(Session["user_id"].ToString()),
                    picture_id = id,
                    comment1 = collection["comment"],
                    time_stamp = DateTime.Now
                };

                db.Comments.Add(newComment);
                db.SaveChanges();
                //string controller = this.ControllerContext.Controller.ToString();
                //if (this.ControllerContext.Controller.ToString().Equals("Wall"))
                //{
                //    return RedirectToAction("Index");
                //}
                //else
                //{
                //    return RedirectToAction("Details", "Profile", new {id = db.Pictures.Single(p => p.picture_id == id).profile_id });
                //}

                //Use Ajax Instead of this.
                return Redirect(Request.UrlReferrer.ToString());

            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int profile_id = int.Parse(Session["user_id"].ToString());
            var comment = db.Comments.SingleOrDefault(c => c.comment_id == id);
            if(comment != null)
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", "Wall", new { id = profile_id });
            }
         
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int profile_id = int.Parse(Session["user_id"].ToString());
            Models.Comment comment = db.Comments.SingleOrDefault(c => c.comment_id == id);
            int commentid = comment.comment_id;

            comment.comment1 = collection["updated_comment"];
            db.SaveChanges();

            return RedirectToAction("Index", "Wall", null);
        }

        // POST: Comment/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                var comment = db.Comments.Single(c => c.comment_id == id);
                db.Comment_Reaction.RemoveRange(comment.Comment_Reaction);
                db.Comments.Remove(comment);
                db.SaveChanges();

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddReaction(Models.Comment_Reaction reaction)
        {
            try
            {
                // TODO: Add delete logic here

                //Models.Reaction_Type reaction = db.Reaction_Type.SingleOrDefault(p => p.reaction_type_id.ToString() == collection["reaction_type_id"]);

                int current_user = int.Parse(Session["user_id"].ToString());
                var commentReaction = db.Comment_Reaction.SingleOrDefault(p => p.profile_id == current_user && p.comment_id == reaction.comment_id);

                Models.Comment_Reaction newReaction = new Models.Comment_Reaction()
                {
                    profile_id = int.Parse(Session["user_id"].ToString()),
                    comment_id = reaction.comment_id,
                    reaction_type_id = reaction.reaction_type_id,
                    time_stamp = DateTime.Now
                };

                Boolean is_new = false;
                int old_reaction_id = 0;
                if (commentReaction != null)
                {
                    old_reaction_id = (int)commentReaction.reaction_type_id;
                    if (commentReaction.reaction_type_id != reaction.reaction_type_id)
                    {

                        db.Comment_Reaction.Remove(commentReaction);
                        db.SaveChanges();
                        db.Comment_Reaction.Add(newReaction);
                    }
                    else
                    {
                   

                        db.Comment_Reaction.Remove(commentReaction);
                    }
         
                }
                else
                {
                    is_new = true;
                    db.Comment_Reaction.Add(newReaction);
                }

                db.SaveChanges();

                return Json(new { newReaction = newReaction, type = "comment", is_new = is_new, old_reaction_id = old_reaction_id }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
    }
}
