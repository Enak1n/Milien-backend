namespace MilienAPI.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAddRepository Ads { get; }

        Task Save();
    }
}
