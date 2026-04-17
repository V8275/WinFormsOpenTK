using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WinFormsOpenTK
{
    public class DataLoader
    {
        string dbPath = "Data Source=ModelViewerDB.db";

        List<DbModel> models = new List<DbModel>();

        public void LoadData()
        {
            models = GetData();
        }

        public List<DbModel> GetLoadedData()
        {
            return models;
        }

        private List<DbModel> GetData()
        {
            List<DbModel> mod = new List<DbModel>();

            string connectStr = dbPath;
            string query = "SELECT * FROM modelsData";

            using(SqliteConnection connection = new SqliteConnection(connectStr))
            {
                SqliteCommand cmd = new SqliteCommand(query, connection);

                connection.Open();
                SqliteDataReader reader= cmd.ExecuteReader();

                while (reader.Read())
                {
                    DbModel dbModel = new DbModel();
                    (string?, string?, string?, string?, string?, string?) dataForModel =
                    (reader["name"].ToString(),
                        reader["model"].ToString(),
                        reader["texture"].ToString(),
                        reader["format"].ToString(),
                        reader["normal"].ToString(),
                        reader["metallic"].ToString());

                    if (String.IsNullOrEmpty(dataForModel.Item1) ||
                        String.IsNullOrEmpty(dataForModel.Item2) ||
                        String.IsNullOrEmpty(dataForModel.Item3) ||
                        String.IsNullOrEmpty(dataForModel.Item4))
                        throw new Exception("Not Full Data");
                    else
                    {
                        dbModel.SetData(dataForModel);
                        mod.Add(dbModel);
                    }
                }
            }

            return mod;
        }
    }
}
