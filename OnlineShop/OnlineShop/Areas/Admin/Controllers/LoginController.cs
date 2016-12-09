using Model.Dao;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName,Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetByID(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");

                }
                else if(result == 0)
                {
                    ModelState.AddModelError("","User does not exist");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("","User is locked");
                }
                else if(result == -2)
                {
                    ModelState.AddModelError("", "Password is incorrect");
                }
                else
                {
                    ModelState.AddModelError("", "Cannot Login");
                }
            }
                return View("Index");
        }
        
    }
}