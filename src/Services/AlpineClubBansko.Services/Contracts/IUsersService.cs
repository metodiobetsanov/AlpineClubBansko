using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IUsersService
    {
        IEnumerable<UserProfileViewModel> GetAllUsers();

        User GetUser(string id);

        UserProfileViewModel GetUserById(string id);
    }
}
