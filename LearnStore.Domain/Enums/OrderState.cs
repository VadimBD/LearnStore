using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Enums
{
    public enum OrderState
    {
        New = 0,
        Pending = 1,
        Completed = 2,
        Cancelled = 3
    }
}
