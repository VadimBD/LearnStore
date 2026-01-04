using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Enums
{
    public enum OrderState
    {
        Pending = 0,
        Processing = 1,
        Cancelled = 2,
        Completed = 3
    }
}
