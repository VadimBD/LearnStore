namespace LearnStore.Application.Commands
{
    public record class AddOrderItemCommand: IRequest<int>
    {
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}
