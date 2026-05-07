using LearnStore.Application.Validators;
using Microsoft.Extensions.DependencyInjection;


namespace LearnStore.Application.Extensions
{
    public static class ApplicationRegistration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationRegistration).Assembly));
            services.AddValidatorsFromAssemblyContaining<CreateAuthorCommandValidator>();

            services.AddSingleton<IMapper<Product, ProductDto>, ProductMapper>();
            services.AddSingleton<IMapper<Customer, CustomerDto>, CustomerMapper>();
            services.AddSingleton<IMapper<OrderItem, OrderItemDto>, OrderItemMapper>();
            services.AddSingleton<IMapper<Order, OrderDto>, OrderMapper>();
            services.AddSingleton<IMapper<Author, AuthorDto>, AuthorMapper>();
            services.AddSingleton<IMapper<Payment, PaymentDto>, PaymentMapper>();
            services.AddSingleton<IMapper<ProductCategory, ProductCategoryDto>, ProductCategoryMapper>();
            services.AddSingleton<IMapper<Seller, SellerDto>, SellerMapper>();
            

        }
    }
}
