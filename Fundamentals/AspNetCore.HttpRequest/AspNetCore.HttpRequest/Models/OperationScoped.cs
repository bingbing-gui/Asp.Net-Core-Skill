﻿namespace AspNetCore.HttpRequest.Models
{
    public interface IOperationScoped
    {
        string OperationId { get; }
    }
    public class OperationScoped : IOperationScoped
    {
        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];
    }
}
