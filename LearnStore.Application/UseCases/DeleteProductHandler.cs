using LearnStore.Application.Commands.ProductCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class DeleteProductHandler(IProductRepository ProductRepository, IMapper<Product, ProductDto> Mapper) : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (command.ProductId <= 0)
                return new DeleteProductResult
                {
                    Success = false,
                    Message = "Invalid ProductId."
                };

            return await ProductRepository.DeleteProductAsync(command.ProductId, cancellationToken);
        }
    }
}
