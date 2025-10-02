using AutoMapper;
using RealEstate.Application.Modules.Owner.DTOs;
using RealEstate.Application.Modules.Owner.Interfaces;
using RealEstate.Domain.Modules.Owner.Entities;
using RealEstate.Domain.Modules.Owner.Interfaces;

namespace RealEstate.Application.Modules.Owner.Services
{
    public class OwnerService(IOwnerRepository ownerRepository, IMapper mapper) : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
        {
            var owners = await _ownerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }

        public async Task<OwnerDto?> GetOwnerByIdAsync(string id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            return owner is null ? null : _mapper.Map<OwnerDto>(owner);
        }

        public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto)
        {
            var owner = _mapper.Map<OwnerEntity>(createOwnerDto);
            await _ownerRepository.CreateAsync(owner);
            return _mapper.Map<OwnerDto>(owner);
        }

        public async Task UpdateOwnerAsync(string id, UpdateOwnerDto updateOwnerDto)
        {
            var existing = await _ownerRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Owner with Id {id} not found.");
            _mapper.Map(updateOwnerDto, existing);
            await _ownerRepository.UpdateAsync(id, existing);
        }

        public async Task DeleteOwnerAsync(string id)
        {
            var existing = await _ownerRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Owner with Id {id} not found.");
            await _ownerRepository.DeleteAsync(id);
        }

        public async Task<bool> OwnerExistsAsync(string id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            return owner is not null;
        }
    }
}
