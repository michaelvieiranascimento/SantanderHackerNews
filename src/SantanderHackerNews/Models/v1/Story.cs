namespace SantanderHackerNews.Models.v1;

public record StoryDto(
    string Title,
    string Uri,
    string PostedBy,
    DateTimeOffset Time,
    int Score,
    int CommentCount
);