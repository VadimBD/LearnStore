using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    /// <summary>
    /// Query to calculate the total price of an order based on the provided productId and their quantities.
    /// </summary>
    /// <param name="Items"></param>
    public record class CalculateTotalQuery(List<CalculateTotalItem> Items) : IRequest<decimal>;
 
    public record class CalculateTotalItem(int ProductId, int Quantity);
}
