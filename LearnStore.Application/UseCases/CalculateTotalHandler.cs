using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CalculateTotalHandler(IProductRepository productRepository) : IRequestHandler<CalculateTotalQuery, decimal>
    {
        public async Task<decimal> Handle(CalculateTotalQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            ArgumentNullException.ThrowIfNull(query.Items, nameof(query.Items));
            decimal total = 0;

            foreach (var item in query.Items.Where(i => i.Quantity > 0 && i.ProductId > 0))
            {
                var product = await productRepository.GetProductNoTrackingAsync(
                    item.ProductId,
                    cancellationToken);

                total += product.Price * item.Quantity;
            }

            return total;
        }
    }
}
