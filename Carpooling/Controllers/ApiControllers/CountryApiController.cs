using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers.ApiControllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryApiController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IAuthValidator _authValidator;
        private readonly IMapper _mapper;
        public CountryApiController(ICountryService countryService, IAuthValidator authValidator, IMapper mapper)
        {
            _countryService = countryService;
            _authValidator = authValidator;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetAllCountriesAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                var countries = await _countryService.GetAllAsync();
                List<CountryDTO> countryDTOS = new List<CountryDTO>();// _mapper.Map<CountryDTO>(countries);
                foreach (var country in countries)
                {
                    var countryToMap = _mapper.Map<CountryDTO>(country);
                    countryDTOS.Add(countryToMap);
                }

                return StatusCode(StatusCodes.Status200OK, countryDTOS);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountryByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggerUser = await _authValidator.ValidateCredentialAsync(credentials);
                var country = _mapper.Map<CountryDTO>(await _countryService.GetByIdAsync(id));
                return StatusCode(StatusCodes.Status200OK, country);
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
        public async Task<ActionResult<CountryDTO>> CreateCountryAsync([FromHeader] string credentials, [FromBody] CountryDTO countryDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);

                Country country = _mapper.Map<Country>(countryDTO);
                Country countryToCreate = await _countryService.CreateAsync(country, loggedUser);

                return StatusCode(StatusCodes.Status201Created, countryToCreate);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountryAsync(int id, [FromHeader] string credentials, [FromBody] CountryDTO countryDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                Country country = _mapper.Map<Country>(countryDTO);
                Country countryToUpdate = await _countryService.UpdateAsync(id, country, loggedUser);

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
        public async Task<IActionResult> DeleteCountryAsync(int id, [FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                await _countryService.DeleteAsync(id, loggedUser);
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
