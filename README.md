Santander Hacker News API
This API retrieves the "Best Stories" from Hacker News, formatted and sorted according to the technical assessment requirements.

🚀 How to Run the Application
Ensure you have the .NET 8 SDK installed.

Clone the repository.

In the terminal, at the project root, execute:

Bash

dotnet run
The Swagger documentation will automatically open at http://localhost:5000/index.html (or your configured port).

Premises and Design Decisions
Managing External API Load: I implemented an In-Memory Cache (IMemoryCache) and a concurrency limiter (SemaphoreSlim). This ensures that even with high user demand, the number of requests sent to Hacker News is controlled and reused.

Architecture: I opted for a monolithic architecture to prioritize operational efficiency, high cohesion, and simplified scalability.

Data Conversion: Hacker News returns time in Unix Epoch. I implemented a conversion to DateTimeOffset to comply with the requested ISO 8601 format.

Resilience: I utilized the Microsoft.Extensions.Http.Resilience package to automatically handle transient network failures.

🛠 Improvements I Would Make with More Time
Distributed Cache (Redis): If the API needs to run on multiple servers (Load Balancer), in-memory caching would not be sufficient. I would use Redis to share the cache across instances.

Filters and Dynamic Sorting: Expansion of the route to support sorting parameters (e.g., by date, by author) in addition to the current descending score.

Result Pagination: Implementation of page and pageSize to optimize network traffic and frontend performance when dealing with large volumes of stories.

Structured Logging: I would implement Serilog to send logs to ElasticSearch or Application Insights, facilitating monitoring.

Auto-refresh (Background Worker): I would implement a BackgroundService that updates the cache every 5 minutes independently of user requests, ensuring zero latency for the end client.
