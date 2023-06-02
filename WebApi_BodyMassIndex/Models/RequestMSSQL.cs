using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApi_BodyMassIndex.Models
{
    public class RequestMSSQL
    {
        static SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-CB1R93J\SQLEXPRESS;Initial Catalog = Hospital; Integrated Security = True;");
        async static public Task AddPatient(string last_name, string first_name, string patronymic, double height, double mass, int age, double BMI)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                await conn.OpenAsync();
       
            string sqlAdd = $"INSERT INTO Patients (last_name, first_name, patronymic, height, mass, age, BMI) values('{last_name}', '{first_name}', '{patronymic}',{height},{mass},{age},{BMI})";
            SqlCommand commandAdd = new SqlCommand(sqlAdd, conn);

            commandAdd.ExecuteScalar();

            if (conn.State == System.Data.ConnectionState.Open)
                await conn.CloseAsync();
        }
    }
}
