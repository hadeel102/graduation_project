using poject.Model;
using poject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class RequestIncubationController : Controller
    {
        // GET: RequestIncubation
        public ActionResult Index()
        {
            using (var db = new ECSalesEntities2())
            {
                var UserID = (Session["UserInfo"] as TblUser).ID;
                ViewBag.list = db.TblRequestedIncubations.Where(x => x.RequestedUser == UserID
                

                 ).ToList();

            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveRequestedIncubation(vmRequestedIncubation oproduct)
        {
            using (var db = new ECSalesEntities2())
            {
                if (oproduct.ID <= 0)
                {

                    string UploadPath = string.Empty;
                    //Use Namespace called :  System.IO  
                    string FileName = Path.GetFileNameWithoutExtension(oproduct.ImageFile.FileName);

                    //To Get File Extension  
                    string FileExtension = Path.GetExtension(oproduct.ImageFile.FileName);

                    UploadPath = Server.MapPath("~/Uploads/RequestedIncubation/") + FileName + FileExtension;
                    db.TblRequestedIncubations.Add(new TblRequestedIncubation()
                    {
                        AttchedFile = FileName + FileExtension,
                        RequestedDescription = oproduct.RequestedDescription,
                        RequestedUser = oproduct.RequestedUser,
                        Status = oproduct.Status
                    });
                    oproduct.ImageFile.SaveAs(UploadPath);
                }
                else
                {
                    //Update 
                    var op = db.TblRequestedIncubations.Find(oproduct.ID);
                    op.RequestedDescription = oproduct.RequestedDescription;
                    op.Status = oproduct.Status;

                    if (oproduct.ImageFile != null && oproduct.ImageFile.FileName != null)
                    {
                        string UploadPath = string.Empty;
                        //Use Namespace called :  System.IO  
                        string FileName = Path.GetFileNameWithoutExtension(oproduct.ImageFile.FileName);

                        //To Get File Extension  
                        string FileExtension = Path.GetExtension(oproduct.ImageFile.FileName);

                        UploadPath = Server.MapPath("~/Uploads/RequestedIncubation/") + FileName + FileExtension;

                        oproduct.ImageFile.SaveAs(UploadPath);

                        op.AttchedFile = FileName + FileExtension;

                        db.Entry<TblRequestedIncubation>(op).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                db.SaveChanges();

            }

            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult Search(FormCollection oform)
        {

            var SearchEname = string.Format("{0}", oform["SearchEname"]).Trim();

            using (var db = new ECSalesEntities2())
            {
                var UserID = (Session["UserInfo"] as TblUser).ID;
                ViewBag.list = db.TblRequestedIncubations.Where(x => x.RequestedUser == UserID
                 && (SearchEname == "" || x.RequestedDescription.Contains(SearchEname))

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
                data = new TblRequestedIncubation
                {
                    AdminComment = oProduct.AdminComment,
                    AttchedFile = oProduct.AttchedFile,
                    RequestedDescription = oProduct.RequestedDescription,
                    RequestedUser = oProduct.RequestedUser,
                    Status = oProduct.Status,
                    ID = oProduct.ID

                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}