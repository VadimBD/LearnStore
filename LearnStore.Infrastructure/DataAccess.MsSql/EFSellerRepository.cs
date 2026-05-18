
namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class EFSellerRepository(AppDbContext Context) : ISellerRepository
    {
        private readonly AppDbContext _context = Context??throw new ArgumentNullException(nameof(Context));

        public IEnumerable<Seller> Sellers =>_context.Sellers;

        public async Task<IEnumerable<Seller>> GetSellerAsync(SellerSearchCriteria criteria, CancellationToken cancellationToken)
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
