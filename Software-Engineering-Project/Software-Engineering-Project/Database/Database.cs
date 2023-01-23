using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace Software_Engineering_Project.Database
{
    public class Database
    {

        private static readonly string pepper = "901A27086541F6F438BF6C87C03DB39B";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection("Server=localhost;Port=5432;" +
                 "Database=software-engineering-database;User Id=postgres;Password=root;");
        }

        public static NpgsqlDataReader ExecuteQuery(string query, NpgsqlConnection con)
        {
            NpgsqlConnection conn = con;
            NpgsqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public static int ExecuteUpdate(string query, NpgsqlConnection con, byte[]? salt = null)
        {
            NpgsqlConnection conn = con;
            NpgsqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;

            if (salt != null)
            {
                NpgsqlParameter param = cmd.CreateParameter();
                param.ParameterName = "@salt";
                param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                param.Value = salt;
                cmd.Parameters.Add(param);
            }

            int result = cmd.ExecuteNonQuery();
            return result;
        }


        public static string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(64);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password+pepper),
                salt,
                1_000_000,
                HashAlgorithmName.SHA512,
                64);

            return Convert.ToHexString(hash);
        }

        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password+pepper, salt, 1_000_000, HashAlgorithmName.SHA512, 64);

            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }
    }
}
