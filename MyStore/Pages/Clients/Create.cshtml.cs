using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientsInfo info = new ClientsInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        { 
            info.name = Request.Form["name"];
            info.email = Request.Form["email"];
            info.phone = Request.Form["phone"];
            info.address = Request.Form["address"];

			/* errorMessage = (info.name.Length == 0 || info.email.Length == 0 || info.phone.Length == 0 || info.address.Length == 0) ?
				 "All fields are required" : "Data Save successfully";*/
			if (info.name.Length == 0 || info.email.Length == 0 ||
                info.phone.Length == 0 || info.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
			}
            // Save the new client into database
            try
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients " +
                        "(name, email, phone, address) VALUES " +
                        "(@name, @email, @phone, @address)";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", info.name);
                        command.Parameters.AddWithValue ("@email", info.email);
                        command.Parameters.AddWithValue("@phone", info.phone);
                        command.Parameters.AddWithValue("@address", info.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            info.name = ""; info.email = ""; info.phone = ""; info.address = "";
            successMessage = "New client Added Successfully";

            Response.Redirect("/Clients/Index");
		}
    }
}
