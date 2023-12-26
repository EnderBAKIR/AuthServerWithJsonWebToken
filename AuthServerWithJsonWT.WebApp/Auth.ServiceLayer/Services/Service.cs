using Auth.CoreLayer.Repositories;
using Auth.CoreLayer.Services;
using Auth.CoreLayer.UnitOfWork;
using Auth.ServiceLayer.MapProfiles;
using Auth.SharedLibrary.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Auth.ServiceLayer.Services
{
    public class Service<Tentity, TDto> : IService<Tentity, TDto> where Tentity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Tentity> _genericRepository;

        public Service(IUnitOfWork unitOfWork, IGenericRepository<Tentity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<Tentity>(entity);
            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();


            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null)
            {
                return Response<TDto>.Fail("Id Not Found", 404, true);
            }

            var tDTO = ObjectMapper.Mapper.Map<TDto>(product);
            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isThereentity = await _genericRepository.GetByIdAsync(id);
            if (isThereentity == null)
            {
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);
            }
            _genericRepository.Remove(isThereentity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            var isThereentity = await _genericRepository.GetByIdAsync(id);
            if (isThereentity == null)
            {
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);
            }

            var updaEntity = ObjectMapper.Mapper.Map<Tentity>(entity);
            _genericRepository.Update(updaEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<Tentity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);

        }
    }
}
