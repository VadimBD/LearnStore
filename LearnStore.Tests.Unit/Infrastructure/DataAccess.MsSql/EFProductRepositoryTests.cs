

namespace LearnStore.Tests.Unit.Infrastructure.DataAccess.MsSql
{
    public class EFProductRepositoryTests
    {
        [Fact]
        public async Task DeleteProductAsync_WhenProductIsUsedInOrders_ReturnsFailureResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);

            var product = new Product { Id = 1, Name = "Test Product" };
            context.Products.Add(product);

            context.OrderItems.Add(new OrderItem { Id = 1, Product = product });
            await context.SaveChangesAsync();
            var repository = new EFProductRepository(context);
            // Act
            var result = await repository.DeleteProductAsync(1, CancellationToken.None);
            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("is used in existing");
        }
        [Fact]
        public async Task DeleteProductAsync_ReturnsSuccessResult_WhenProductIsNotUsed()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);

            var product = new Product { Id = 1, Name = "Test Product" };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new EFProductRepository(context);
            // Act
            var result = await repository.DeleteProductAsync(1, CancellationToken.None);
            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductNotFound_ReturnsFailureResult()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFProductRepository(context);
            var result = await repository.DeleteProductAsync(999, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("not found");
        }

        [Fact]
        public async Task DeleteProductAsync_WhenNotUsed_DeletesProduct()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);

            var product = new Product { Id = 1, Name = "Test Product" };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var repository = new EFProductRepository(context);
            var result = await repository.DeleteProductAsync(1, CancellationToken.None);
            var deletedProduct = await context.Products.FindAsync(1);

            result.Success.Should().BeTrue();
            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductIdIsValidAndNotUsed_DeletesOnlyMatchingProduct()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var product1 = new Product { Id = 1, Name = "Test Product" };
            var product2 = new Product { Id = 2, Name = "Another Product" };
            context.Products.AddRange(product1, product2);
            await context.SaveChangesAsync();
            var repository = new EFProductRepository(context);

            var result = await repository.DeleteProductAsync(1, CancellationToken.None);
            var remainingProduct = await context.Products.FindAsync(2);

            result.Success.Should().BeTrue();
            remainingProduct.Should().NotBeNull();
            remainingProduct.Name.Should().Be(product2.Name);
        }

        [Fact]
        public async Task SaveProductAsync_WhenNullProduct_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFProductRepository(context);

            Func<Task> act = async () => await repository.SaveProductAsync(null!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("product");
        }

        [Fact]
        public async Task SaveProducAsync_UpdatesExistingProduct_WhenProductIdMatch()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var category1 = new ProductCategory { Id = 1, Name = "Category 1" };
            var category2 = new ProductCategory { Id = 2, Name = "Category 2" };
            var author1 = new Author { Id = 1, FirstName = "Name1" };
            var author2 = new Author { Id = 2, FirstName = "Name2" };
            var seller1 = new Seller { Id = 1, Name = "Seller1" };
            var seller2 = new Seller { Id = 2, Name = "Seller2" };
            var product1 = new Product { Id = 1, Name = "Product 1", Category = category1, Author = author1, Seller = seller1 };
            var product2 = new Product { Id = 2, Name = "Product 2", Category = category1, Author = author1, Seller = seller1 };
            context.ProductCategories.AddRange([category1, category2]);
            context.Products.AddRange(product1, product2);
            context.SaveChanges();

            using var context2 = new AppDbContext(options);
            var repsitory = new EFProductRepository(context2);
            var product = new Product() { Id = 2, Name = "Product 2", Category = new() { Id = 2 }, Author = new() { Id = 2 }, Seller = new() { Id = 2 }, Description = "Description", Price = 10M };
            await repsitory.SaveProductAsync(product, CancellationToken.None);

            var savedProduct = context2.Products.Find(product.Id);
            savedProduct.Should().NotBeNull();
            savedProduct.Should().BeEquivalentTo(product);
        }
        [Fact]
        public async Task SaveProductAsync_WhenProductIdIsZero_AddsNewProduct()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            var author = new Author { Id = 1, FirstName = "Name1" };
            var seller = new Seller { Id = 1, Name = "Seller1" };
            context.ProductCategories.Add(category);
            context.Authors.Add(author);
            context.Sellers.Add(seller);
            context.SaveChanges();
            using var context2 = new AppDbContext(options);
            var repository = new EFProductRepository(context2);
            var product = new Product() { Id = 0, Name = "New Product", Category = new() { Id = 1 }, Author = new() { Id = 1 }, Seller = new() { Id = 1 }, Description = "Description", Price = 10M };
            await repository.SaveProductAsync(product, CancellationToken.None);
            var savedProduct = context2.Products.FirstOrDefault(p => p.Name == "New Product");
            savedProduct.Should().NotBeNull();
            savedProduct.Name.Should().Be("New Product");
        }
        [Fact]
        public async Task SaveProductAsync_WhenNullCategory_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFProductRepository(context);
            var product = new Product() { Id = 1, Name = "Product 1", Category = null, Author = new() { Id = 1 }, Seller = new() { Id = 1 }, Description = "Description", Price = 10M };
            //Act
            Func<Task> act = () => repository.SaveProductAsync(product, CancellationToken.None);
            //assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("Category");

        }
        [Fact]
        public async Task SaveProductAsync_WhenNullAuthor_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFProductRepository(context);
            var product = new Product() { Id = 1, Name = "Product 1", Category = new() { Id = 1 }, Author = null, Seller = new() { Id = 1 }, Description = "Description", Price = 10M };
            //Act
            Func<Task> act = () => repository.SaveProductAsync(product, CancellationToken.None);
            //assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("Author");
        }
        [Fact]
        public async Task SaveProductAsync_WhenNullSeller_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFProductRepository(context);
            var product = new Product() { Id = 1, Name = "Product 1", Category = new() { Id = 1 }, Author = new() { Id = 1 }, Seller = null, Description = "Description", Price = 10M };
            //Act
            Func<Task> act = () => repository.SaveProductAsync(product, CancellationToken.None);
            //assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("Seller");
        }


        [Fact]
        public async Task SaveProductAsync_WhenNewProduct_AddsNewProductAndDoesNotDuplicateRelatedEntities()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            var author = new Author { Id = 1, FirstName = "Name1" };
            var seller = new Seller { Id = 1, Name = "Seller1" };
            context.ProductCategories.Add(category);
            context.Authors.Add(author);
            context.Sellers.Add(seller);
            context.SaveChanges();

            var product = new Product() { Id = 0, Name = "New Product", Category = new() { Id = 1 }, Author = new() { Id = 1 }, Seller = new() { Id = 1 }, Description = "Description", Price = 10M };
            using var context2 = new AppDbContext(options);
            var repository = new EFProductRepository(context2);
            await repository.SaveProductAsync(product, CancellationToken.None);
            var seavedProduct = context2.Products.Include(p => p.Category).First(p => p.Id == product.Id);
            seavedProduct.Should().NotBeNull();
            seavedProduct.Should().BeEquivalentTo(product);
            context2.ProductCategories.Count().Should().Be(1);
        }

        [Fact]
        private void GetProduct_WhenProductIdIsValid_ReturnsProduct()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            var author = new Author { Id = 1, FirstName = "Name1" };
            var seller = new Seller { Id = 1, Name = "Seller1" };
            var product = new Product { Id = 1, Name = "Product 1", Category = category, Author = author, Seller = seller };
            context.ProductCategories.Add(category);
            context.Authors.Add(author);
            context.Sellers.Add(seller);
            context.Products.Add(product);
            context.SaveChanges();

            var repository = new EFProductRepository(context);
            var result = repository.GetProduct(1);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        private void GetProduct_WhenProductIdIsInvalid_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            var author = new Author { Id = 1, FirstName = "Name1" };
            var seller = new Seller { Id = 1, Name = "Seller1" };
            var product = new Product { Id = 1, Name = "Product 1", Category = category, Author = author, Seller = seller };
            context.ProductCategories.Add(category);
            context.Authors.Add(author);
            context.Sellers.Add(seller);
            context.Products.Add(product);
            context.SaveChanges();

            var repository = new EFProductRepository(context);
            var result = repository.GetProduct(999);
            result.Should().BeNull();
        }

        [Fact]
        private void GetProducts_WhenProductCriteriaIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFProductRepository(context);
            Action act = () => repository.GetProduct(null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("criteria");
        }
        [Fact]
        private void GetProducts_ReturnsAllProducts_WhenCriteriaIsEmpty()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            var author = new Author { Id = 1, FirstName = "Name1" };
            var seller = new Seller { Id = 1, Name = "Seller1" };
            var product1 = new Product { Id = 1, Name = "Product 1", Category = category, Author = author, Seller = seller };
            var product2 = new Product { Id = 2, Name = "Product 2", Category = category, Author = author, Seller = seller };
            context.ProductCategories.Add(category);
            context.Authors.Add(author);
            context.Sellers.Add(seller);
            context.Products.AddRange(product1, product2);
            context.SaveChanges();
            var repository = new EFProductRepository(context);
            var result = repository.GetProduct(new ProductSearchCriteria());
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
        [Fact]
        private void GetProducts_WhenCriteriaIsValid_ReturnsProduct()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var author1 = new Author { Id = 1, FirstName = "Name1" };
            var author2 = new Author { Id = 2, FirstName = "Name2" };

            var seller1 = new Seller { Id = 1, Name = "Seller1" };
            var seller2 = new Seller { Id = 2, Name = "Seller2" };

            var productCategory1 = new ProductCategory { Id = 1, Name = "Category 1" };
            var productCategory2 = new ProductCategory { Id = 2, Name = "Category 2" };

            var product1 = new Product { Id = 1, Name = "Product 1", Category = productCategory1, Author = author1, Seller = seller1 };
            var product2 = new Product { Id = 2, Name = "Product 2", Category = productCategory2, Author = author2, Seller = seller2 };
            context.Authors.Add(author1);
            context.Authors.Add(author2);
            context.Sellers.Add(seller1);
            context.Sellers.Add(seller2);
            context.ProductCategories.Add(productCategory1);
            context.ProductCategories.Add(productCategory2);
            context.Products.Add(product1);
            context.Products.Add(product2);
            context.SaveChanges();
            var repository = new EFProductRepository(context);
            var criteria = new ProductSearchCriteria { Name = "Product 2", Author = new() { Id=2} , Seller = new() { Id=2}, Category=new() { Id=2} };
            var result = repository.GetProduct(criteria);
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().Contain(product2);
        }
    }
               
}
