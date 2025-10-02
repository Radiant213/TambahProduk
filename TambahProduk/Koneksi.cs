using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoneksiDatabase
{
    internal class Koneksi
    {
        public static string connectionString = @"Data Source=RPL2-C1\SQLEXPRESS;Initial Catalog=TokoDR;Integrated Security=True;Encrypt=False;";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
