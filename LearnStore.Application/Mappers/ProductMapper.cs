using LearnStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class ProductMapper
    {
        
        private AuthorMapper _authorMapper = new AuthorMapper();
        private SellerMapper _sellerMapper = new SellerMapper();
        private ProductCategoryMapper _productCategoryMapper = new ProductCategoryMapper();

        public ProductDto ToDto(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Author = product.Author != null ? _authorMapper.ToDto(product.Author) : null,
                Seller = product.Seller != null ? _sellerMapper.ToDto(product.Seller) : null,
                Category = product.Category != null ? _productCategoryMapper.ToDto(product.Category) : null,
                ChildProducts = [..product.ChildProducts.Select(p=>ToDto(p))], 
                IsActive = product.IsActive,
                Price = product.Price
            };

        }

        public Product ToEntity(ProductDto productDto)
        {
            ArgumentNullException.ThrowIfNull(productDto, nameof(productDto));
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Author = productDto.Author != null ? _authorMapper.ToEntity(productDto.Author) : null,
                Seller = productDto.Seller != null ? _sellerMapper.ToEntity(productDto.Seller) : null,
                Category = productDto.Category != null ? _productCategoryMapper.ToEntity(productDto.Category) : null,
                ChildProducts = [..productDto.ChildProducts.Select(p=>ToEntity(p))],
                IsActive = productDto.IsActive,
                Price = productDto.Price
            };
        }
    }
}
