using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Common;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.UserViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AlpineClubBansko.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> usersRepository;

        public UsersService(IRepository<User> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public IQueryable<User> GetAllUsers()
        {
            return this.usersRepository.All();
        }

        public IEnumerable<UserProfileViewModel> GetAllUsersAsViewModels()
        {
            return this.usersRepository.All().To<UserProfileViewModel>();
        }

        public User GetUserById(string userId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(userId, nameof(userId));

            return this.usersRepository.GetById(userId);
        }

        public UserProfileViewModel GetUserByIdAsViewModel(string userId)
        {
            ArgumentValidator.ThrowIfNullOrEmpty(userId, nameof(userId));
            return this.GetAllUsersAsViewModels().FirstOrDefault(a => a.Id == userId);
        }
    }
}