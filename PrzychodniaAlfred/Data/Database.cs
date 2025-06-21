// tu byłby poważny kod ALE HOSTING NIE OBSŁUGUJE POŁĄCZEŃ WIĘC POSZLIŚMY W PHP
//using MySql.Data.MySqlClient;
//using System;
//using System.Data;

//namespace PrzychodniaAlfred.Data
//{
//    public static class Database
//    {
        
//        private static readonly string connStr = "Server=mysql1.small.pl;Database=przychDB;Uid=m2157_przychDB;Pwd=Ewolucja2024!;";

//        public static MySqlConnection GetConnection()
//        {
//            var conn = new MySqlConnection(connStr);
//            conn.Open();
//            return conn;
//        }

//        public static DataRow GetUser(string login, string haslo)
//        {
//            using var conn = GetConnection();
//            string query = "SELECT * FROM Uzytkownicy WHERE Login = @login AND HasloHash = SHA2(@haslo, 256)";
//            using var cmd = new MySqlCommand(query, conn);
//            cmd.Parameters.AddWithValue("@login", login);
//            cmd.Parameters.AddWithValue("@haslo", haslo);

//            using var adapter = new MySqlDataAdapter(cmd);
//            var dt = new DataTable();
//            adapter.Fill(dt);

//            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
//        }
//    }
//}


