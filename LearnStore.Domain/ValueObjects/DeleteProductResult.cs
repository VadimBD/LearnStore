using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.ValueObjects
{
    public class DeleteProductResult
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
