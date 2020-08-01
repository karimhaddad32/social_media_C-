using Profile.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.Controllers
{

    [LoginFilters]
    public class WallController : Controller
    {

        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        // GET: Wall
        public ActionResult Index()
        { 
            

            int profile_id = int.Parse(Session["user_id"].ToString());
            var profile = db.Profiles.SingleOrDefault(p => p.profile_id == profile_id);
       
            var friends = profile.Friends;
            List<Models.Profile> friendsProfiles = friends.Where(f => f.accepted == true).Select(f => f.Profile1).ToList();
            var friends1 = profile.Friends1;

            List<Models.Profile> friendsProfiles1 = friends1.Where(f => f.accepted == true).Select(f => f.Profile).ToList();
            friendsProfiles.AddRange(friendsProfiles1);
 
            var picturePosts = profile.Pictures;
            var friendsPicturePosts = friendsProfiles.SelectMany(p => p.Pictures);

            List<Models.Picture> allPictures = new List<Models.Picture>();
            allPictures.AddRange(picturePosts);
            allPictures.AddRange(friendsPicturePosts);

            ViewBag.reactions_list = (SelectList)new SelectList(db.Reaction_Type, "reaction_type_id", "reaction_description");

            return View(allPictures);
        }

  
        




    }
}
