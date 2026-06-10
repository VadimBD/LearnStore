
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

        public async Task<Customer> GetCustomerAsync(CustomerSearchCriteria criteria, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(criteria, nameof(criteria));
            IQueryable<Customer> query = _context.Customers.Include(c=> c.PurchasedProducts).AsQueryable();
            if (!string.IsNullOrWhiteSpace(criteria.Id))
                query = query.Where(c => c.Id == criteria.Id);
            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(c => c.Name.Contains(criteria.Name));
            if (!string.IsNullOrWhiteSpace(criteria.EmailAddress))
                query = query.Where(c => c.EmailAddress.Contains(criteria.EmailAddress));
            if (!string.IsNullOrWhiteSpace(criteria.PhoneNumber))
                query = query.Where(c => c.PhoneNumber.Contains(criteria.PhoneNumber));

            return await query.FirstOrDefaultAsync(cancellationToken);
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
                UpdateCustomerProducts(existingCustomer,customer);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        private void  UpdateCustomerProducts(Customer existedCustomer, Customer updatedCustomer)
        {
            foreach (var product in updatedCustomer.PurchasedProducts)
            {
                if (!existedCustomer.PurchasedProducts.Any(p => p.Id == product.Id))
                {
                    existedCustomer.PurchasedProducts.Add(product);
                    _context.Attach(product);
                }
            }
            var productsToRemove = existedCustomer.PurchasedProducts.Where(p => !updatedCustomer.PurchasedProducts.Any(up=>up.Id==p.Id)).ToList();
            foreach (var product in productsToRemove) { 
            existedCustomer.PurchasedProducts.Remove(product);
            }
        }

        public async Task UpdatePurchasedProductsAsync(string customerId, IEnumerable<Product> products, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Include(c=> c.PurchasedProducts).FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
            if (customer is null)
                throw new Exception("Customet not found.");
            foreach (var product in products)
            {
                if (!customer.PurchasedProducts.Any(p => p.Id == product.Id))
                {
                    customer.PurchasedProducts.Add(product);
                }
            }
            var productsToRemove = customer.PurchasedProducts.Where(p => !products.Any(up => up.Id == p.Id)).ToList();
            foreach (var product in productsToRemove)
            {
                customer.PurchasedProducts.Remove(product);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
