using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["loginSession"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            User user  = new User();
            user.Name = fc["username"];
            user.Password = fc["password"];

            if (Session["loginSession"] == null)
            {
                Session["loginSession"] = user.Name;
            }

            SqlConnection con = null;
            SqlDataReader dr = null;
            try
            {
                string connectionString = GetConnectionString();

                con = new SqlConnection(connectionString);

                con.Open();
                string str = $"select * from Customer where Username = " + $" '{user.Name}' and Password = '{user.Password}'";
                SqlCommand cmd = new SqlCommand(str, con);

                dr = cmd.ExecuteReader();

                if (ModelState.IsValid)
                {
                    if (dr.HasRows)
                    {
                        TempData["message"] = "Login Successful";
                        TempData.Keep("message");
                        return RedirectToAction("BankPage", user);
                    }
                    else
                    {
                        ViewBag.attempt = "Login Failed. Please try again or if you are new then please Register";
                    }
                }

            }
            finally
            {
                // close reader
                if (dr != null)
                {
                    dr.Close();
                }
                // close connection
                if (con != null)
                {
                    con.Close();
                }
            }
            return View();
        }
        protected string GetConnectionString()
        {
            var datasource = @"LAPTOP-DNBGPDGU\Luke";//your server
            var database = "BankSystemdb"; //your database name
            var username = "Luke"; //username of server to connect
            var password = "County2002"; //password
                                         //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            return connString;
        }
    }
}