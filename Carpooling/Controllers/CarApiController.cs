﻿using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Carpooling.BusinessLayer.Dto_s.Requests;
using Humanizer;
using Microsoft.AspNetCore.Routing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System;


namespace Carpooling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarApiController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IAuthValidator _authValidator;
        private readonly IMapper _mapper;
        public CarApiController(ICarService carService, IAuthValidator authValidator, IMapper mapper)
        {
            _carService = carService;
            _authValidator = authValidator;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCarsAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                var cars = _carService.GetAllAsync();

                return StatusCode(StatusCodes.Status200OK, cars);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggerUser = await _authValidator.ValidateCredentialAsync(credentials);
                var car = await _carService.GetByIdAsync(id);
                return StatusCode(StatusCodes.Status200OK, car);
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
        [HttpGet("{filter}")]
        public async Task<IActionResult> FilterCarsAndSortAsync([FromHeader] string credentials, [FromQuery] string sortBy)
        {
            try
            {
                var loggerUser = await _authValidator.ValidateCredentialAsync(credentials);
                var filteredAndSortedCars = await _carService.FilterCarsAndSortAsync(sortBy);
                return StatusCode(StatusCodes.Status200OK, filteredAndSortedCars);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }     
            catch(EmptyListException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }


        [HttpGet("cars/{brand}/{model}")]

        public async Task<IActionResult> GetCarByBrandAndModelAsync([FromHeader] string credentials, string brand, string model)
        {
            try
            {
                var loggerUser = await _authValidator.ValidateCredentialAsync(credentials);
                var car = await _carService.GetByBrandAndModelAsync(brand, model);
                return StatusCode(StatusCodes.Status200OK, car);
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
        public async Task<ActionResult<Feedback>> CreateCarAsync([FromHeader] string credentials, [FromBody] CarDTO carDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                Car car = _mapper.Map<Car>(carDTO);
                Car carToCreate = await _carService.CreateAsync(car, loggedUser);
                return StatusCode(StatusCodes.Status201Created, carToCreate);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarAsync(int id, [FromHeader] string credentials, [FromBody] CarDTO carDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                Car car = _mapper.Map<Car>(carDTO);
                Car carToUpdate = await _carService.UpdateAsync(id, car, loggedUser);

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
        public async Task<IActionResult> DeleteCarAsync(int id, [FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                await _carService.DeleteAsync(id, loggedUser);
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
