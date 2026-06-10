using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands.SellerCommands
{
    public record class AddToSellerBalanceCommand():IRequest<Unit>
    {
        public string SellerId { get; set;}
        public decimal Ammaunt { get; set; }
    }
}
