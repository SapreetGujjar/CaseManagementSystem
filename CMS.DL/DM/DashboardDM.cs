using CMS.DL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.DM
{
    public class DashboardDM
    {
        private readonly IDbConnection _dbConnection;

        public DashboardDM(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        #region GET

        public async Task<IEnumerable<Dashboard>> GetDashBoardDataList(Guid _currentLoginUser)
        {
            string sql = "declare  @StatusCount int = null;\r\ndeclare  @Statustype int = null;\r\nselect @StatusCount =count(*) ,@Statustype= Status from Cases c \r\nwhere c.Status in (2, 5, 7) and c.EndClient = @Id \r\ngroup by Status\r\nselect isnull(@StatusCount, 0) as StatusCount , isnull(@Statustype, 0) Statustype\r\n";
            var CasesCount = await _dbConnection.QueryAsync<Dashboard>(sql , new { Id = _currentLoginUser});
            return CasesCount.ToList();
        }
        public async Task<IEnumerable<Dashboard>> GetDashBoardDataListAdmin()
        {
            string sql = "select count(*) as StatusCount, Status as Statustype from Cases c where c.Status in (1, 2, 5, 7)  \r\ngroup by Status";
            var CasesCount = await _dbConnection.QueryAsync<Dashboard>(sql);
            return CasesCount.ToList();
        }
        public async Task<IEnumerable<Dashboard>> GetDashBoardDataListClient(Guid _currentLoginUser)
        {
            string sql = $@"SELECT count(C.Status) as StatusCount, C.Status as Statustype
                FROM Cases AS C 
                    LEFT JOIN Companies AS CO ON C.CompanyId = CO.Id 
                    LEFT JOIN TraceLevels AS TL ON C.TraceLevelId = TL.Id 
                    LEFT JOIN TraceReason AS TR ON C.TraceReasonId = TR.Id 
                    LEFT JOIN Subjects AS S ON C.SubjectId = S.Id 
                    LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
                    LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
                WHERE C.CreatedBy = @Id OR C.ClientRef = @Id   group by Status";
            var CasesCount = await _dbConnection.QueryAsync<Dashboard>(sql, new {Id = _currentLoginUser });
            return CasesCount.ToList();
        }
        public async Task<IEnumerable<Dashboard>> GetDashBoardDataAgent(Guid _currentLoginUser)
        {
            string sql = $@"SELECT
              C.Status AS Statustype ,count(C.Status) StatusCount
                FROM Cases AS C 
                INNER JOIN Subjects AS S ON C.SubjectId = S.Id 
               LEFT JOIN TitlePrefixes AS TP ON S.TitlePrefixId = TP.Id 
               LEFT JOIN Users AS CU ON C.ClientRef = CU.Id 
               LEFT JOIN Users AS AU ON C.EndClient = AU.Id 
           WHERE C.EndClient = @Id OR C.EndClient IS NULL OR C.Status IN (1,5,6,7)
          GROUP BY C.Status";
            var CasesCount = await _dbConnection.QueryAsync<Dashboard>(sql, new { Id = _currentLoginUser });
            return CasesCount.ToList();
        }

        #endregion

    }
}
