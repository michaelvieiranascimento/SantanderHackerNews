using SantanderHackerNews.Contracts.v1;
using SantanderHackerNews.Services.v1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Santander Hacker News API",
        Version = "v1",
        Description = "API para recuperar as melhores histórias do Hacker News com cache e resiliência."
    });
});

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IStoryService, StoryServiceHandler>(client => {
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
})
.AddStandardResilienceHandler(options =>
{
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(2);
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Santander HN API v1");
    c.RoutePrefix = string.Empty;
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapGet("/api/v1/best-stories", async (int quantity, IStoryService storyService) => {
    if (quantity <= 0) return Results.BadRequest("A quantidade deve ser maior que zero.");

    var result = await storyService.GetBestStoriesAsync(quantity);
    return Results.Ok(result);
})
.WithName("GetBestStories")
.WithOpenApi(operation =>
{
    operation.Summary = "Recupera as melhores histórias do Hacker News";
    operation.Description = "Retorna uma lista de histórias ordenadas pelo score de forma decrescente.";
    return operation;
});

app.Run();