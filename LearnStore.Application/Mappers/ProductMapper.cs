using LearnStore.Application.Interfaces;
using LearnStore.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class ProductMapper : IMapper<Product, ProductDto>
    {
        
        private readonly IMapper<Author,AuthorDto> _authorMapper;
        private readonly IMapper<Seller,SellerDto> _sellerMapper;
        private readonly IMapper<ProductCategory,ProductCategoryDto> _productCategoryMapper;
        public ProductMapper(IMapperRegistry mapperRegistry)
        {   
            _authorMapper= mapperRegistry.Get<Author, AuthorDto>();
            _sellerMapper= mapperRegistry.Get<Seller, SellerDto>();
            _productCategoryMapper= mapperRegistry.Get<ProductCategory, ProductCategoryDto>();
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
                FileName = product.FileName,
                FileStorageName = product.FileStorageName,
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
                FileName = productDto.FileName,
                FileStorageName = productDto.FileStorageName,
                IsActive = productDto.IsActive,
                Price = productDto.Price
            };
        }

        public object ToDto(object domain)
        {
            return ToDto((Product)domain);
        }

        public object ToDomain(object dto)
        {
            return ToDomain((ProductDto)dto);
        }
    }
}
