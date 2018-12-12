using AlpineClubBansko.Data.Contracts;
using AlpineClubBansko.Data.Models;
using AlpineClubBansko.Services.Contracts;
using AlpineClubBansko.Services.Mapping;
using AlpineClubBansko.Services.Models.StoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlpineClubBansko.Services
{
    public class StoryService : IStoryService
    {
        private readonly IRepository<Story> storyRepository;

        public StoryService(IRepository<Story> storyRepository)
        {
            this.storyRepository = storyRepository;
        }

        public IEnumerable<StoryViewModel> GetAllStories()
        {
            return this.storyRepository.All().To<StoryViewModel>();
        }

        public Story GetStory(string id)
        {
            return this.storyRepository.GetById(id);
        }

        public StoryViewModel GetStoryById(string id)
        {
            return this.GetAllStories().FirstOrDefault(s => s.Id == id);
        }

        public async Task<string> CreateAsync(StoryViewModel model, User user)
        {
            Story story = new Story {
                Title = model.Title,
                Content = model.Content,
                Author = user
            };

            await this.storyRepository.AddAsync(story);
            await this.storyRepository.SaveChangesAsync();

            return story.Id;
        }

        public async Task<string> UpdateAsync(StoryViewModel model)
        {
            Story story = this.storyRepository.GetById(model.Id);
            story.Title = model.Title;
            story.Content = model.Content;
            story.ModifiedOn = DateTime.UtcNow;

            this.storyRepository.Update(story);
            await this.storyRepository.SaveChangesAsync();

            return story.Id;
        }

        public async Task<int> DeleteAsync(string id)
        {

            Story story = this.storyRepository.All().FirstOrDefault(s => s.Id == id);

            this.storyRepository.Delete(story);

            return await this.storyRepository.SaveChangesAsync();
        }
    }
}
