﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace Software_Engineering_Project.Controllers
{
    public class HomeController : Controller
    {

        // Login page view method
        //GET
        public IActionResult Login()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                model.IsLoginConfirmed = false;
                return View("Login", model);
            }
            string username = model.Username;
            string password = model.Password;

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select username" +
                ", password , role, salt from users where username = '{0}'", username),conn);

            if (reader.Read())
            {
                byte[] salt = new byte[64];
                string hash = reader.GetString(1);
                string role = reader.GetString(2);
                reader.GetBytes(3, 0, salt, 0, 64);

                conn.Close();

                if (Database.Database.VerifyPassword(password,hash, salt))
                {
                    if(role == "professor")
                    {
                        //Creating and populating the identity cookie with data
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, role)
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        
                        //Sending the cookie to the clients machine
                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                        
                        ViewBag.Username = model.Username;
                        return View("~/Views/Teacher/TeacherHome.cshtml", model);
                    }
                    else
                    {
                        NpgsqlConnection new_conn = Database.Database.GetConnection();
                        NpgsqlDataReader new_reader = Database.Database.ExecuteQuery(String.Format("select has_ever_connected" + 
                            " from student where student = '{0}'", username), new_conn);
                        if (new_reader.Read())
                        {
                            bool has_connected = new_reader.GetBoolean(0);

                            //Creating and populating the identity cookie with data
                            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, role)
                        };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            //Sending the cookie to the clients machine
                            HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity));


                            if (has_connected) 
                            {
                                ViewBag.Username = model.Username;
                                return View("~/Views/Student/StudentHome.cshtml", model.Username);
                            }
                            else
                            {
                                return View("~/Views/Student/SetPassword.cshtml", new StudentProfileModel());
                            }
                        }
                       
                    }
                }
            }
            model.IsLoginConfirmed = false;
            return View("Login", model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

    }
}