
namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class EFProductRepository(AppDbContext Context) : IProductRepository
    {
        private readonly AppDbContext _context = Context ?? throw new ArgumentNullException(nameof(Context));
        public IEnumerable<Product> Products => _context.Products.Include(p => p.Category).Include(p => p.Seller).Include(p => p.Author);

        public IEnumerable<ProductCategory> Categories => _context.ProductCategories;

        

        public Product? GetProduct(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public IEnumerable<Product> GetProduct(ProductSearchCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);
            IQueryable<Product> query = _context.Products;
            if (criteria.ProductId > 0)
                query = query.Where(p => p.Id == criteria.ProductId);
            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Author != null)
                query = query.Where(p => p.Author != null && p.Author.Id == criteria.Author.Id);
            if (criteria.Seller != null)
                query = query.Where(p => p.Seller != null && p.Seller.Id == criteria.Seller.Id);
            if (criteria.Category != null)
                query = query.Where(p => p.Category != null && p.Category.Id == criteria.Category.Id);
            if (criteria.Price > 0)
                query = query.Where(p => p.Price == criteria.Price);
            return [.. query];
        }

        public async Task SaveProductAsync(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));
            ArgumentNullException.ThrowIfNull(product.Author, nameof(product.Author));
            ArgumentNullException.ThrowIfNull(product.Seller, nameof(product.Seller));
            ArgumentNullException.ThrowIfNull(product.Category, nameof(product.Category));

            _context.Attach(product.Author);
            _context.Attach(product.Seller);
            _context.Attach(product.Category);
            _context.AttachRange(product.ChildProducts);

            var existingProduct = await _context.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);

            if (existingProduct == null)
                _context.Products.Add(product);
            else
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                if (existingProduct.Author is not null)
                    existingProduct.Author=product.Author;
                if (existingProduct.Seller is not null)
                    existingProduct.Seller = product.Seller;
                if (existingProduct.Category is not null)
                    existingProduct.Category=product.Category;
                UpdateChildProducts(product, existingProduct);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<DeleteProductResult> DeleteProductAsync(int productId, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(productId, cancellationToken);
            if (product is null)
                return new DeleteProductResult
                {
                    Success = false,
                    Message = "Product not found."
                };
            var usedInOrders = await _context.OrderItems.AnyAsync(oi => oi.Product != null && oi.Product.Id == productId);
            
            if (usedInOrders )
            {
                return new DeleteProductResult
                {
                    Success = false,
                    Message = "Cannot delete product because it is used in existing orders"
                };
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new DeleteProductResult
            {
                Success = true,
                Message = string.Empty
            };
        }

        private void UpdateChildProducts(Product updatedProduct, Product existingProduct)
        {
            foreach (var child in updatedProduct.ChildProducts)
            {
                var existingChild = existingProduct.ChildProducts.FirstOrDefault(p => p.Id == child.Id);
                if (existingChild == null)
                    existingProduct.ChildProducts.Add(child);
                else
                {
                    _context.Entry(existingChild).CurrentValues.SetValues(child);
                    existingProduct.Author = child.Author;
                    existingProduct.Category = child.Category;
                    existingProduct.Seller = child.Seller;
                }
            }

            var toRemove = existingProduct.ChildProducts.Where(p => updatedProduct.ChildProducts.All(c => c.Id != p.Id)).ToList();
            foreach (var child in toRemove)
                existingProduct.ChildProducts.Remove(child);
        }


     
    }
}
