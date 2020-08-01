using Profile.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.Controllers
{
    [LoginFilters]
    public class PictureController : Controller
    {
        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();

        // GET: Picture
        public ActionResult Index(int id)
        {  
            return View(db.Profiles.SingleOrDefault(p => p.profile_id == id));           
        }

        // GET: Picture/Add
        public ActionResult Add(int id)
        {
            ViewBag.id = id;
            return View();
        }

        // POST: Picture/Add
        [HttpPost]
        public ActionResult Add(int id,FormCollection collection, HttpPostedFileBase path )
        {
            try
            {
                // TODO: Add insert logic here

                string[] allowedTypes = { "image/jpeg", "image/png", "image/gif", "image/jpg" };
                string type = path.ContentType;


                if (path != null && path.ContentLength > 0 && allowedTypes.Contains(type))
                {
                    Guid g = Guid.NewGuid();


                var fileName = g.ToString() + Path.GetExtension(path.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                path.SaveAs(filePath);

                    Models.Picture newPicture = new Models.Picture() {
                        profile_id = id,
                        caption = collection["caption"],
                        location = collection["location"],
                        path = fileName,
                        timestamp = DateTime.Now
                };

                db.Pictures.Add(newPicture);
                db.SaveChanges();

                }
                return RedirectToAction("Details","Home", new {id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Picture/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.Pictures.SingleOrDefault(p => p.picture_id ==id));
        }

        // POST: Picture/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Picture picture = db.Pictures.SingleOrDefault(p => p.picture_id == id);
                Models.Profile profile = db.Profiles.SingleOrDefault(p => p.profile_id == picture.profile_id);

                if(picture.picture_id == profile.profile_picture)
                {
                    profile.profile_picture = null;
                    db.SaveChanges();
                }

                db.Picture_Reaction.RemoveRange(picture.Picture_Reaction);

                foreach(var item in picture.Comments)
                {
                    db.Comment_Reaction.RemoveRange(item.Comment_Reaction);
                }
      
                db.Comments.RemoveRange(picture.Comments);
                
                db.Pictures.Remove(picture);
                db.SaveChanges();

                DeletePictureFromImagesFile(picture.path);

                    return RedirectToAction("Details", "Home", new { id = picture.profile_id });
                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SetProfilePicture(int id ,int profile_id ,FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here


                Models.Profile profile = db.Profiles.SingleOrDefault(p => p.profile_id == profile_id);
                profile.profile_picture = id;
                db.SaveChanges();

              
                    return RedirectToAction("Details","Home", new { id = profile.profile_id });
        }
            catch
            {
            return View();
    }
}

        public void DeletePictureFromImagesFile(string path)
        {
            string startupPath = Server.MapPath("~/Images/");
            System.IO.File.Delete(startupPath + path);
        }


        public ActionResult AddReaction(Models.Picture_Reaction reaction)
        {
            try
            {
                // TODO: Add delete logic here
                var id = (int)reaction.picture_id;
                var reaction_id = (int)reaction.reaction_type_id;

                //Models.Reaction_Type reaction = db.Reaction_Type.SingleOrDefault(p => p.reaction_type_id.ToString() == collection["reaction_type_id"]);

                int current_user = int.Parse(Session["user_id"].ToString());
                var pictureReaction = db.Picture_Reaction.SingleOrDefault(p => p.profile_id == current_user && p.picture_id == id);

                Models.Picture_Reaction newReaction = new Models.Picture_Reaction()
                {
                    profile_id = int.Parse(Session["user_id"].ToString()),
                    picture_id = id,
                    reaction_type_id = reaction_id,
                    time_stamp = DateTime.Now
                };




                int old_reaction_id = 0;
                Boolean is_new = false;

                if (pictureReaction != null)
                {
                    old_reaction_id = (int)pictureReaction.reaction_type_id;
                    if (pictureReaction.reaction_type_id != reaction_id)
                    {
                        db.Picture_Reaction.Remove(pictureReaction);
                        db.SaveChanges();
                        db.Picture_Reaction.Add(newReaction);
                    }
                    else
                    {
                        db.Picture_Reaction.Remove(pictureReaction);
                    }

                }
                else
                {
                    is_new = true;
                    db.Picture_Reaction.Add(newReaction);
                }
                
                db.SaveChanges();

                return Json(new { newReaction = newReaction, type = "picture", is_new = is_new, old_reaction_id = old_reaction_id }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }


    }
}
