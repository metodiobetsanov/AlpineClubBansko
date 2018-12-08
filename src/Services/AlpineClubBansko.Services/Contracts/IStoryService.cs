using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services.Contracts
{
    public interface IStoryService
    {
        IEnumerable<StoryViewModel> GetAllStories();

        StoryViewModel GetStoryById(string id);

        Task<string> CreateAsync(StoryViewModel model, User user);

        Task<string> UpdateAsync(StoryViewModel model);

        Task<int> DeleteAsync(string id);
    }
}
