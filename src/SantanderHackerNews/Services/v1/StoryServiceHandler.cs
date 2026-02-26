using Microsoft.Extensions.Caching.Memory;
using SantanderHackerNews.Contracts.v1;
using SantanderHackerNews.Models.v1;

namespace SantanderHackerNews.Services.v1;

public sealed class StoryServiceHandler(HttpClient httpClient, IMemoryCache cache) : IStoryService
{
    private static readonly SemaphoreSlim _throttler = new(10);

    public async Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int n)
    {
        var bestIds = await cache.GetOrCreateAsync("best_ids", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return httpClient.GetFromJsonAsync<int[]>("beststories.json");
        });

        var targetIds = bestIds?.Take(n) ?? Enumerable.Empty<int>();

        var tasks = targetIds.Select(id => GetStoryWithCacheAsync(id));
        var stories = await Task.WhenAll(tasks);

        return stories
            .Where(s => s != null)
            .OrderByDescending(s => s!.Score)
            .ToList()!;
    }

    private async Task<StoryDto?> GetStoryWithCacheAsync(int id)
    {
        return await cache.GetOrCreateAsync($"story_{id}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

            await _throttler.WaitAsync();
            try
            {
                var hnStory = await httpClient.GetFromJsonAsync<HackerNewsStory>($"item/{id}.json");
                if (hnStory == null) return null;

                return new StoryDto(
                    hnStory.Title,
                    hnStory.Url,
                    hnStory.By,
                    DateTimeOffset.FromUnixTimeSeconds(hnStory.Time),
                    hnStory.Score,
                    hnStory.CommentCount
                );
            }
            finally
            {
                _throttler.Release();
            }
        });
    }
}