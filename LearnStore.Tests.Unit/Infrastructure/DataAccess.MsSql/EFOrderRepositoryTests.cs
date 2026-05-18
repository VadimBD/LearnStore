using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Infrastructure.DataAccess.MsSql
{
    public class EFOrderRepositoryTests
    {
        [Fact]
        public async Task DeleteOrderAsync_WhenIdMatches_RemovesExistingOrder()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer = new Customer() { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" };
            context.Customers.Add(customer);
            var orderItem = new OrderItem() { Id = 1, Product = new() { Id = 1 }, PriceAtOrderTime = 1223, Quantity = 1 };
            var orderItem2 = new OrderItem() { Id = 2, Product = new() { Id = 2 }, PriceAtOrderTime = 1223, Quantity = 1 };
            context.OrderItems.Add(orderItem2);
            context.OrderItems.Add(orderItem);
            var payment1 = new Payment() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow };
            var payment2 = new Payment() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow };
            var orderId = Guid.NewGuid();
            var targetOrderId = Guid.NewGuid();

            context.Orders.Add(new() { Id = orderId, Customer = customer, Items = new List<OrderItem>() { orderItem }, Payments = new List<Payment>() { payment1 } });
            context.Orders.Add(new() { Id = targetOrderId, Customer = customer, Items = new List<OrderItem>() { orderItem2 }, Payments = new List<Payment>() { payment2 } });
            context.SaveChanges();
            context.SaveChanges();

            var repository = new EFOrderRepository(context);

            await repository.DeleteOrderAsync(targetOrderId, CancellationToken.None);
            context.Orders.Should().HaveCount(1);
            var deletedOrder = context.Orders.Find(targetOrderId);
            deletedOrder.Should().BeNull();
            var existingOrder = context.Orders.Find(orderId);
            existingOrder.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteOrderAsync_WhenOrderDoesNotExist_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFOrderRepository(context);
            var nonExistentOrderId = Guid.NewGuid();
            // Act
            var result = await repository.DeleteOrderAsync(nonExistentOrderId, CancellationToken.None);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetOrdersAsync_WhenOrderIdMatch_ReturnsOneOrder()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer = new Customer() { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" };
            context.Customers.Add(customer);
            var orderItem = new OrderItem() { Id = 1, Product = new() { Id = 1 }, PriceAtOrderTime = 1223, Quantity = 1 };
            var orderItem2 = new OrderItem() { Id = 2, Product = new() { Id = 2 }, PriceAtOrderTime = 1223, Quantity = 1 };
            context.OrderItems.Add(orderItem2);
            context.OrderItems.Add(orderItem);
            var payment1 = new Payment() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow };
            var payment2 = new Payment() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow };
            var orderId = Guid.NewGuid();
            var targetOrderId = Guid.NewGuid();

            context.Orders.Add(new() { Id = orderId, Customer = customer, Items = new List<OrderItem>() { orderItem }, Payments = new List<Payment>() { payment1 } });
            context.Orders.Add(new() { Id = targetOrderId, Customer = customer, Items = new List<OrderItem>() { orderItem2 }, Payments = new List<Payment>() { payment2 } });
            context.SaveChanges();
            var repository = new EFOrderRepository(context);

            var criteria = new OrderSearchCriteria() { OrderId = targetOrderId };
            var result = await repository.GetOrdersAsync(criteria, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().ContainSingle(o => o.Id == targetOrderId);
        }

        [Fact]
        public async Task GetOrdersAsync_WhenCustomerIdMatch_ReturnsOrders()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var customer = new Customer() { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" };
            var customer2 = new Customer() { Id = "2", Name = "Test2", EmailAddress = "test2", PhoneNumber = "+2312314" };
            var customer3 = new Customer() { Id = "3", Name = "Test3", EmailAddress = "test3", PhoneNumber = "+2312314" };
            context.Customers.AddRange(customer, customer2, customer3);
            var product = new Product() { Id = 1, Name = "TestProduct1" };
            var product2 = new Product() { Id = 2, Name = "TestProduct2" };
            var product3 = new Product() { Id = 3, Name = "TestProduct3" };
            context.Products.AddRange(product, product2, product3);
            context.SaveChanges();

            context.Orders.Add(new()
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                Items = new List<OrderItem>()
                {
                    new(){ Id = 1,Product = product, PriceAtOrderTime = 1223, Quantity = 2 }
                },
                Payments = new List<Payment>() { new() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow } }
            });

            context.Orders.Add(new()
            {
                Id = Guid.NewGuid(),
                Customer = customer2,
                Items = new List<OrderItem>()
                {
                    new() { Id = 2, Product = product2, PriceAtOrderTime = 1223, Quantity = 3 }
                },
                Payments = new List<Payment>() { new() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow } }
            });

            context.Orders.Add(new()
            {
                Id = Guid.NewGuid(),
                Customer = customer3,
                Items = new List<OrderItem>()
                {
                    new() { Id = 3, Product = product3, PriceAtOrderTime = 1223, Quantity = 1 }
                },
                Payments = new List<Payment>() { new() { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow } }
            });

            context.SaveChanges();
            var repository = new EFOrderRepository(context);
            var result = await repository.GetOrdersAsync(new() { CustomerId = "2" }, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().ContainSingle(o => o.Customer.Id == "2");
        }



        [Fact]
        public async Task GetOrdersAsync_WhenNoCriteriaProvided_ReturnsAllOrders()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFOrderRepository(context);
            //Act
            var result = await repository.GetOrdersAsync(new() { OrderId = Guid.NewGuid() }, CancellationToken.None);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetOrdersAsync_WhenNoOrdersMatchCriteria_ReturnsEmptyList()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFOrderRepository(context);
            //Act
            var result = await repository.GetOrdersAsync(new() { OrderId = Guid.NewGuid() }, CancellationToken.None);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty(); ;
        }

        [Fact]
        public async Task GetOrdersAsync_WhenMultipleCriteriaMatch_ReturnsOrders()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var context = new AppDbContext(options);

            var customer = new Customer { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" };
            context.Customers.Add(customer);

            var product = new Product { Id = 1, Name = "TestProduct1" };
            context.Products.Add(product);

            context.SaveChanges();

            var orderId = Guid.NewGuid();

            context.Orders.Add(new Order
            {
                Id = orderId,
                Customer = customer,
                State = OrderState.New,
                Items = new List<OrderItem>
        {
            new()
            {
                Id = 1,
                Product = product,
                Quantity = 2,
                PriceAtOrderTime = 1223
            }
        },
                Payments = new List<Payment>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 1223,
                PaymentDate = DateTime.UtcNow
            }
        }
            });

            context.SaveChanges();

            var repository = new EFOrderRepository(context);

            var criteria = new OrderSearchCriteria
            {
                OrderId = orderId,
                CustomerId = "1"
            };

            // Act
            var result = await repository.GetOrdersAsync(criteria, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().OnlyContain(o =>
                o.Id == orderId &&
                o.Customer != null &&
                o.Customer.Id == "1");
        }

        [Fact]
        public async Task UpdateOrderStageAsync_WhenOrderFound_UpdatesOrderStageSuccessfully()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            var repository = new EFOrderRepository(context);

            var orderIdTarget = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var customer = new Customer
            {
                Id = "1",
                Name = "Test",
                EmailAddress = "test@test.com",
                PhoneNumber = "123"
            };

            context.Customers.Add(customer);

            context.Orders.Add(new Order
            {
                Id = orderIdTarget,
                State = OrderState.New,
                Customer = customer
            });

            context.Orders.Add(new Order
            {
                Id = orderId,
                State = OrderState.New,
                Customer = customer
            });

            context.SaveChanges();

            await repository.UpdateOrderStageAsync(
                orderIdTarget,
                OrderState.Completed,
                CancellationToken.None);

            var updatedOrder = context.Orders.Find(orderIdTarget);

            updatedOrder.Should().NotBeNull();
            updatedOrder!.State.Should().Be(OrderState.Completed);

            var order = context.Orders.Find(orderId);

            order.Should().NotBeNull();
            order!.State.Should().Be(OrderState.New);
        }
        [Fact]
        public async Task UpdateOrderStageAsync_WhenOrderNotFound_DoesNothing()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            var repository = new EFOrderRepository(context);

            var orderId = Guid.NewGuid();

            context.Orders.Add(new Order
            {
                Id = orderId,
                Customer = new Customer { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" },
                State = OrderState.New
            });

            context.SaveChanges();

            await repository.UpdateOrderStageAsync(
                Guid.NewGuid(),
                OrderState.Completed,
                CancellationToken.None);

            var order = context.Orders.Find(orderId);

            order.Should().NotBeNull();
            order!.State.Should().Be(OrderState.New);
        }

        [Fact]
        public async Task SaveOrderAsync_WhenOrderIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);
            var repository = new EFOrderRepository(context);
            // Act
            Func<Task> act = () => repository.SaveOrderAsync(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("order");
        }

        [Fact]
        public async Task SaveOrderAsync_WhenOrderDoesNotExist_AddsNewOrder()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);


            var customer = new Customer { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" };
            context.Customers.Add(customer);


            var product = new Product { Id = 1 };
            context.Products.Add(product);


            var existingItem = new OrderItem
            {
                Id = 1,
                Product = product,
                PriceAtOrderTime = 1223,
                Quantity = 1
            };
            context.OrderItems.Add(existingItem);

            await context.SaveChangesAsync();

            var orderRepository = new EFOrderRepository(context);

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                Items = new List<OrderItem> { existingItem },
                Payments = new List<Payment>
        {
            new Payment { Id = Guid.NewGuid(), Amount = 1223, PaymentDate = DateTime.UtcNow }
        },
                State = OrderState.New
            };

            await orderRepository.SaveOrderAsync(newOrder, CancellationToken.None);

            var existingOrder = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Payments)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == newOrder.Id);

            existingOrder.Should().NotBeNull();
            existingOrder!.Customer.Should().NotBeNull();
            existingOrder.Payments.Should().NotBeNull();
            existingOrder.Items.Should().NotBeNull();
            existingOrder.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveOrderAsync_WhenOrderIdMatch_UpdatesExistingOrder()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var context = new AppDbContext(options);

            var customer = new Customer { Id = "1", Name = "Test", EmailAddress = "test", PhoneNumber = "+2312314" };

            context.Customers.Add(customer);

            var product = new Product { Id = 1, Name = "Product1" };
            var product2 = new Product { Id = 2, Name = "Product2" };

            context.Products.AddRange(product, product2);

            var orderId = Guid.NewGuid();
            var paymentId1 = Guid.NewGuid();
            var paymentId2 = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Customer = customer,
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1, Product = product, PriceAtOrderTime = 1223, Quantity = 2 },
                    new OrderItem { Id = 2, Product = product2, PriceAtOrderTime = 1223, Quantity = 2 }
                },
                Payments = new List<Payment>
                {
                    new() { Id = paymentId1, Amount = 1223, PaymentDate = DateTime.UtcNow },
                    new (){ Id = paymentId2, Amount = 1223, PaymentDate = DateTime.UtcNow }
                },
                State = OrderState.New
            };

            context.Orders.Add(order);
            context.SaveChanges();

            using var context2 = new AppDbContext(options);
            var repository = new EFOrderRepository(context2);
            var updatedOrder = new Order
            {
                Id = orderId,
                Customer = customer,
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1, Product = product2, PriceAtOrderTime = 67, Quantity = 1 },
                    new OrderItem { Id = 0, Product = product, PriceAtOrderTime = 1223, Quantity = 2 }
                },
                Payments = new List<Payment>
                {
                    new Payment { Id = paymentId1, Amount = 32453, PaymentDate = DateTime.UtcNow },
                    new Payment { Id = Guid.Empty, Amount = 1223, PaymentDate = DateTime.UtcNow }
                },
                State = OrderState.Pending
            };


            await repository.SaveOrderAsync(updatedOrder, CancellationToken.None);

            var savedOrder = context2.Orders.Find(orderId);

            savedOrder.Should().NotBeNull();
            savedOrder.Id.Should().Be(orderId);
            savedOrder.Customer.Should().NotBeNull();
            savedOrder.Customer.Should().BeEquivalentTo(customer);
            savedOrder.Payments.Should().NotBeNull();

            savedOrder.Items.Should().NotBeNull();
            savedOrder.Items.Should().BeEquivalentTo(updatedOrder.Items);
            savedOrder.Items.Should().HaveCount(updatedOrder.Items.Count);
        }


        [Fact]
        public async Task SaveOrderAsync_WhenOrderIsValid_AddsNewOrderAndDoesNotDuplicateRelatedEntities()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            var customer = new Customer
            {
                Id = "1",
                Name = "Customer1",
                EmailAddress = "email@test.com",
                PhoneNumber = "123"
            };

            var product1 = new Product
            {
                Id = 1,
                Name = "Product1"
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Product2"
            };

            context.Customers.Add(customer);
            context.Products.AddRange(product1, product2);
            context.SaveChanges();

            using var context2 = new AppDbContext(options);
            var repository = new EFOrderRepository(context2);

            var order = new Order
            {
                Id = Guid.NewGuid(),


                Customer = new Customer
                {
                    Id = "1",
                    Name = "Customer1",
                    EmailAddress = "email@test.com",
                    PhoneNumber = "123"
                },

                Items = new List<OrderItem> {
                 new (){
                Id = 1,
                Product = new Product { Id = 1, Name = "Product1" },
                Quantity = 2,
                PriceAtOrderTime = 10 },

                new () {
                Id = 2,
                Product = new Product { Id = 2, Name = "Product2" },
                Quantity = 1,
                PriceAtOrderTime = 20} },

                Payments = new List<Payment>
                {
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        Amount = 30,
                         PaymentDate = DateTime.UtcNow
                       } },
                State = OrderState.New
            };


            await repository.SaveOrderAsync(order, CancellationToken.None);


            var savedOrder = context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.Payments)
                .FirstOrDefault(o => o.Id == order.Id);

            savedOrder.Should().NotBeNull();

            savedOrder.Customer.Should().NotBeNull();
            savedOrder.Items.Should().HaveCount(2);
            savedOrder.Payments.Should().HaveCount(1);


            context.Customers.Count().Should().Be(1);
            context.Products.Count().Should().Be(2);
        }


    }
}
