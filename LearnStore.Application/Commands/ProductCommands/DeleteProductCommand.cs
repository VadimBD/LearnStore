using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands.ProductCommands
{
    public record class DeleteProductCommand (int ProductId): IRequest<DeleteProductResult>;
}
