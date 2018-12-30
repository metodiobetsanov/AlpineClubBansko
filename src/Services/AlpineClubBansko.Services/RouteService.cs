using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.RouteViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRepository<Route> routeRepository;

        public RouteService(IRepository<Route> routeRepository)
        {
            this.routeRepository = routeRepository;
        }

        public IEnumerable<RouteViewModel> GetAllRoutes()
        {
            return this.routeRepository.All().To<RouteViewModel>();
        }

        public Route GetRoute(string id)
        {
            return this.routeRepository.GetById(id);
        }

        public RouteViewModel GetRouteById(string id)
        {
            return this.GetAllRoutes().FirstOrDefault(a => a.Id == id);
        }

        public async Task<string> CreateRouteAsync(string name, User user)
        {
            Route route = new Route
            {
                Title = name,
                Author = user
            };

            await this.routeRepository.AddAsync(route);
            await this.routeRepository.SaveChangesAsync();

            return route.Id;
        }
    }
}