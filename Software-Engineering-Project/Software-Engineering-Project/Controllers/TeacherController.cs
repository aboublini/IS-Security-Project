﻿using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;

namespace Software_Engineering_Project.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StudentHandler(string username)
        {
            ViewBag.Username = username;
            return View(); 
        }

        public IActionResult Meeting(string username)
        {
            return View();
        }

        public IActionResult AddMeeting(string username)
        {
            return View();
        }

        //GET
        public IActionResult AddStudent(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent(StudentModel model, string proffesorName)
        {
            model.Professor = proffesorName;
            if (ModelState.IsValid)
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("insert into users (username , password,first_name,last_name" +
                    ",gender,email,phone,role) values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}');" +
                    " insert into student (student , start_year, professor) values ('{8}', {9}, {10})",
                    model.Username, model.Password, model.FirstName, model.LastName, model.Gender, model.Email, model.Phone,
                    model.Role, model.Username, model.StartYear, model.Professor), conn);
                if (result != 0)
                {
                    conn.Close();
                    ViewBag.Success = true;
                    return View();
                }
                conn.Close();
            }
            ViewBag.Success = false;
            return View();

        }

        public IActionResult StudentSearch(string queryString, string professorName)
        {
            List<SearchModel> searchModels = new List<SearchModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            if(queryString.Contains(' '))
            {
                string firstName = queryString.Substring(0, queryString.IndexOf(' '));
                string lastName = queryString.Substring(queryString.IndexOf(' ') + 1);

                NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select student" +
                    ", start_year, first_name, last_name, email, phone, title, thesis_start_date, " +
                    "grade, language, technology from thesis natural join (select student, start_year," +
                    " first_name, last_name, email, phone from student as s join users as u on " +
                    "s.student = u.username where first_name='{0}' and last_name='{1}' " +
                    "and professor='{2}') as student",firstName,lastName, professorName), conn);

                while (reader.Read())
                {
                    SearchModel model = new SearchModel();
                    model.Student = reader.GetString(0);
                    model.StartYear = reader.GetInt32(1);
                    model.FirstName = reader.GetString(2);
                    model.LastName = reader.GetString(3);
                    model.Email = reader.GetString(4);
                    model.Phone = reader.GetDecimal(5).ToString();
                    model.Title = reader.GetString(6);
                    model.StartDate = (DateOnly)reader.GetDate(7);
                    model.Grade = reader.GetInt32(8);
                    model.Language = reader.GetString(9);
                    model.Technology = reader.GetString(10);
                    searchModels.Add(model);
                }
                conn.Close();
            }
            else
            {
                NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select student" +
                    ", start_year, first_name, last_name, email, phone, title, thesis_start_date, " +
                    "grade, language, technology from thesis natural join (select student, start_year," +
                    " first_name, last_name, email, phone from student as s join users as u on " +
                    "s.student = u.username where student='{0}' " +
                    "and professor='{1}') as student", queryString, professorName), conn);

                while (reader.Read())
                {
                    SearchModel model = new SearchModel();
                    model.Student = reader.GetString(0);
                    model.StartYear = reader.GetInt32(1);
                    model.FirstName = reader.GetString(2);
                    model.LastName = reader.GetString(3);
                    model.Email = reader.GetString(4);
                    model.Phone = reader.GetDecimal(5).ToString();
                    model.Title = reader.GetString(6);
                    model.StartDate = (DateOnly)reader.GetDate(7);
                    model.Grade = reader.GetInt32(8);
                    model.Language = reader.GetString(9);
                    model.Technology = reader.GetString(10);
                    searchModels.Add(model);
                }
                conn.Close();
            }
            return View(searchModels);
        }
    }
}
