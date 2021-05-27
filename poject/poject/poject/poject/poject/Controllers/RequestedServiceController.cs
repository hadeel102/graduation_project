using Newtonsoft.Json;
using poject.Model;
using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class RequestedServiceController : Controller
    {
        public ActionResult Search(FormCollection oform)
        {

            var SearchEname = string.Format("{0}", oform["SearchEname"]).Trim();
            var SearchPrice = string.Format("{0}", oform["SearchPrice"]).Trim();
            var dSearchPrice = (SearchPrice == "") ? 0 : decimal.Parse(SearchPrice);
            using (var db = new ECSalesEntities2())
            {

                ViewBag.list = db.products.Include("TblCategory").Where(x => x.IsService == false
                 && (SearchEname == "" || x.Ename.Contains(SearchEname))
                   && (SearchPrice == "" || x.Price == dSearchPrice)
                   && x.status == 0
                 ).ToList();
                var dataCategory = db.TblCategories.Where(
                     x => x.IsService == true && x.ParentID != null
                     ).ToList();
                IEnumerable<SelectListItem> olstData = dataCategory.Select(x => new SelectListItem()
                {
                    Text = x.Ename,
                    Value = string.Format("{0}", x.ID),
                    Selected = false
                }).ToList();
                ViewBag.Categiores = olstData;

            }
            return PartialView("_GridContent", ViewBag.list);
        }
        // GET: RequestedProducts
        public ActionResult Index()
        {
            using (var db = new ECSalesEntities2())
            {
                ViewBag.list = db.products.Include("TblCategory").Where(x => x.IsService == true && x.status == 0).ToList();
                var dataCategory = db.TblCategories.Where(
                     x => x.IsService == true && x.ParentID != null
                     ).ToList();
                IEnumerable<SelectListItem> olstData = dataCategory.Select(x => new SelectListItem()
                {
                    Text = x.Ename,
                    Value = string.Format("{0}", x.ID),
                    Selected = false
                }).ToList();
                ViewBag.Categiores = olstData;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Approve(vmproduct product)
        {
            using (var db = new ECSalesEntities2())
            {
                var oProduct = db.products.Where(x => x.ID == product.ID).FirstOrDefault();
                oProduct.status = 1;
                oProduct.AdminComment = product.AdminComment;
                oProduct.OldData = null;

                db.productHistories.Add(new productHistory()
                {
                    Aname = oProduct.Aname,
                    CategoryID = oProduct.CategoryID,
                    Description = oProduct.Description,
                    Ename = oProduct.Ename,
                    ID = oProduct.ID,
                    Image1 = oProduct.Image1,
                    IsService = true,
                    Price = oProduct.Price,
                    UserID = (Session["UserInfo"] as TblUser).ID,
                    TotalQty = oProduct.TotalQty,
                    status = 1,
                    AdminComment = product.AdminComment
                });

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
        [HttpPost]
        public ActionResult Reject(vmproduct vmproduct)
        {
            using (var db = new ECSalesEntities2())
            {
                var oProduct = db.products.AsNoTracking().Where(x => x.ID == vmproduct.ID).FirstOrDefault();
                var olddata = oProduct.OldData;
                db.productHistories.Add(new productHistory()
                {
                    Aname = oProduct.Aname,
                    CategoryID = oProduct.CategoryID,
                    Description = oProduct.Description,
                    Ename = oProduct.Ename,
                    ID = oProduct.ID,
                    Image1 = oProduct.Image1,
                    IsService = true,
                    Price = oProduct.Price,
                    UserID = (Session["UserInfo"] as TblUser).ID,
                    TotalQty = oProduct.TotalQty,
                    status = 2,
                    AdminComment = oProduct.AdminComment
                });
                if (string.IsNullOrEmpty(olddata))
                {
                    db.Entry<product>(oProduct).State = System.Data.Entity.EntityState.Deleted;

                }
                else
                {
                    // update rejected
                    var odata = JsonConvert.DeserializeObject<product>(olddata);
                    odata.status = 1;
                    odata.AdminComment = vmproduct.AdminComment;
                    oProduct.AdminComment = vmproduct.AdminComment;
                    oProduct.Aname = odata.Aname;
                    oProduct.CategoryID = odata.CategoryID;
                    oProduct.Description = odata.Description;
                    oProduct.Ename = odata.Ename;
                    oProduct.Image1 = odata.Image1;
                    oProduct.IsService = odata.IsService;
                    oProduct.Price = odata.Price;
                    oProduct.status = odata.status;
                    oProduct.TotalQty = odata.TotalQty;
                    oProduct.UserID = odata.UserID;

                    db.Entry<product>(oProduct).State = System.Data.Entity.EntityState.Modified;

                }


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

    
        [HttpGet]
        public FileResult DownloadFile(string AttchedFile)
        {
            //Build the File Path.
            string path = Server.MapPath("~/Uploads/Services/") + AttchedFile;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", AttchedFile);
        }
    }
}