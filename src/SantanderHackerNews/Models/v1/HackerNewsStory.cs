using System.Text.Json.Serialization;

namespace SantanderHackerNews.Models.v1;

public record HackerNewsStory(
    string Title,
    string Url,
    string By,
    long Time,
    int Score,
    [property: JsonPropertyName("descendants")] int CommentCount
);