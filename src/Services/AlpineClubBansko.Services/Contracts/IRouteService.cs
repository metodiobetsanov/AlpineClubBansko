using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.RouteViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IRouteService
    {
        IEnumerable<RouteViewModel> GetAllRoutes();

        Route GetRoute(string id);

        RouteViewModel GetRouteById(string id);

        Task<string> CreateRouteAsync(string name, User user);
    }
}