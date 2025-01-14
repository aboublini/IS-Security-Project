﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;
using System.Text.RegularExpressions;

namespace Software_Engineering_Project.Controllers
{
    [Authorize(Roles = "professor")]
    public class TeacherController : Controller
    {
        public IActionResult TeacherHome(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //GET
        public IActionResult StudentHandler(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        public IActionResult AddMeeting(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMeeting(MeetingModel model, string proffesorName)
        {
            model.Professor = proffesorName;
            if (ModelState.IsValid)
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("insert into meeting (professor, student, type" +
                    ", duration, title, meet_date) values ('{0}','{1}','{2}','{3}','{4}','{5}');",
                   model.Professor, model.Student, model.Type, model.Duration, model.Title, model.DateTime.ToString("yyyy-M-dd hh:mm:ss")), conn);
                if (result != 0)
                {
                    conn.Close();
                    ViewBag.Username = proffesorName;
                    ViewBag.Success = true;
                    return View();
                }
                conn.Close();
            }
            ViewBag.Success = false;
            ViewBag.Username = proffesorName;
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
        public IActionResult AddStudent(ThesisModel model, string proffesorName)
        {
            model.Professor = proffesorName;
            if (ModelState.IsValid)
            {
                string hash = Database.Database.HashPasword(model.Password, out var salt);
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("insert into users (username , password,first_name,last_name" +
                    ",gender,email,phone,role) values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}');" +
                    " insert into student (student , start_year, professor) values ('{8}', {9}, '{10}');" +
                    " insert into thesis (professor, student, title, thesis_start_date, grade, language, technology) " +
                    "values ('{11}', '{12}', '{13}', '{14}', -1, '{15}', '{16}');",
                    model.Username, hash, model.FirstName, model.LastName, model.Gender, model.Email, model.Phone,
                    model.Role, model.Username, model.StartYear, model.Professor, model.Professor, model.Username, model.Title, model.ThesisStartDate, model.Language, model.Technology), conn);
                    conn.Close();

                int saltResult = Database.Database.ExecuteUpdate(String.Format("update users set salt=@salt " +
                        " where username='{0}'", model.Username), conn, salt);
                conn.Close();

                if (result != 0 && saltResult != 0)
                {
                    ViewBag.Success = true;
                    ViewBag.Username = proffesorName;
                    return View();
                }
            }
            ViewBag.Username = proffesorName;
            ViewBag.Success = false;
            return View();

        }

        public IActionResult SearchMeetingStudent(string Username)
        {
            List<ThesisModel> searchModels = new List<ThesisModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("SELECT student, first_name, last_name" +
                "FROM student NATURAL JOIN users where professor = 'professor'"), conn);

            while (reader.Read())
            {
                ThesisModel model = new ThesisModel();
                model.Username = reader.GetString(0);
                model.FirstName = reader.GetString(1);
                model.LastName = reader.GetString(2);

                searchModels.Add(model);
            }
            conn.Close();
            ViewBag.Username = Username;
            return View("AddMeeting", searchModels);

        }

        //GET
        public IActionResult Meeting(string Username)
        {
            ViewBag.Username = Username;
            return View("Meeting");
        }


        //POST
        public IActionResult SearchMeeting(string selected_month, string selected_day, string Username)
        {
            List<MeetingModel> meetingModels = new List<MeetingModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("SELECT student, " +
                "type, duration, title, meet_date FROM meeting WHERE professor='{0}' "
                + "and EXTRACT(month FROM meet_date)='{2}' and EXTRACT(day FROM meet_date)='{1}'", Username, selected_day, selected_month), conn);

            while (reader.Read())
            {
                MeetingModel model = new MeetingModel();
                model.Student = reader.GetString(0);
                model.Type = reader.GetString(1);
                model.Duration = reader.GetString(2);
                model.Title = reader.GetString(3);
                model.DateTime = reader.GetDateTime(4);

                meetingModels.Add(model);
            }
            ViewBag.popup = true;
            ViewBag.Username = Username;
            conn.Close();
            return View("Meeting", meetingModels);
        }
    


    public IActionResult StudentSearch(string queryString, string professorName)
        {
            if (queryString == null)
            {
                ViewBag.Fail = true;
                ViewBag.Username = professorName;
                return View("StudentHandler");
            }

            if(!(Regex.Match(professorName, @"^\w*$").Success || Regex.Match(queryString, @"^(?=.*?\s)\w*$").Success))
            {
                ViewBag.Fail = true;
                ViewBag.Username = professorName;
                return View("StudentHandler");
            }
            List<ThesisModel> searchModels = new List<ThesisModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            if (queryString.Contains(' '))
            {
                string firstName = queryString.Substring(0, queryString.IndexOf(' '));
                string lastName = queryString.Substring(queryString.IndexOf(' ') + 1);

                NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select student" +
                    ", start_year, first_name, last_name, email, phone, title, thesis_start_date, " +
                    "grade, language, technology from thesis natural join (select student, start_year," +
                    " first_name, last_name, email, phone from student as s join users as u on " +
                    "s.student = u.username where first_name='{0}' and last_name='{1}' " +
                    "and professor='{2}') as student", firstName, lastName, professorName), conn);

                while (reader.Read())
                {
                    ThesisModel model = new ThesisModel();
                    model.Username = reader.GetString(0);
                    model.StartYear = reader.GetInt32(1);
                    model.FirstName = reader.GetString(2);
                    model.LastName = reader.GetString(3);
                    model.Email = reader.GetString(4);
                    model.Phone = reader.GetDecimal(5).ToString();
                    model.Title = reader.GetString(6);
                    model.ThesisStartDate = (DateOnly)reader.GetDate(7);
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
                    ThesisModel model = new ThesisModel();
                    model.Username = reader.GetString(0);
                    model.StartYear = reader.GetInt32(1);
                    model.FirstName = reader.GetString(2);
                    model.LastName = reader.GetString(3);
                    model.Email = reader.GetString(4);
                    model.Phone = reader.GetDecimal(5).ToString();
                    model.Title = reader.GetString(6);
                    model.ThesisStartDate = (DateOnly)reader.GetDate(7);
                    model.Grade = reader.GetInt32(8);
                    model.Language = reader.GetString(9);
                    model.Technology = reader.GetString(10);
                    searchModels.Add(model);
                }
                conn.Close();
            }
            if (searchModels.Count < 1)
            {
                ViewBag.Fail = true;
                ViewBag.Username = professorName;
                return View("StudentHandler");
            }
            ViewBag.Username = professorName;
            return View(searchModels);
        }

        //GET
        public IActionResult Profile(string username)
        {
            ProfessorProfileModel model = new();

            NpgsqlConnection conn = Database.Database.GetConnection();

            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, email, phone" +
                ", office_address, technology,language from professor as p join users as u on p.professor=u.username" +
                " where professor='{0}'",username), conn);

            if (reader.Read())
            {
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                model.Email = reader.GetString(2);
                model.Phone = reader.GetDecimal(3).ToString();
                model.OfficeAddress = reader.GetString(4);
                model.Technology = reader.GetString(5);
                model.Language = reader.GetString(6);
                model.Username = username;
            }

            ViewBag.Username = username;
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ProfessorProfileModel profileModel)
        {
            string professorName = profileModel.Username;
            ViewBag.wrongPassword = 0;
            ViewBag.notSamePasswords = 0;
            ViewBag.success = 0;
            ViewBag.failure = 0;
            ViewBag.emptyPassword = 0;


            if (!ModelState.IsValid)
            {
                ProfessorProfileModel model = new();

                NpgsqlConnection conn1 = Database.Database.GetConnection();

                NpgsqlDataReader reader1 = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, email, phone" +
                    ", office_address, technology,language from professor as p join users as u on p.professor=u.username" +
                    " where professor='{0}'", professorName), conn1);

                if (reader1.Read())
                {
                    model.FirstName = reader1.GetString(0);
                    model.LastName = reader1.GetString(1);
                    model.Email = reader1.GetString(2);
                    model.Phone = reader1.GetDecimal(3).ToString();
                    model.OfficeAddress = reader1.GetString(4);
                    model.Technology = reader1.GetString(5);
                    model.Language = reader1.GetString(6);
                    model.Username = professorName;
                }

                ViewBag.failure = 1;
                ViewBag.Username = professorName;
                return View("Profile", model);
            }
            else
            {
                string hash = "";
                byte[] salt = new byte[64];

                NpgsqlConnection conn = Database.Database.GetConnection();
                NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select username, password, salt " +
                                                            "from users where username='{0}'", professorName), conn);
                while (reader.Read())
                {
                    hash = reader.GetString(1);
                    reader.GetBytes(2, 0, salt, 0, 64);
                }
                conn.Close();

                if (!Database.Database.VerifyPassword(profileModel.Password, hash, salt))
                {
                    ViewBag.wrongPassword = 1;
                }
                else
                {
                    if (profileModel.NewPassword != profileModel.NewPassword1)
                    {
                        ViewBag.notSamePasswords = 1;
                    }
                    else
                    {
                        int result = Database.Database.ExecuteUpdate(String.Format("update users set password='{0}'" +
                            " where username='{1}'", Database.Database.HashPasword(profileModel.NewPassword.Trim(), out var newSalt), professorName), conn);
                        conn.Close();

                        int saltResult = Database.Database.ExecuteUpdate(String.Format("update users set salt=@salt " +
                            " where username='{0}'", professorName), conn, newSalt);

                        if (result == 1 && saltResult == 1)
                        {
                            ViewBag.success = 1;
                        }
                        else
                        {
                            ViewBag.failure = 1;
                        }
                    }
                }
                conn.Close();

                ProfessorProfileModel model = new();

                NpgsqlConnection conn1 = Database.Database.GetConnection();

                NpgsqlDataReader reader1 = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, email, phone" +
                    ", office_address, technology,language from professor as p join users as u on p.professor=u.username" +
                    " where professor='{0}'", professorName), conn1);

                if (reader1.Read())
                {
                    model.FirstName = reader1.GetString(0);
                    model.LastName = reader1.GetString(1);
                    model.Email = reader1.GetString(2);
                    model.Phone = reader1.GetDecimal(3).ToString();
                    model.OfficeAddress = reader1.GetString(4);
                    model.Technology = reader1.GetString(5);
                    model.Language = reader1.GetString(6);
                    model.Username = professorName;
                }
                ViewBag.Username = professorName;
                return View("Profile", model);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePhone(ProfessorProfileModel profileModel)
        {
            string professorName = profileModel.Username;

            if (ModelState.IsValid && (profileModel.Phone == profileModel.NewPhone))
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("update users set phone='{0}' " +
                                                        "where username='{1}'", profileModel.NewPhone, professorName), conn);
                if (result == 1)
                {
                    ViewBag.success = 1;
                }
                else
                {
                    ViewBag.failure = 1;
                }
                conn.Close();
            }
            else
            {
                ViewBag.failure = 1;
            }

            ProfessorProfileModel model = new();

            NpgsqlConnection conn1 = Database.Database.GetConnection();

            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, email, phone" +
                ", office_address, technology,language from professor as p join users as u on p.professor=u.username" +
                " where professor='{0}'", professorName), conn1);

            if (reader.Read())
            {
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                model.Email = reader.GetString(2);
                model.Phone = reader.GetDecimal(3).ToString();
                model.OfficeAddress = reader.GetString(4);
                model.Technology = reader.GetString(5);
                model.Language = reader.GetString(6);
                model.Username = professorName;
            }

            ViewBag.Username = professorName;
            return View("Profile",model);
        }

        public IActionResult GradeList(string username)
        {
            List<ThesisModel> models = new();

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, grade " +
                "from users as u join(select student, grade from thesis where professor='{0}' order by grade desc) as foo" +
                " on u.username = foo.student", username), conn);

            while (reader.Read())
            {
                ThesisModel model = new();
                model.Professor = username;
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                model.Grade = reader.GetInt32(2);
                models.Add(model);
            }
            conn.Close();
            ViewBag.Username = username;
            return View(models);
        }

        public IActionResult ThesisStartList(string username)
        {
            List<ThesisModel> models = new();

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, thesis_start_date " +
                "from users as u join(select student, thesis_start_date from thesis where professor='{0}' " +
                "order by thesis_start_date) as foo on u.username = foo.student", username), conn);

            while (reader.Read())
            {
                ThesisModel model = new();
                model.Professor = username;
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                if (model.ThesisStartDate != null)
                {
                    model.ThesisStartDate = (DateOnly)reader.GetDate(2);
                }
                else
                {
                    model.ThesisStartDate = new DateOnly();
                }
                
                models.Add(model);
            }
            conn.Close();
            ViewBag.Username = username;
            return View(models);
        }

        public IActionResult UngradedStudents(string username)
        {
            List<ThesisModel> models = new();

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select student, title, thesis_start_date, language, technology" +
                " from student as s natural join (select * from thesis where grade = -1 and professor = '{0}') as t", username), conn);

            while (reader.Read())
            {
                ThesisModel model = new();
                model.Username = reader.GetString(0);
                model.Title = reader.GetString(1);
                model.ThesisStartDate = (DateOnly)reader.GetDate(2);
                model.Language = reader.GetString(3);
                model.Technology = reader.GetString(4);
                models.Add(model);
            }
            conn.Close();

            ViewBag.Username = username;
            return View(models);
        }

        public IActionResult Grade(string username, string student)
        {

            Dictionary<string, byte[]?> uploads = new Dictionary<string, byte[]?>() { { "upload1", null }, { "upload2", null }, { "upload3", null } };

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select upload1, upload2, upload3 from thesis " +
                "where professor='{0}' and student='{1}'", username, student), conn);

            if (reader.Read())
            {
                if (!Convert.IsDBNull(reader[0]))
                {
                    uploads["upload1"] = (byte[]?)reader[0];
                }
                if (!Convert.IsDBNull(reader[1]))
                {
                    uploads["upload2"] = (byte[]?)reader[1];
                }
                if (!Convert.IsDBNull(reader[2]))
                {
                    uploads["upload3"] = (byte[]?)reader[2];
                }
            }

            ViewBag.Username = username;
            ViewBag.Student = student;
            return View(uploads);

        }

        public FileResult Download(string username, string student, string filename)
        {
            byte[] file = null;

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select {0} from thesis " +
                "where professor='{1}' and student='{2}'", filename, username, student), conn);

            if (reader.Read())
            {
                file = (byte[])reader[0];
            }

            return File(file, "application/zip", filename + ".zip");

        }

        public IActionResult FinalGrade(string username, string student, int grade)
        {
            if (Regex.Match(username, @"^\w*$").Success && Regex.Match(student, @"^\w*$").Success && Regex.Match(grade.ToString(), @"^\d{10}$").Success)
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("update thesis set grade = {0} where student = '{1}'", grade, student), conn);
            }
            else
            {
                ViewBag.failure = 1;
                ViewBag.Username = username;
                return View("Grade");
            }
            ViewBag.Username = username;
            return View("StudentHandler");
        }
    }
}
