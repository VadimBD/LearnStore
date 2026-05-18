namespace LearnStore.Application.Commands.SellerCommands
{
    public record class DeleteSellerCommand (int SellerId ) : IRequest<SellerDto>;
    
}
