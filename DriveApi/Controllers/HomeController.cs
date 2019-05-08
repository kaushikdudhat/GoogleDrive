using DriveApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DriveApi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult GetGoogleDriveFiles()
        {
            return View(GoogleDriveFilesRepository.GetDriveFiles());
        }
        
        [HttpGet]
        public ActionResult GetGoogleDriveFiles1()
        {
            return View(GoogleDriveFilesRepository.GetDriveFiles());
        }


        [HttpGet]
        public ActionResult GetGoogleDriveFiles2()
        {
            return View(GoogleDriveFilesRepository.GetDriveFiles());
        }

        [HttpPost]
        public ActionResult DeleteFile(GoogleDriveFile file)
        {
            GoogleDriveFilesRepository.DeleteFile(file);
            return RedirectToAction("GetGoogleDriveFiles");
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            GoogleDriveFilesRepository.FileUpload(file);
            return RedirectToAction("GetGoogleDriveFiles");
        }

        public void DownloadFile(string id)
        {
            string FilePath = GoogleDriveFilesRepository.DownloadGoogleFile(id);
                
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles/" + Path.GetFileName(FilePath)));
            Response.End();
            Response.Flush();
        }
        

        [HttpGet]
        public ActionResult GetContainsInFolder(string folderId)
        {
            return View(GoogleDriveFilesRepository.GetContainsInFolder(folderId));
        }

        [HttpPost]
        public ActionResult CreateFolder(String FolderName)
        {
            GoogleDriveFilesRepository.CreateFolder(FolderName);
            return RedirectToAction("GetGoogleDriveFiles1");
        }

        
        [HttpPost]
        public ActionResult FileUploadInFolder(GoogleDriveFile FolderId, HttpPostedFileBase file)
        {
            GoogleDriveFilesRepository.FileUploadInFolder(FolderId.Id, file);
            return RedirectToAction("GetGoogleDriveFiles1");
        }
        

        [HttpGet]
        public JsonResult FolderLists()
        {
            List<GoogleDriveFile> AllFolders = GoogleDriveFilesRepository.GetDriveFolders();
            List<DDLOptions> obj = new List<DDLOptions>();

            foreach (GoogleDriveFile EachFolder in AllFolders)
            {
                obj.Add(new DDLOptions { Id = EachFolder.Id, Name = EachFolder.Name });
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public string MoveFiles(String fileId, String folderId)
        {
            string Result = GoogleDriveFilesRepository.MoveFiles(fileId, folderId);
            return Result;
        }

        public string CopyFiles(String fileId, String folderId)
        {
            string Result = GoogleDriveFilesRepository.CopyFiles(fileId, folderId);
            return Result;
        }

        public JsonResult Render_GetGoogleDriveFilesView()
        {
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            var result = GoogleDriveFilesRepository.GetDriveFiles();
            jsonResult.Add("Html", RenderRazorViewToString("~/Views/Home/GetGoogleDriveFiles1.cshtml", result));
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;

            }
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}