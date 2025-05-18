using System.Data;
using Npgsql;
using ExportDemoApp.Models;
using System.Collections.Generic;

namespace ExportDemoApp.Repositories
{
    public class LaporanRepository
    {
        private readonly string _connectionString;

        public LaporanRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<LaporanModel> GetLaporan()
        {
            var result = new List<LaporanModel>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM get_laporan_data()", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new LaporanModel
                {
                    Id = reader.GetInt32(0),               // id integer
                    Nama = reader.GetString(1),            // nama text/varchar
                    Tanggal = reader.GetDateTime(2)        // tanggal date
                });
            }

            return result;
        }
    }

}
