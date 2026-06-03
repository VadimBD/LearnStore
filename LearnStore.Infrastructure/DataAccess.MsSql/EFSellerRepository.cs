
namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class EFSellerRepository(AppDbContext Context) : ISellerRepository
    {
        private readonly AppDbContext _context = Context??throw new ArgumentNullException(nameof(Context));

        public IEnumerable<Seller> Sellers =>_context.Sellers;

        public async Task<DeleteSellerResult> DeleteSellerAsync(string sellerId, CancellationToken cancellationToken)
        {
            var seller = await _context.Sellers.FindAsync([sellerId], cancellationToken);
            if (seller is null)
                return new DeleteSellerResult() { Success = false, Message = "Seller not found" };
            if (_context.Products.Any(p => p.Seller!.Id == sellerId))
                return new DeleteSellerResult() { Success = false, Message = "Seller has products" };
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync(cancellationToken);
            return new DeleteSellerResult() { Success = true, Message = "Seller deleted successfully" };
        }

        public async Task<Seller?> GetSellerAsync(string sellerId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(sellerId, nameof(sellerId));
            return await _context.Sellers.FirstOrDefaultAsync(s => s.Id == sellerId, cancellationToken);
        }

        public async Task<IEnumerable<Seller>> GetSellersAsync(SellerSearchCriteria criteria, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(criteria, nameof(criteria));
            IQueryable<Seller> query = _context.Sellers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(criteria.Id))
                query = query.Where(s => s.Id == criteria.Id);
            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(s => s.Name.Contains(criteria.Name));
            if (!string.IsNullOrWhiteSpace(criteria.EmailAddress))
                query = query.Where(s => s.EmailAddress.Contains(criteria.EmailAddress));
            if (!string.IsNullOrWhiteSpace(criteria.PhoneNumber))
                query = query.Where(s => s.PhoneNumber.Contains(criteria.PhoneNumber));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task SaveSellerAsync(Seller seller, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(seller, nameof(seller));
            ArgumentException.ThrowIfNullOrWhiteSpace(seller.Name,nameof(seller.Name));
            ArgumentException.ThrowIfNullOrWhiteSpace(seller.PhoneNumber, nameof(seller.PhoneNumber));
            ArgumentException.ThrowIfNullOrWhiteSpace(seller.EmailAddress, nameof(seller.EmailAddress));

            var existingSeller = await _context.Sellers.FirstOrDefaultAsync(s=>s.Id==seller.Id,cancellationToken);

            if (existingSeller == null)
                _context.Sellers.Add(seller);
            else
                _context.Entry(existingSeller).CurrentValues.SetValues(seller);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
