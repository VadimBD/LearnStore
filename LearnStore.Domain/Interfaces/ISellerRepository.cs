
using LearnStore.Domain.Entities;

namespace LearnStore.Domain.Interfaces
{
    public interface ISellerRepository
    {
        Task<IEnumerable<Seller>> GetSellerAsync(SellerSearchCriteria criteria, CancellationToken cancellationToken);
        Task SaveSellerAsync(Seller seller, CancellationToken cancellationToken);
    }
}
