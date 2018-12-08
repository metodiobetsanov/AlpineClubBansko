﻿using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.AlbumViewModels;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IAlbumService
    {
        IEnumerable<AlbumViewModel> GetAllAlbums();

        AlbumViewModel GetAlbumById(string id);

        Task<string> CreateAsync(AlbumViewModel model, User user);

        Task<string> UpdateAsync(AlbumViewModel model);

        Task<int> DeleteAsync(string id);
    }
}