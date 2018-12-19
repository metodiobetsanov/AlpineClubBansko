using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlpineClubBansko.Services.Extensions
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> usersRepository;

        public UsersService(IRepository<User> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public IEnumerable<UserProfileViewModel> GetAllUsers()
        {
            return this.usersRepository.All().To<UserProfileViewModel>();
        }

        public User GetUser(string id)
        {
            return this.usersRepository.GetById(id);
        }

        public UserProfileViewModel GetUserById(string id)
        {
            return this.GetAllUsers().FirstOrDefault(a => a.Id == id);
        }
    }
}
