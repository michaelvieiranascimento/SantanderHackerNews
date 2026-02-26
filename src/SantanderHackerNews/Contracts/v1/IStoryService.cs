using SantanderHackerNews.Models.v1;

namespace SantanderHackerNews.Contracts.v1;

public interface IStoryService
{
    Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int n);
}