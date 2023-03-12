using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientsInfo> listClients = new List<ClientsInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                ClientsInfo info = new ClientsInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.name = reader.GetString(1);
                                info.email = reader.GetString(2);
                                info.phone = reader.GetString(3);
                                info.address = reader.GetString(4);
                                info.created_at = reader.GetDateTime(5).ToString();

                                listClients.Add(info);
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
  
    public class ClientsInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
}
