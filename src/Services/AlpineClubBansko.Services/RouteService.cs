using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Common;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.RouteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRepository<Route> routeRepository;
        private readonly IRepository<RouteComment> routeCommentRepository;
        private readonly IRepository<LikedRoutes> likedRoutesRepository;
        private readonly IRepository<Location> locationRepository;

        public RouteService(IRepository<Route> routeRepository,
            IRepository<RouteComment> routeCommentRepository,
            IRepository<LikedRoutes> likedRoutesRepository,
            IRepository<Location> locationRepository)
        {
            this.locationRepository = locationRepository;
            this.likedRoutesRepository = likedRoutesRepository;
            this.routeCommentRepository = routeCommentRepository;
            this.routeRepository = routeRepository;
        }

        public IQueryable<Route> GetAllRoutes()
        {
            return this.routeRepository.All();
        }

        public IEnumerable<RouteViewModel> GetAllRoutesAsViewModels()
        {
            return this.routeRepository.All().To<RouteViewModel>();
        }

        public Route GetRouteById(string routeId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));

            return this.routeRepository.GetById(routeId);
        }

        public RouteViewModel GetRouteByIdAsViewModel(string routeId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));

            return this.GetAllRoutesAsViewModels().FirstOrDefault(a => a.Id == routeId);
        }

        public async Task<string> CreateAsync(string name, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            Route route = new Route
            {
                Title = name,
                Author = user,
                CreatedOn = DateTime.UtcNow
            };

            await this.routeRepository.AddAsync(route);
            await this.routeRepository.SaveChangesAsync();

            return route.Id;
        }

        public async Task<bool> UpdateAsync(RouteViewModel model)
        {
            ArgumentValidator.ThrowIfNull(model, nameof(model));

            Route route = this.routeRepository.GetById(model.Id);
            route.Title = model.Title;
            route.Content = model.Content;
            route.TimeNeeded = model.TimeNeeded;
            route.ModifiedOn = DateTime.UtcNow;

            this.routeRepository.Update(route);
            var result = await this.routeRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string routeId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));

            Route route = this.routeRepository.All().FirstOrDefault(s => s.Id == routeId);

            if (route.Locations != null)
            {
                foreach (var item in route.Locations)
                {
                    this.locationRepository.Delete(item);
                }
            }

            if (route.Comments != null)
            {
                foreach (var item in route.Comments)
                {
                    this.routeCommentRepository.Delete(item);
                }
            }

            if (route.Favorite != null)
            {
                foreach (var item in route.Favorite)
                {
                    this.likedRoutesRepository.Delete(item);
                }
            }

            this.routeRepository.Delete(route);

            var result = await this.routeRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> CreateLocationAsync(LocationViewModel model, User user)
        {
            ArgumentValidator.ThrowIfNull(model, nameof(model));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            Location location = new Location
            {
                Name = model.Name,
                CreatedOn = DateTime.UtcNow,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                AuthorId = user.Id,
                RouteId = model.RouteId
            };

            await this.locationRepository.AddAsync(location);
            var result = await this.locationRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DeleteLocationAsync(string locationId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(locationId, nameof(locationId));

            Location location = this.locationRepository.GetById(locationId);

            this.locationRepository.Delete(location);
            var result = await this.locationRepository.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> CreateCommentAsync(string routeId, string content, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));
            ArgumentValidator.ThrowIfNullOrEmpty(content, nameof(content));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            var comment = new RouteComment
            {
                AuthorId = user.Id,
                RouteId = routeId,
                Comment = content,
                CreatedOn = DateTime.UtcNow
            };

            await this.routeCommentRepository.AddAsync(comment);
            var result = await this.routeCommentRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(commentId, nameof(commentId));

            var item = this.routeCommentRepository.GetById(commentId);

            this.routeCommentRepository.Delete(item);
            var result = await this.routeCommentRepository.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> AddViewedAsync(string storyId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(storyId, nameof(storyId));

            Route route = this.routeRepository.GetById(storyId);

            route.Views += 1;

            this.routeRepository.Update(route);
            var changed = await this.routeRepository.SaveChangesAsync();

            return changed != 0;
        }

        public async Task<bool> FavoriteAsync(string routeId, User user)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(routeId, nameof(routeId));
            ArgumentValidator.ThrowIfNull(user, nameof(user));

            if (this.likedRoutesRepository.All()
                .Any(f => f.UserId == user.Id && f.RouteId == routeId))
            {
                var item = this.likedRoutesRepository.All()
                    .FirstOrDefault(f => f.UserId == user.Id && f.RouteId == routeId);

                this.likedRoutesRepository.Delete(item);
            }
            else
            {
                await this.likedRoutesRepository.AddAsync(new LikedRoutes
                {
                    UserId = user.Id,
                    RouteId = routeId,
                });
            }
            var changed = await this.likedRoutesRepository.SaveChangesAsync();

            return changed != 0;
        }
    }
}