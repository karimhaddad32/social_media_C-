using Profile.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Profile.Controllers
{
    [LoginFilters]
    public class AddressController : Controller
    {

        Models.ProfilesDBEntities db = new Models.ProfilesDBEntities();
        // GET: Address
        public ActionResult Index(int id)
        {
            return View(db.Profiles.SingleOrDefault(p => p.profile_id == id));
        }

        // GET: Address/Details/5
        public ActionResult Details(int id, string controllerSource)
        {
                return View(db.Addresses.Single(p => p.address_id == id)); 
        }

        // GET: Address/Create
        public ActionResult Add(int id)
        {
            ViewBag.id = id;
            ViewBag.countries = new SelectList(db.Countries, "country_id", "country_name");
            return View();
        }

        // POST: Address/Create
        [HttpPost]
        public ActionResult Add(int id,FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Models.Address newAddress = new Models.Address()
                {
                    profile_id = id,
                    description = collection["description"],
                    street_address = collection["street_address"],
                    city = collection["city"],
                    postal_code = collection["postal_code"],
                    province = collection["province"],
                    country_id = int.Parse(collection["country_id"])
                };

                db.Addresses.Add(newAddress);
                db.SaveChanges(); 
  
                return RedirectToAction("Index", new {id = id});
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.countries = new SelectList(db.Countries.ToList(), "country_id", "country_name", db.Addresses.Single(p => p.address_id == id).Country.country_name);

            return View(db.Addresses.Single(p => p.address_id == id));
        }

        // POST: Address/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,string controllerSource, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Models.Address address = db.Addresses.Single(p => p.address_id == id);
                address.country_id = int.Parse(collection["country_id"]);
                address.city = collection["city"];
                address.province = collection["province"];
                address.street_address = collection["street_address"];
                address.postal_code = collection["postal_code"];
                address.description = collection["description"];

                db.SaveChanges();

                    return RedirectToAction("Index", new { id = address.profile_id });
                
        }
            catch
            {
                return View();
    }
}

        // GET: Address/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.Addresses.Single(p => p.address_id == id));
        }

        // POST: Address/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Address address = db.Addresses.Single(p => p.address_id == id);

                db.Addresses.Remove(address);
                db.SaveChanges();


              
                    return RedirectToAction("Index", new { id = address.profile_id });
                
            }
            catch
            {
                return View();
            }
        }
    }
}
