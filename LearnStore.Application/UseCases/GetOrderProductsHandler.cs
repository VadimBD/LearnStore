using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetOrderProductsHandler(IOrderRepository OrderRepository, IMapper<Product,ProductDto> Mapper) : IRequestHandler<GetOrderProductsQuery, IEnumerable<ProductDto>>
    {


        public async Task<IEnumerable<ProductDto>> Handle(GetOrderProductsQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            var order = await OrderRepository.GetOrderNoTrackingAsync(query.Orderid, cancellationToken);

            return order.Items.Select(i => Mapper.ToDto( i.Product)).ToList();
        }
    }
}
