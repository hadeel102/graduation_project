using Newtonsoft.Json;
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
    public class ServiceController : Controller
    {
        [HttpPost]
        public ActionResult Search(FormCollection oform)
        {

            var SearchEname = string.Format("{0}", oform["SearchEname"]).Trim();
            var SearchPrice = string.Format("{0}", oform["SearchPrice"]).Trim();
            var dSearchPrice = (SearchPrice == "") ? 0 : decimal.Parse(SearchPrice);
            using (var db = new ECSalesEntities2())
            {
                var UserID = (Session["UserInfo"] as TblUser).ID;
                ViewBag.list = db.products.Include("TblCategory").Where(x => x.IsService == true && x.UserID == UserID
                 && (SearchEname == "" || x.Ename.Contains(SearchEname))
                   && (SearchPrice == "" || x.Price == dSearchPrice)
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
        // GET: Dashboard
        public ActionResult Index()
        {
            using (var db = new ECSalesEntities2())
            {
                var UserID = (Session["UserInfo"] as TblUser).ID;
                ViewBag.list = db.products.Include("TblCategory").Where(x => x.IsService == true && x.UserID == UserID).ToList();
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
        public ActionResult GetById(int id)
        {

            product oProduct = new product();
            using (var db = new ECSalesEntities2())
            {
                oProduct = db.products.Where(x => x.ID == id).FirstOrDefault();

            }

            return Json(new
            {
                data = new product
                {
                    Aname = oProduct.Aname,
                    ID = oProduct.ID,
                    CategoryID = oProduct.CategoryID,
                    Description = oProduct.Description,
                    Ename = oProduct.Ename,
                    Image1 = oProduct.Image1,
                    IsService = oProduct.IsService,
                    Price = oProduct.Price,
                    TotalQty = oProduct.TotalQty
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProduct(vmproduct oproduct)
        {
            using (var db = new ECSalesEntities2())
            {
                if (oproduct.ID <= 0)
                {
                    db.products.Add(new product()
                    {
                        Aname = oproduct.Aname,
                        CategoryID = oproduct.CategoryID,
                        Description = oproduct.Description,
                        Ename = oproduct.Ename,
                        ID = oproduct.ID,
                        Image1 = oproduct.ImageFile.FileName,
                        IsService = true,
                        Price = oproduct.Price,
                        UserID = (Session["UserInfo"] as TblUser).ID,
                        TotalQty = oproduct.TotalQty,
                        status = 0
                    });
                    string UploadPath = string.Empty;
                    //Use Namespace called :  System.IO  
                    string FileName = Path.GetFileNameWithoutExtension(oproduct.ImageFile.FileName);

                    //To Get File Extension  
                    string FileExtension = Path.GetExtension(oproduct.ImageFile.FileName);

                    UploadPath = Server.MapPath("~/Uploads/Services/") + FileName + FileExtension;

                    oproduct.ImageFile.SaveAs(UploadPath);
                    db.productHistories.Add(new productHistory()
                    {
                        Aname = oproduct.Aname,
                        CategoryID = oproduct.CategoryID,
                        Description = oproduct.Description,
                        Ename = oproduct.Ename,
                        ID = oproduct.ID,
                        Image1 = oproduct.ImageFile.FileName,
                        IsService = true,
                        Price = oproduct.Price,
                        UserID = (Session["UserInfo"] as TblUser).ID,
                        TotalQty = oproduct.TotalQty,
                        status = 0
                    });


                }
                else
                {
                    //Update 
                    var op = db.products.Where(x => x.ID == oproduct.ID).FirstOrDefault();
                    string olddata = JsonConvert.SerializeObject(new product()
                    {
                        AdminComment = op.AdminComment,
                        Aname = op.Aname,
                        CategoryID = op.CategoryID,
                        Description = op.Description,
                        Ename = op.Ename,
                        ID = op.ID,
                        Image1 = op.Image1,
                        IsService = op.IsService,
                        Price = op.Price,
                        status = op.status,
                        TotalQty = op.TotalQty,
                        UserID = op.UserID
                    }, Formatting.None, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    op.Aname = oproduct.Aname;
                    op.CategoryID = oproduct.CategoryID;
                    op.Description = oproduct.Description;
                    op.Ename = oproduct.Ename;
                    op.Price = oproduct.Price;
                    op.TotalQty = oproduct.TotalQty;
                    op.status = 0;
                    op.OldData = olddata;
                    if (oproduct.ImageFile != null && oproduct.ImageFile.FileName != null)
                    {
                        string UploadPath = string.Empty;
                        //Use Namespace called :  System.IO  
                        string FileName = Path.GetFileNameWithoutExtension(oproduct.ImageFile.FileName);

                        //To Get File Extension  
                        string FileExtension = Path.GetExtension(oproduct.ImageFile.FileName);

                        UploadPath = Server.MapPath("~/Uploads/Services/") + FileName + FileExtension;

                        oproduct.ImageFile.SaveAs(UploadPath);

                        op.Image1 = oproduct.ImageFile.FileName;
                        op.UserID = (Session["UserInfo"] as TblUser).ID;
                        db.Entry<product>(op).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.productHistories.Add(new productHistory()
                    {
                        Aname = op.Aname,
                        CategoryID = op.CategoryID,
                        Description = op.Description,
                        Ename = op.Ename,

                        Image1 = op.Image1,
                        IsService = true,
                        Price = op.Price,
                        UserID = op.UserID,
                        TotalQty = op.TotalQty,
                        status = 0
                    });
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {


                }


            }


            return Redirect("Index");
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