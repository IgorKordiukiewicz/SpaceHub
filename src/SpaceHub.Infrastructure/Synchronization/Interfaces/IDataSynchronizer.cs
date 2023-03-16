using FluentResults;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Synchronization.Interfaces;

public interface IDataSynchronizer<TModel> where TModel : class
{
    Task<Result> Synchronize();
}
