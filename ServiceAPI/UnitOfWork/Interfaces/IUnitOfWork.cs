using MilienAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPaidAdRepository PaidAds { get; }

        Task Save();
    }
}
