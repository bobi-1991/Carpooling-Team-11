using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Carpooling.Service.Dto_s.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Carpooling.BusinessLayer.Dto_s.Requests;

namespace Carpooling.Controllers.ApiControllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressApiController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IAuthValidator _authValidator;
        private readonly IMapper _mapper;
        public AddressApiController(IAddressService addressService, IAuthValidator authValidator, IMapper mapper)
        {
            _addressService = addressService;
            _authValidator = authValidator;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAllAddressesAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                var addresses = await _addressService.GetAllAsync();

                List<AddressDTO> addressDTOS = new List<AddressDTO>();
                foreach (var address in addresses)
                {
                    var addressToMap = _mapper.Map<AddressDTO>(address);
                    addressDTOS.Add(addressToMap);
                }


                return StatusCode(StatusCodes.Status200OK, addressDTOS);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDTO>> GetAddressByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggerUser = await _authValidator.ValidateCredentialAsync(credentials);
                var address = _mapper.Map<AddressDTO>(await _addressService.GetByIdAsync(id));
                return StatusCode(StatusCodes.Status200OK, address);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<AddressDTO>> CreateAddressAsync([FromHeader] string credentials, [FromBody] AddressDTO addressDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);

                Address address = _mapper.Map<Address>(addressDTO);
                Address addressToCreate = await _addressService.CreateAsync(address, loggedUser);

                return StatusCode(StatusCodes.Status201Created, addressToCreate);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressAsync(int id, [FromHeader] string credentials, [FromBody] AddressDTO addressDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                Address address = _mapper.Map<Address>(addressDTO);
                Address addressToUpdate = await _addressService.UpdateAsync(id, loggedUser, address);

                return NoContent();
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressAsync(int id, [FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                await _addressService.DeleteAsync(id, loggedUser);
                return NoContent();
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
