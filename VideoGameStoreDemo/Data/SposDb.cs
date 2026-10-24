using AdoDotNetTutorial.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VideoGameCharacterApi.Models;

namespace AdoDotNetTutorial.Data
{
    public class SposDb
    {
        //private string _connectionString;

        //public SposDb(string v)
        //{
        //    this._connectionString = "";
        //}

        public string ConnectionString = "";
        public SposDb()
        {
            var builder = new ConfigurationBuilder().SetBasePath(
                Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            ConnectionString = builder.GetConnectionString("SPOSConnection")!;
        }

        public async Task<List<BranchTrnSummary>> GetBranchTrnSummaryUsingReader()
        {
            var summary = new List<BranchTrnSummary>();

            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand("GetBranchTrnSummary", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 120
            };

            command.Prepare();

            await using var reader = await command.ExecuteReaderAsync();
            {
                int trnDateIdx = reader.GetOrdinal("TrnDate");
                int trnTypeIdx = reader.GetOrdinal("TrnType");
                int codeIdx = reader.GetOrdinal("Code");
                int tillIdx = reader.GetOrdinal("Till");
                int zedIdx = reader.GetOrdinal("ZED");
                int inAmtIdx = reader.GetOrdinal("InAmt");
                int outAmtIdx = reader.GetOrdinal("OutAmt");
                int userIdx = reader.GetOrdinal("User");
                int trnTimeIdx = reader.GetOrdinal("TrnTime");
                int docNoIdx = reader.GetOrdinal("DocumentNumber");
                int commentsIdx = reader.GetOrdinal("Comments");
                int docDateIdx = reader.GetOrdinal("DocDate");
                int punchDateIdx = reader.GetOrdinal("PunchDate");
                int profitIdx = reader.GetOrdinal("Profit");
                int pettyCashIdx = reader.GetOrdinal("PettyCashNo");
                int glCodeIdx = reader.GetOrdinal("GLCode");
                int assAssetGlIdx = reader.GetOrdinal("AssAssetGLCode");

                while (await reader.ReadAsync())
                {
                    summary.Add(new BranchTrnSummary
                    {
                        TrnDate = reader.GetDateTime(trnDateIdx),
                        TrnType = reader.GetStringSafe(trnTypeIdx),
                        Code = reader.GetNullable<int>(codeIdx),
                        Till = reader.GetNullable<double>(tillIdx) ?? 0.0,
                        Zed = reader.GetNullable<int>(zedIdx),
                        InAmt = reader.GetNullable<double>(inAmtIdx),
                        OutAmt = reader.GetNullable<double>(outAmtIdx),
                        User = reader.GetNullableString(userIdx),
                        TrnTime = reader.GetStringSafe(trnTimeIdx),
                        DocumentNumber = reader.GetStringSafe(docNoIdx),
                        Comments = reader.GetNullableString(commentsIdx),
                        DocDate = reader.GetNullable<DateTime>(docDateIdx),
                        PunchDate = reader.GetNullable<DateTime>(punchDateIdx),
                        Profit = reader.GetNullable<int>(profitIdx),
                        PettyCashNo = reader.GetNullable<int>(pettyCashIdx),
                        Glcode = reader.GetNullableString(glCodeIdx),
                        AssAssetGlcode = reader.GetNullableString(assAssetGlIdx)
                    });
                }
            }

            return summary;
        }

        public async Task<List<BranchTrnSummary>> GetBranchTrnSummaryUsingDataset(string fromDate, string toDate)
        {
            var summary = new List<BranchTrnSummary>();

            await using var connection = new SqlConnection(ConnectionString);
            await using var command = new SqlCommand("GetBranchTrnSummary", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            Console.WriteLine(fromDate);
            Console.WriteLine(toDate);
            var dataSet = new DataSet();
            var adapter = new SqlDataAdapter(command);
            // Add the parameters required by the stored procedure
            command.Parameters.AddWithValue("@fromDate", fromDate);
            command.Parameters.AddWithValue("@toDate", toDate);
            // SqlDataAdapter has no FillAsync 😐
            await Task.Run(() => adapter.Fill(dataSet));

            if (dataSet.Tables.Count == 0)
                return summary;

            DataTable table = dataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                // Map DB types to model types safely (handle SQL floats, tinyint, and nullable datetimes)
                var tillByte = row.Field<byte?>("Till"); // SQL tinyint -> byte
                summary.Add(new BranchTrnSummary
                {
                    TrnDate = row.Field<DateTime>("TrnDate"),
                    TrnType = row.Field<string>("TrnType") ?? "",
                    Code = row.Field<int?>("Code"),
                    Till = tillByte.HasValue ? Convert.ToDouble(tillByte.Value) : 0.0,
                    Zed = row.Field<int?>("ZED"),
                    InAmt = row.Field<double?>("InAmt"),
                    OutAmt = row.Field<double?>("OutAmt"),
                    User = row.Field<string?>("User"),
                    TrnTime = row.Field<string>("TrnTime") ?? "",
                    DocumentNumber = row.Field<string>("DocumentNumber") ?? "",
                    Comments = row.Field<string?>("Comments"),
                    DocDate = row.Field<DateTime?>("DocDate"),
                    PunchDate = row.Field<DateTime?>("PunchDate"),
                    Profit = row.Field<double?>("Profit"),
                    PettyCashNo = row.Field<int?>("PettyCashNo"),
                    Glcode = row.Field<string?>("GLCode"),
                    AssAssetGlcode = row.Field<string?>("AssAssetGLCode")
                });
            }

            return summary;
        }

        public async Task<List<BranchTrnSummary>> GetAllBranchTrnDapper()
        {
            await using var connection = new SqlConnection(ConnectionString);

            var summary = await connection.QueryAsync<BranchTrnSummary>(
                "GetBranchTrnSummary",
                commandType: CommandType.StoredProcedure
             );

            return summary.AsList();
        }
    }
}
