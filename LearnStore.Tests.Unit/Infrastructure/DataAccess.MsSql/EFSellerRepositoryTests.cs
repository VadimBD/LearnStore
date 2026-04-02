using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Infrastructure.DataAccess.MsSql
{
    public class EFSellerRepositoryTests
    {

        [Fact]
        public async Task SaveSellerAsync_ThrowArgumentNullException_WhenNullSeller()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);


            var repository = new EFSellerRepository(context);
            Func<Task> act = async () => await repository.SaveSellerAsync(null!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("seller");
        }

        [Fact]

        public async Task SaveSellerAsync_UpdatesExistingSeller_WhenSellerIdMatch()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);

            var seller = new Seller { Id = 1, Name = "Seller 1" ,PhoneNumber = "test",EmailAddress = "test@test.com"};
            var seller2 = new Seller { Id = 2, Name = "Seller 2",PhoneNumber = "test",EmailAddress = "test@test.com"};
            context.Sellers.Add(seller);
            context.Sellers.Add(seller2);
            context.SaveChanges();

            var context2 = new AppDbContext(options);
            var repository = new EFSellerRepository(context2);
            var updatedSeller = new Seller { Id = 1, Name = "Updated Seller 1" , PhoneNumber = "test" , EmailAddress = "test@test.com" };
            await repository.SaveSellerAsync(updatedSeller, CancellationToken.None);

            var savedSeller = await context2.Sellers.FindAsync(1);
            savedSeller.Should().NotBeNull();
            savedSeller!.Name.Should().Be("Updated Seller 1");
        }

        [Fact]
        public async Task SaveSellerAsync_AddsNewSeller_WhenSellerIdIsZero()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var seller = new Seller { Name = "New Seller" ,PhoneNumber = "+232144124", EmailAddress="test" };
            context.Sellers.Add(seller);
            context.SaveChanges();
            var repository = new EFSellerRepository(context);
            await repository.SaveSellerAsync(seller, CancellationToken.None);

            var savedSeller = await context.Sellers.FirstOrDefaultAsync(s => s.Name == "New Seller");
            savedSeller.Should().NotBeNull();
            savedSeller.Name.Should().Be("New Seller");
        }

        [Fact]
        public async Task SaveSellerAsync_ThrowsArgumentNullException_WhenNameIsNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);
            var repository = new EFSellerRepository(context);

            var seller = new Seller { Id = 1, EmailAddress = "test", PhoneNumber = "+232144124" };

            Func<Task> act = () => repository.SaveSellerAsync(seller, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("Name");
        }
        [Fact]
        public async Task SaveSellerAsync_ThrowsArgumentNullException_WhenPhoneNumberIsNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFSellerRepository(context);
            var seller = new Seller { Id = 1, Name = "test", EmailAddress = "test" };
            Func<Task> act = () => repository.SaveSellerAsync(seller, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("PhoneNumber");

        }
        [Fact]
        public async Task SaveSellerAsync_ThrowsArgumentNullException_WhenEmailAddressIsNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFSellerRepository(context);
            var seller = new Seller { Id = 1, Name = "test", PhoneNumber = "+232144124" };
            Func<Task> act = () => repository.SaveSellerAsync(seller, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("EmailAddress");
        }

        [Fact]
        public async Task GetSellerAsync_ReturnsSeller_WhenSellerIdIsValid()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);

            var seller = new Seller { Id = 1, Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var seller2 = new Seller { Id = 2, Name = "Seller 2", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Sellers.Add(seller);
            context.Sellers.Add(seller2);
            context.SaveChanges();
            var repository = new EFSellerRepository(context);
            var criteria = new SellerSearchCriteria { Id = 2 };

            var result = await repository.GetSellerAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(seller2);
        }

        [Fact]
        public async Task GetSellerAsync_ReturnsEmpty_WhenSellerIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var seller = new Seller { Id = 1, Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            
            context.Sellers.Add(seller);
            context.SaveChanges();
            var repository = new EFSellerRepository(context);
            var criteria = new SellerSearchCriteria { Id = 999 };

            var result = await repository.GetSellerAsync(criteria, CancellationToken.None);
           
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSellersAsync_ReturnsNull_WhenSellersCriteriaIsNull() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFSellerRepository(context);
            Func<Task> act = () => repository.GetSellerAsync(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("criteria");
        }

        [Fact]
        public async Task GetSellersAsync_ReturnsAllSellers_WhenCriteriaIsEmpty() 
        { 
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var seller1 = new Seller { Id = 1, Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var seller2 = new Seller { Id = 2, Name = "Seller 2", EmailAddress = "test", PhoneNumber = "+232144124" };
            context.Sellers.Add(seller1);
            context.Sellers.Add(seller2);
            context.SaveChanges();
            var repository = new EFSellerRepository(context);
            var criteria = new SellerSearchCriteria();
            var result = await repository.GetSellerAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(seller1);
            result.Should().Contain(seller2);
        }
        [Fact]
        public async Task GetSellersAsync_ReturnsSellers_WhenCriteriaValid()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var seller1 = new Seller { Id = 1, Name = "Seller 1", EmailAddress = "test", PhoneNumber = "+232144124" };
            var seller2 = new Seller { Id = 2, Name = "Seller 2", EmailAddress = "test", PhoneNumber = "+232144124" };

            context.Sellers.Add(seller1);
            context.Sellers.Add(seller2);
            context.SaveChanges();
            var repository = new EFSellerRepository(context);
            var criteria = new SellerSearchCriteria { Name = "Seller 1", Id = 1 };
            var result = await repository.GetSellerAsync(criteria, CancellationToken.None);
            result.Should().NotBeNull();
            result.Should().Contain(seller1);
        }

    }
}
