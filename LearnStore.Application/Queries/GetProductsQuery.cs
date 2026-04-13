using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public int SellerId { get; init; }
        public int CategoryId { get; init; }
        public int AuthorId { get; init; }
        public bool IsActive { get; init; }
        public decimal Price { get; init; }

        public string ProductName { get; init; }

    }
}
