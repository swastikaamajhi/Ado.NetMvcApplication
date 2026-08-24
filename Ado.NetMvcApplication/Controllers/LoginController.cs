using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Ado.NetMvcApplication.Models;

namespace Ado.NetMvcApplication.Controllers
{
    public class LoginController : Controller
    {
        string cs = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        // SHOW DATA
        public ActionResult Index()
        {
            List<LoginModel> list = new List<LoginModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT Username, Password FROM Users";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new LoginModel
                    {
                        Username = dr["Username"].ToString(),
                        Password = dr["Password"].ToString()
                    });
                }
            }

            return View(list);
        }

        // INSERT DATA
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Users VALUES(@u,@p)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@u", model.Username);
                cmd.Parameters.AddWithValue("@p", model.Password);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}