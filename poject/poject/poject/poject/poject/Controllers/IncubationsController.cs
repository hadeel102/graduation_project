using poject.Model;
using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class IncubationsController : Controller
    {
        // GET: Incubations
        public ActionResult Index()
        {
            using (var db = new ECSalesEntities2())
            {
                ViewBag.list = db.TblRequestedIncubations.Include("TblUser").ToList();

            }
            return View();
        }
        [HttpPost]
        public ActionResult Search(FormCollection oform)
        {

            var SearchEname = string.Format("{0}", oform["SearchEname"]).Trim();

            using (var db = new ECSalesEntities2())
            {
                ViewBag.list = db.TblRequestedIncubations.Where(x => (SearchEname == "" || x.RequestedDescription.Contains(SearchEname))

                 ).ToList();

            }
            return PartialView("_GridContent", ViewBag.list);
        }

        [HttpPost]
        public ActionResult GetById(int id)
        {

            TblRequestedIncubation oProduct = new TblRequestedIncubation();
            using (var db = new ECSalesEntities2())
            {
                oProduct = db.TblRequestedIncubations.Where(x => x.ID == id).FirstOrDefault();

            }

            return Json(new
            {
                data = new vmRequestedIncubation
                { 
                    AdminComment = oProduct.AdminComment,
                    AttchedFile = oProduct.AttchedFile,
                    RequestedDescription = oProduct.RequestedDescription,
                    RequestedUser = oProduct.RequestedUser.GetValueOrDefault(0),
                    Status = oProduct.Status.GetValueOrDefault(0),
                    ID = oProduct.ID

                }
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public FileResult DownloadFile(string AttchedFile)
        {
            //Build the File Path.
            string path = Server.MapPath("~/Uploads/RequestedIncubation/") + AttchedFile;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", AttchedFile);
        }
        [HttpPost]
        public ActionResult Approve(vmRequestedIncubation product) {
            using (var db = new ECSalesEntities2())
            {
                var oProduct = db.TblRequestedIncubations.Where(x => x.ID == product.ID).FirstOrDefault();
                oProduct.Status = 1;
                oProduct.AdminComment = product.AdminComment;
                db.SaveChanges();
            }
            return Json(new
            {
                data = new 
                {
                   success= true

                }
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Reject(vmRequestedIncubation product)
        {
            using (var db = new ECSalesEntities2())
            {
                var oProduct = db.TblRequestedIncubations.Where(x => x.ID == product.ID).FirstOrDefault();
                oProduct.Status = 1;
                oProduct.AdminComment = product.AdminComment;
                db.SaveChanges();
            }
            return Json(new
            {
                data = new
                {
                    success = true

                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}