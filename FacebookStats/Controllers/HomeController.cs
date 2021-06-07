using FacebookStats.Models;
using FacebookStats.Models.FacebookModels;
using FacebookStats.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FacebookStats.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var result = SQLHealper.GetPostWallInstances();
            return View(result);
        }

        public ActionResult Preview(int id)
        {
            var result = SQLHealper.GetPostWallInstances().Where(d=> d.Id == id).FirstOrDefault();
            if (result != null)
            {
                var preview = FBServices.GetPreview(result.AdId);
                ViewBag.Preview = preview;
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult CampaignData()
        {
            var accountInfo = FBServices.GetAccountInfo();
            ViewBag.accountInfo = accountInfo;
            return View("Index");
        }


        [HttpGet]
        public ActionResult PostOnWall()
        {
            return View();
        }



        [HttpPost]
        public ActionResult PostOnWall(PostWallInstance postWallInstance)
        {
            if (postWallInstance.IsValid())
            {
                try
                {
                    int id = SQLHealper.Insert(postWallInstance);
                    var result = FBServices.CreateCampaignAndAd(new CampaignWallInstance(postWallInstance));
                    postWallInstance.Id = id;
                    postWallInstance.AdId = result.AdID;
                    var updateData = SQLHealper.Update(postWallInstance);
                    return Json(new { isValid = true, result = result });
                }
                catch (Exception exp)
                {
                    return Json(new { isValid = false, result = exp.Message });
                }
            }
            return Json(new { isValid = false, result = "Data is not complete" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string _imgname = string.Empty;
            string _fileName = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    _fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("/Upload/MVC_") + _imgname + _ext;
                    _imgname = "MVC_" + _imgname + _ext;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                }
            }
            return Json(new { imagePath = Convert.ToString(_imgname), filename = _fileName }, JsonRequestBehavior.AllowGet);
        }

    }
}