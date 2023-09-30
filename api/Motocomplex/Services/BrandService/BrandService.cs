﻿using AutoMapper;
using Motocomplex.Data.Repositories.BrandRepository;
using Motocomplex.DTOs.BrandDTOs;
using Motocomplex.DTOs.SharedDTOs;
using Motocomplex.Entities;
using Sieve.Models;

namespace Motocomplex.Services.BrandService
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BrandDisplayDto> GetBrandById(Guid brandId)
        {
            var brand = await _brandRepository.GetBrandById(brandId);
            return _mapper.Map<BrandDisplayDto>(brand);
        }

        public async Task<RespondListDto<BrandDisplayDto>> GetBrands(SieveModel query)
        {
            int pageSize = query.PageSize != null ? (int)query.PageSize : 40;

            var brands = await _brandRepository.GetBrands(query);
            var brandsDto = _mapper.Map<List<BrandDisplayDto>>(brands);

            RespondListDto<BrandDisplayDto> respondListDto = new RespondListDto<BrandDisplayDto>();
            respondListDto.Items = brandsDto;
            respondListDto.ItemsCount = await _brandRepository.GetBrandsCount(query);
            respondListDto.PagesCount = (int)Math.Ceiling((double)respondListDto.ItemsCount / pageSize);

            return respondListDto;
        }

        public async Task<BrandDisplayDto> CreateBrand(BrandCreateDto brandDto)
        {
            var brand = _mapper.Map<Brand>(brandDto);
            await _brandRepository.CreateBrand(brand);

            return _mapper.Map<BrandDisplayDto>(brand);
        }

        public async Task<BrandDisplayDto> UpdateBrand(BrandUpdateDto brandDto)
        {
            var brand = _mapper.Map<Brand>(brandDto);
            await _brandRepository.UpdateBrand(brand);

            return _mapper.Map<BrandDisplayDto>(brand);
        }
    }
}
