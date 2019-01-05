using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Invasion
{

    class LeaderBoard
    {
        public static bool isUpdated = false;

        private static string ConnectionString = "Data Source=C:\\Users\\user\\Documents\\Invasion\\invasion.db;Version=3;";
       
        public static void queryDb(string query)
        {
            SQLiteConnection conn;
            conn = new SQLiteConnection(LeaderBoard.ConnectionString);
            conn.Open();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Dispose();
            //SQLiteDataReader reader = command.ExecuteReader();

            //while (reader.Read())
            //    Console.WriteLine(reader["player_name"]);



        }
        public static void updateTotalGames(string player)
        {
            string sqlUpdateQueryTemplate = @"INSERT OR IGNORE into player_stats (player_name) VALUES ('{0}');
                                      UPDATE player_stats SET total_games = total_games + 1 WHERE player_name='{0}';";
            string sqlUpdateQuery = String.Format(sqlUpdateQueryTemplate, player);

           Console.WriteLine(String.Format("Updating {0} total_games", player));
           LeaderBoard.queryDb(sqlUpdateQuery);
        }

        public static void updateTotalWins(string player)
        {
            string sqlUpdateQueryTemplate = @"INSERT OR IGNORE INTO player_stats (player_name) VALUES ('{0}');
                                      UPDATE player_stats SET total_wins = total_wins + 1 WHERE player_name='{0}';";
            string sqlUpdateQuery = String.Format(sqlUpdateQueryTemplate, player);

            LeaderBoard.queryDb(sqlUpdateQuery);
        }


    }
}
