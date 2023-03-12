using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
		public ClientsInfo info = new ClientsInfo();
		public string errorMessage = "";
		public string successMessage = "";

		public void OnGet()
        {
			string id = Request.Query["id"];

			try
            {
				string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM clients WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							
							while (reader.Read())
							{
								info.id = "" + reader.GetInt32(0);
								info.name = reader.GetString(1);
								info.email = reader.GetString(2);
								info.phone = reader.GetString(3);
								info.address = reader.GetString(4);
							}
						}
					}
				}
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
			info.id = Request.Query["id"];	
			info.name = Request.Form["name"];
			info.email = Request.Form["email"];
			info.phone = Request.Form["phone"];
			info.address = Request.Form["address"];

			/* errorMessage = (info.name.Length == 0 || info.email.Length == 0 || info.phone.Length == 0 || info.address.Length == 0) ?
				 "All fields are required" : "Data Save successfully";*/
			if (info.id.Length == 0 || info.name.Length == 0 || info.email.Length == 0 ||
				info.phone.Length == 0 || info.address.Length == 0)
			{
				errorMessage = "All fields are required";
				return;
			}
			try
			{
				string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "UPDATE clients SET name=@name, email=@email, phone=@phone, address=@address WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", info.name);
						command.Parameters.AddWithValue("@email", info.email);
						command.Parameters.AddWithValue("@phone", info.phone);
						command.Parameters.AddWithValue("@address", info.address);
						command.Parameters.AddWithValue("@id", info.id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			Response.Redirect("/Clients/Index");
		}
    }
}
