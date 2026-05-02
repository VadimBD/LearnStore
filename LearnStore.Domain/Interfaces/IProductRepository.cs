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
        Product? GetProduct(int productId);
        IEnumerable<Product> GetProduct(ProductSearchCriteria criteria);


        IEnumerable <ProductCategory> Categories { get; }

    }
}
