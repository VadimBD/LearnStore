using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class ProductCategoryMapper : IMapper<ProductCategory, ProductCategoryDto>
    {
        public ProductCategory ToDomain(ProductCategoryDto productCategoryDto)
        {
            ArgumentNullException.ThrowIfNull(productCategoryDto, nameof(productCategoryDto));
            return new ProductCategory
            {
                Id = productCategoryDto.Id,
                Name = productCategoryDto.Name
            };
        }

        public object ToDomain(object dto)
        {
            return ToDomain((ProductCategoryDto)dto);
        }

        public ProductCategoryDto ToDto(ProductCategory productCategory)
        {
            ArgumentNullException.ThrowIfNull(productCategory, nameof(productCategory));
            return new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name
            };
        }

        public object ToDto(object domain)
        {
           return ToDto((ProductCategory)domain);
        }
    }
}
