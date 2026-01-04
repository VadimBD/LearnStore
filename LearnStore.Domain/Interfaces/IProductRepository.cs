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
        Product? DeleteProduct(Guid productId);

        IEnumerable<ProductCategory> Categories { get; }

    }
}
