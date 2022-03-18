using Core.DAL;
using Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMediatR(this IServiceCollection collection)
        {
            return collection.AddMediatR(typeof(GetGamesHandler).Assembly);
        }
        public static IServiceCollection RegisterDbContext(this IServiceCollection collection)
        {
            return collection
                .AddDbContext<GameContext>(opt => opt.UseInMemoryDatabase("Games"))
                .AddScoped<IGameContext, GameContext>();
        }
    }
}
