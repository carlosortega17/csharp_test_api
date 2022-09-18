using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace Movpinion_api.Models
{
    public class Database
    {
        public static SQLiteConnection connection;

        public static void Startup(string dbName)
        {
            connection = new SQLiteConnection(string.Format("Data Source={0}.db", dbName));
            connection.Open();
            ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS Movies(
id INTEGER PRIMARY KEY AUTOINCREMENT,
name VARCHAR(120) NOT NULL,
description TEXT NOT NULL
)");
        }

        public static dynamic ExecuteQuery(string command, string tname)
        {
            DataSet data = new DataSet();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, connection);
            adapter.Fill(data);
            return data.Tables[0];
        }

        public static void ExecuteNonQuery(string command)
        {
            new SQLiteCommand(command, connection).ExecuteNonQuery();
        }
    }
}
