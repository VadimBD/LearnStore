
using LearnStore.Domain.Entities;

namespace LearnStore.Domain.Interfaces
{
    public interface ISellerRepository
    {
        Task<IEnumerable<Seller>> GetCustomersAsync(SellerSearchCriteria criteria, CancellationToken cancellationToken);
        Task SaveCustomerAsync(Seller seller, CancellationToken cancellationToken);
        Task<Seller> DeleteCustomerAsync(Guid sellerId, CancellationToken cancellationToken);
    }
}
