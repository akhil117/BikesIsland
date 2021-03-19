using BikesIsland.Configurations.Models;
using BikesIsland.Integrations.Interfaces;
using BikesIsland.Integrations.Interfaces.Storage;
using BikesIsland.Models.Dto;
using BikesIsland.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BikesIsland.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BikeController : ControllerBase
    {
        private readonly IDataRepository<Bike> _dataRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly Configure _configure;
        public BikeController(IOptions<Configure> configure,IBlobStorageService blobStorageService, IDataRepository<Bike> dataRepository)
        {
            _configure = configure.Value;
            _dataRepository = dataRepository;
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Uploading a image
        /// </summary>
        [HttpPost("/upload")]
        public async Task<IActionResult> UploadBlob([FromForm] BikeDto bikeDetails)
        {
            try
            {
                string attachmentUrl = string.Empty;
                Bike bike = null;
                if (!string.IsNullOrEmpty(bikeDetails.image.FileName))
                {
                    string fileName = $"{Guid.NewGuid()}-{bikeDetails.image.FileName}";
                    string fileTempPath = @$"{Path.GetTempPath()}{fileName}";


                    using var stream = new FileStream(fileTempPath, FileMode.Create, FileAccess.ReadWrite);
                    await bikeDetails.image.CopyToAsync(stream);
                    attachmentUrl = await _blobStorageService.UploadBlobAsync(stream, fileName);
                }
                if (!string.IsNullOrEmpty(attachmentUrl))
                {
                    bike = new Bike()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImageUrl = attachmentUrl,
                        Brand = bikeDetails.Brand,
                        Location = bikeDetails.Location,
                        Model = bikeDetails.Model,
                        PricePerDay = bikeDetails.PricePerDay
                    };
                }
                var createdQuery = await _dataRepository.AddAsync(bike);
                return Ok(createdQuery);
            }
            catch (Exception e)
            {
                return Ok("Transaction Failed");
            }
        }


        [HttpGet("/success")]
        public async Task<IActionResult> showSuccessLog()
        {
            Log.Information("Success");
            return Ok("Success");
        }

        [HttpGet("/failed")]
        public async Task<IActionResult> showErrorLog()
        {
            Log.Error("Error");
            return BadRequest("Error");
        }

        [HttpGet("/exception")]
        public async Task<IActionResult> throwExceptionWithLog()
        {
            Log.Error("API IS NOT showing any response");
            try
            {
                throw new Exception("API IS NOT showing any response");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal server error");
            }

        }


        [HttpGet("/string")]
        public async Task<IActionResult> returnCString()
        {
            try
            {
                return Ok(_configure.CosmosDbSettings.ConnectionString);
            }catch(Exception ex)
            {
                return NotFound("not able to fetch");
            }
        }
    }
}
