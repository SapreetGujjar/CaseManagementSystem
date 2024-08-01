using CaseManagementSystem.Pages;
using CMS.DL.DM;
using CMS.DL.Model;
using CaseManagementSystem.Data.Dashboard;

namespace CaseManagementSystem.Data.Dashboard
{
    public class DashboardService
    {
        private readonly string _connectionString = "";

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        public async Task<IEnumerable<DashboardViewModel>> GetDashBoardDataList(Guid _currentLoginUser)
        {
            DashboardDM dashboardDM = new DashboardDM(_connectionString);
            var result = await dashboardDM.GetDashBoardDataList(_currentLoginUser);
            return result.Select(c => new DashboardViewModel
            {
                  Statustype  = c.Statustype,
                  StatusCount = c.StatusCount
            });
        }

        public async Task<IEnumerable<DashboardViewModel>> GetDashBoardDataListAdmin()
        {
            DashboardDM dashboardDM = new DashboardDM(_connectionString);
            var result = await dashboardDM.GetDashBoardDataListAdmin();
            return result.Select(c => new DashboardViewModel
            {
                Statustype = c.Statustype,
                StatusCount = c.StatusCount
            });
        }

        public async Task<IEnumerable<DashboardViewModel>> GetDashBoardDataListClient(Guid _currentLoginUser)
        {
            DashboardDM dashboardDM = new DashboardDM(_connectionString);
            var result = await dashboardDM.GetDashBoardDataListClient(_currentLoginUser);
            return result.Select(c => new DashboardViewModel
            {
                Statustype = c.Statustype,
                StatusCount = c.StatusCount
            });
        }
        public async Task<IEnumerable<DashboardViewModel>> GetDashBoardDataAgent(Guid _currentLoginUser)
        {
            DashboardDM dashboardDM = new DashboardDM(_connectionString);
            var result = await dashboardDM.GetDashBoardDataAgent(_currentLoginUser);
            return result.Select(c => new DashboardViewModel
            {
                Statustype = c.Statustype,
                StatusCount = c.StatusCount
            });
        }

    }
}
