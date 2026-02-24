using LearnStore.Application.Interfaces;
using LearnStore.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class ProductMapper
    {
        
        private readonly IMapper<Author,AuthorDto> _authorMapper;
        private readonly IMapper<Seller,SellerDto> _sellerMapper;
        private readonly IMapper<ProductCategory,ProductCategoryDto> _productCategoryMapper;
        public ProductMapper(IEnumerable<IMapper> mappers)
        {   
            _authorMapper= mappers.OfType<IMapper<Author, AuthorDto>>().FirstOrDefault() ?? throw new ArgumentException("Author mapper not found", nameof(mappers));
            _sellerMapper= mappers.OfType<IMapper<Seller, SellerDto>>().FirstOrDefault() ?? throw new ArgumentException("Seller mapper not found", nameof(mappers));
            _productCategoryMapper= mappers.OfType<IMapper<ProductCategory, ProductCategoryDto>>().FirstOrDefault() ?? throw new ArgumentException("ProductCategory mapper not found", nameof(mappers));
        }

        public ProductDto ToDto(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Author = product.Author is not null ? _authorMapper.ToDto(product.Author) : null,
                Seller = product.Seller is not null ? _sellerMapper.ToDto(product.Seller) : null,
                Category=product.Category is not null ? _productCategoryMapper.ToDto(product.Category) : null,
                ChildProducts = [..product.ChildProducts.Select(p=>ToDto(p))], 
                IsActive = product.IsActive,
                Price = product.Price
            };

        }

        public Product ToDomain(ProductDto productDto)
        {
            ArgumentNullException.ThrowIfNull(productDto, nameof(productDto));
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Author = productDto.Author is not null ? _authorMapper.ToDomain(productDto.Author) : null,
                Seller = productDto.Seller is not null ? _sellerMapper.ToDomain(productDto.Seller) : null,
                Category = productDto.Category is not null ? _productCategoryMapper.ToDomain(productDto.Category) : null,
                ChildProducts = [..productDto.ChildProducts.Select(p=>ToDomain(p))],
                IsActive = productDto.IsActive,
                Price = productDto.Price
            };
        }
    }
}
