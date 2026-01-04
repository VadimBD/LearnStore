using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class ProductCategoryMapper
    {
        public ProductCategoryDto ToDto(ProductCategory productCategory)
        {
            ArgumentNullException.ThrowIfNull(productCategory, nameof(productCategory));
            return new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name
            };
        }

        public ProductCategory ToEntity(ProductCategoryDto productCategoryDto)
        {
            ArgumentNullException.ThrowIfNull(productCategoryDto, nameof(productCategoryDto));
            return new ProductCategory
            {
                Id = productCategoryDto.Id,
                Name = productCategoryDto.Name
            };
        }
    }
}
