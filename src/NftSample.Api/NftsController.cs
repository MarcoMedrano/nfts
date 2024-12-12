using NftSample.Api.Authorization;
using NftSample.Infraestructure.Api;
using NftSample.Dtos.Nft;
using Microsoft.AspNetCore.Mvc;

namespace NftSample.Api;

[ApiController]
[Route("api/[controller]")]
public class NftsController(INftService service, ILogger<NftsController> logger) : ControllerBase, INftsController
{
    [HttpGet("{id}")]
    public async Task<NftDto?> GetById(long id) => (await service.GetById(id)).Adapt<NftDto>();

    [HttpGet]
    public async Task<IEnumerable<NftDto>?> Get() => (await service.GetAll()).Adapt<IEnumerable<NftDto>>();

    [Authorize]
    [HttpPost]
    public Task<long> Add(NftCreateDto dto)
    {
        var nft = dto.Adapt<Nft>();
        nft.UserId = SiweJwtMiddleware.GetAddressFromContext(HttpContext);

        return service.Add(nft);
    }

    [Authorize]
    [HttpPatch]
    public Task Update(NftDto dto) => service.Update(dto.Adapt<Nft>());

    [Authorize]
    [HttpDelete("{id}")]
    public Task Delete(long id) => service.Delete(id);
}