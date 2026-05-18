using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Infrastructure.DataAccess.MsSql
{
    public class EFCustomerRepositoryTests
    {
        [Fact]
        public async Task SaveCustomerAsync_WhenNullCustomer_ThrowArgumentNullException() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFCustomerRepository(context);
            Func<Task> act = () => repository.SaveCustomerAsync(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("customer");
        }

        [Fact]
        public async Task SaveCustomerAsync_WhenCustomerIdMatch_UpdatesExistingCustomer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer1 = new Customer { Id = "1", Name = "Customer 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var customer2 = new Customer { Id = "2", Name = "Customer 2", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();

            var context2 = new AppDbContext(options);
            var repository = new EFCustomerRepository(context2);
            var updatedCustomer = new Customer { Id = "1", Name = "Updated Customer 1", EmailAddress = "updated@test", PhoneNumber = "+232144124" };
            await repository.SaveCustomerAsync(updatedCustomer, CancellationToken.None);
            var savedCustomer = await context2.Customers.FindAsync("1");
            savedCustomer.Should().NotBeNull();
            savedCustomer!.Name.Should().Be("Updated Customer 1");
        }

        [Fact]
        public async Task SaveCustomerAsync_WhenCustomerIdIsZero_AddsNewCustomer() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer = new Customer { Name = "New Customer", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Customers.Add(customer);
            context.SaveChanges();
            var repository = new EFCustomerRepository(context);
            await repository.SaveCustomerAsync(customer, CancellationToken.None);

            var savedCustomer = await context.Customers.FirstOrDefaultAsync(c => c.Name == "New Customer");
            savedCustomer.Should().NotBeNull();
            savedCustomer!.Name.Should().Be("New Customer");
        }

        [Fact]
        public async Task SaveCustomerAsync_WhenNameIsNull_ThrowsArgumentNullException() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFCustomerRepository(context); 
            var customer = new Customer { Id = "1", Name = null!, EmailAddress = "test", PhoneNumber = "+232144124" };
            Func<Task> act = () => repository.SaveCustomerAsync(customer, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("Name");

        }

        [Fact]
        public async Task SaveCustomerAsync_WhenPhoneNumberIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFCustomerRepository(context);
            var customer = new Customer { Id = "1", Name = "Seller 1", EmailAddress = "test", PhoneNumber = null! };
            Func<Task> act = () => repository.SaveCustomerAsync(customer, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("PhoneNumber");
        }

        [Fact]
        public async Task SaveCustomerAsync_WhenEmailAddressIsNull_ThrowsArgumentNullException() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFCustomerRepository(context);
            var customer = new Customer { Id = "1", Name = "Seller 1", EmailAddress = null!, PhoneNumber = "+232144124" };
            Func<Task> act = () => repository.SaveCustomerAsync(customer, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("EmailAddress");
        }

        [Fact]
        public async Task GetCustomerAsync_WhenCustomerIdIsValid_ReturnsCustomer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer1 = new Customer { Id = "1", Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var customer2 = new Customer { Id = "2", Name = "Seller 2", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();
            var repository = new EFCustomerRepository(context);
            var criteria = new CustomerSearchCriteria { Id = "2" };

            var result = await repository.GetCustomersAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(customer2);
        }

        [Fact]
        public async Task GetCustomerAsync_WhenCustomerIdIsInvalid_ReturnsEmpty()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer = new Customer { Id = "1", Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Customers.Add(customer);
            context.SaveChanges();
            var repository = new EFCustomerRepository(context);
            var criteria = new CustomerSearchCriteria { Id = "2" };
            var result = await repository.GetCustomersAsync(criteria, CancellationToken.None);
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCustomerAsync_WhenCustomerCriteriaIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFCustomerRepository(context);
            Func<Task> act = () => repository.GetCustomersAsync(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("criteria");
        }

        [Fact]
        public async Task GetCustomerAsync_WhenCriteriaIsEmpty_ReturnsAllCustomers() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer1 = new Customer { Id = "1", Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var customer2 = new Customer { Id = "2", Name = "Seller 2", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();
            var repository = new EFCustomerRepository(context);
            var criteria = new CustomerSearchCriteria { };
            var result = await repository.GetCustomersAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(customer1);
            result.Should().Contain(customer2);
        }

        [Fact]
        public async Task GetCustomerAsync_WhenCriteriaIsValid_ReturnsCustomer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer1 = new Customer { Id = "1", Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var customer2 = new Customer { Id = "2", Name = "Seller 2", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();
            var repository = new EFCustomerRepository(context);
            var criteria = new CustomerSearchCriteria { Name = "Seller 1" };
            var result = await repository.GetCustomersAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(customer1);
        }
    }
}
