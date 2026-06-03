
using LearnStore.Domain.Entities;

namespace LearnStore.Domain.Interfaces
{
    public interface ISellerRepository
    {
        Task<Seller?> GetSellerAsync(string sellerId, CancellationToken cancellationToken);
        Task<IEnumerable<Seller>> GetSellersAsync(SellerSearchCriteria criteria, CancellationToken cancellationToken);
        Task SaveSellerAsync(Seller seller, CancellationToken cancellationToken);

        Task<DeleteSellerResult> DeleteSellerAsync(string sellerId, CancellationToken cancellationToken);
    }
}
