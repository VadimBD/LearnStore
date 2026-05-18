namespace LearnStore.Application.Commands.SellerCommands
{
    public record class DeleteSellerCommand (string SellerId ) : IRequest<SellerDto>;
    
}
