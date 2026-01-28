namespace LearnStore.Application.Commands
{
    public record class DeleteSellerCommand (int SellerId ) : IRequest<SellerDto>;
    
}
