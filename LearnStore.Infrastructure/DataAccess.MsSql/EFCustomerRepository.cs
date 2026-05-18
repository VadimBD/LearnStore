
namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class EFCustomerRepository(AppDbContext Context) : ICustomerRepository
    {
        private readonly AppDbContext _context = Context ?? throw new ArgumentNullException(nameof(Context));
        public IEnumerable<Customer> Customers => _context.Customers;

        public async Task<Customer> DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer Id cannot be empty.", nameof(customerId));

            var customer = await _context.Customers.FindAsync(new object[] { customerId }, cancellationToken);

            if (customer is null)
                throw new KeyNotFoundException($"Customer with id {customerId} not found.");

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync(cancellationToken);

            return customer;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(CustomerSearchCriteria criteria, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(criteria, nameof(criteria));
            IQueryable<Customer> query = _context.Customers.AsQueryable();
            if(!string.IsNullOrWhiteSpace(criteria.Id))
                query=query.Where(c=>c.Id == criteria.Id);
            if(!string.IsNullOrWhiteSpace(criteria.Name))
                query=query.Where(c=>c.Name.Contains(criteria.Name));
            if (!string.IsNullOrWhiteSpace(criteria.EmailAddress))
                query = query.Where(c => c.EmailAddress.Contains(criteria.EmailAddress));
            if (!string.IsNullOrWhiteSpace(criteria.PhoneNumber))
                query=query.Where(c=>c.PhoneNumber.Contains(criteria.PhoneNumber));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task SaveCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(customer.Id, nameof(customer.Id));
            ArgumentNullException.ThrowIfNull(customer.Name, nameof(customer.Name));
            ArgumentNullException.ThrowIfNull(customer.PhoneNumber,nameof(customer.PhoneNumber));
            ArgumentNullException.ThrowIfNull(customer.EmailAddress, nameof(customer.EmailAddress));

            var existingCustomer=await _context.Customers.FirstOrDefaultAsync(c=>c.Id==customer.Id, cancellationToken);

            if(existingCustomer == null)
                _context.Customers.Add(customer);
            else
            {
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
