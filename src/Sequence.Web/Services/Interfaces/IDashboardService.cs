using Sequence.Web.ViewModels.Dashboard;

namespace Sequence.Web.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync(string userId);
}