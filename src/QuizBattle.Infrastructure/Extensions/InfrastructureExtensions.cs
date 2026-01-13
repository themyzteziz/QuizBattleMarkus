//using Microsoft.Extensions.DependencyInjection;

using QuizBattle.Application.Interfaces;
using QuizBattle.Infrastructure.Repositories;

namespace QuizBattle.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {

            services.AddSingleton<IQuestionRepository, InMemoryQuestionRepository>();
            services.AddSingleton<ISessionRepository, InMemorySessionRepository>();

            return services;
        }
    }
}
