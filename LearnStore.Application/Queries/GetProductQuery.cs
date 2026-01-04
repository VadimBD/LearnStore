using System;
using System.Collections.Generic;
using System.Text;
using MediatR;


namespace LearnStore.Application.Queries
{
    public class GetProductQuery :  IRequest<ProductDto?>
    {
    }
}
