using AlpineClubBansko.Services.Models.StoryVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    interface IStoryService
    {
        IEnumerable<StoryViewModel> GetAllStories();

        StoryViewModel GetStoryById(string id);

        Task<int> CreateAsync(StoryViewModel model);

        Task<int> UpdateAsync(StoryViewModel model);

        Task<int> DeleteAsync(string id);
    }
}
