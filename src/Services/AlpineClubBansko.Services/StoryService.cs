using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.StoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class StoryService : IStoryService
    {
        private readonly IRepository<Story> _storyRepository;

        public StoryService(IRepository<Story> storyRepository)
        {
            this._storyRepository = storyRepository;
        }

        public IEnumerable<StoryViewModel> GetAllStories()
        {
            return this._storyRepository.All().To<StoryViewModel>().ToList();
        }

        public StoryViewModel GetStoryById(string id)
        {
            return this.GetAllStories().FirstOrDefault(s => s.Id == id);
        }

        public async Task<int> CreateAsync(StoryViewModel model)
        {
            Story story = new Story {
                Title = model.Title,
                Content = model.Content,
                Author = model.Author
            };

            await this._storyRepository.AddAsync(story);

            return await this._storyRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(StoryViewModel model)
        {
            Story story = this._storyRepository.All().FirstOrDefault(s => s.Id == model.Id);
            story.Title = model.Title;
            story.Content = model.Content;
            story.ModifiedOn = DateTime.UtcNow;

            this._storyRepository.Update(story);

            return await this._storyRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {

            Story story = this._storyRepository.All().FirstOrDefault(s => s.Id == id);

            this._storyRepository.Delete(story);

            return await this._storyRepository.SaveChangesAsync();
        }
    }
}
