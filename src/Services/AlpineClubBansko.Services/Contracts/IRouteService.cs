using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.RouteViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IRouteService
    {
        IQueryable<Route> GetAllRoutes();

        IEnumerable<RouteViewModel> GetAllRoutesAsViewModels();

        Route GetRouteById(string routeId);

        RouteViewModel GetRouteByIdAsViewModel(string routeId);

        Task<string> CreateAsync(string name, User user);

        Task<bool> UpdateAsync(RouteViewModel model);

        Task<bool> DeleteAsync(string routeId);

        Task<bool> CreateLocationAsync(LocationViewModel model, User user);

        Task<bool> DeleteLocationAsync(string locationId);

        Task<bool> AddViewedAsync(string routeId);

        Task<bool> FavoriteAsync(string routeId, User user);

        Task<bool> CreateCommentAsync(string routeId, string content, User user);

        Task<bool> DeleteCommentAsync(string commentId);
    }
}