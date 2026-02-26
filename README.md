#Santander Hacker News API
Esta API recupera as melhores histórias (Best Stories) do Hacker News, formatadas e ordenadas conforme os requisitos do teste técnico.

🚀 Como rodar a aplicação
Certifique-se de ter o .NET 8 SDK instalado.

Clone o repositório.

No terminal, na raiz do projeto, execute:

Bash

dotnet run
A documentação Swagger abrirá automaticamente em http://localhost:5000 (ou na porta configurada).

🧠 Premissas e Decisões de Design
Não Sobrecarregar a API Externa: Implementei um sistema de Cache em Memória (IMemoryCache) e um limitador de concorrência (SemaphoreSlim). Isso garante que, mesmo com milhares de usuários acessando nossa API, o número de requisições enviadas ao Hacker News seja controlado e reaproveitado.

Conversão de Dados: O Hacker News retorna o tempo em Unix Epoch. Fizemos a conversão para DateTimeOffset para atender ao formato ISO 8601 solicitado.

Resiliência: Utilizei o pacote Microsoft.Extensions.Http.Resilience para lidar com falhas temporárias de rede de forma automática.

🛠 Melhorias que eu faria com mais tempo
1) Distributed Cache (Redis): Se a API precisar rodar em múltiplos servidores (Load Balancer), o cache em memória não seria suficiente. Usaria Redis para compartilhar o cache entre as instâncias.

2) Testes de Carga: Utilizaria ferramentas como k6 ou JMeter para validar o comportamento sob estresse.

3) Logging Estruturado: Implementaria o Serilog para enviar logs para um ElasticSearch ou Application Insights, facilitando o monitoramento.

4) Auto-refresh (Background Worker): Implementaria um BackgroundService que atualiza o cache a cada 5 minutos de forma independente das requisições dos usuários, garantindo latência zero para o cliente final.