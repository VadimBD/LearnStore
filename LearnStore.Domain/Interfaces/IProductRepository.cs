using LearnStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        Task SaveProductAsync(Product product, CancellationToken cancellationToken);

        Task<DeleteProductResult> DeleteProductAsync(int productId, CancellationToken cancellationToken);
        Task<Product?> GetProductAsync(int productId, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetProductsAsync(ProductSearchCriteria criteria, CancellationToken cancellationToken);

        IEnumerable <ProductCategory> Categories { get; }
        Task<Product?> GetProductNoTrackingAsync(int productId, CancellationToken cancellationToken);
    }
}
