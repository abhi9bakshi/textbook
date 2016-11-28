using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testbook;
using System.Security.Cryptography;
using BCrypt.Net;
namespace testbook.Controllers
{
    public class AccountController : Controller
    {
        static testbookEntities entities = new testbookEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
    
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            if(collection != null)
            {
                string email = collection.Get(1);
                string password = collection.Get(2);
                var user = entities.Users.Where(e => e.Email == email).FirstOrDefault();
                if(user != null)
                {
                    string hashedPassword = GetHashedPassword(password,user.PasswordSalt);
                    if(hashedPassword == user.HashedPassword)
                    {
                        //Login sucessful
                        user.LastAccessDateTime = DateTime.Now;
                        Session["CurrentUserId"] = user.UserId;
                    }
                    else
                    {
                        //login failure
                        return RedirectToAction("Login","Account");
                    }
                }
                else
                {
                    //login failure
                    return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            if (collection != null)
            {
                User newuser = new User();
                newuser.FirstName = collection.Get(1);          //first name
                newuser.LastName = collection.Get(2);           //last name
                newuser.Email = collection.Get(3);              //email 
                string password = collection.Get(4);            //password

                newuser.CreationDateTime = DateTime.Now;
                newuser.LastAccessDateTime = DateTime.Now;
                newuser.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(12);                      //generate salt 
                newuser.HashedPassword = GetHashedPassword(password , newuser.PasswordSalt);    //get hash of password
                try
                {
                    entities.Users.Add(newuser);
                    entities.SaveChanges();
                }
                catch(Exception)
                {
                    //return to index page with custom error message
                    return RedirectToAction("Index", "Home");
                }
                //return to index page with message "Account created successfully"
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Register", "Account");
        }
        public ActionResult SignOut()
        {
            Session["CurrentUserId"] = null;
            return RedirectToAction("Index","Home");
        }

        /*
        generates hash of password based on input password and salt value
        */
        private string GetHashedPassword(string password,string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);   
        }
    }
}