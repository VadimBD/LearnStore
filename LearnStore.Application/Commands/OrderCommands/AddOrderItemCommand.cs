namespace LearnStore.Application.Commands.OrderCommands
{
    public record class AddOrderItemCommand: IRequest<int>
    {
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}
