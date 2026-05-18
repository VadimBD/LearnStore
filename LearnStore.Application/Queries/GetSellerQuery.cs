

namespace LearnStore.Application.Queries
{
    public record class GetSellerQuery(string Id) :IRequest<SellerDto?>
    {
    }

}
