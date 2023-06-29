using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAddRepository Ads { get; private set; }

        public UnitOfWork(IAddRepository addRepository)
        {
            Ads = addRepository;
        }
    }
}
