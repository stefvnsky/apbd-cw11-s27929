using EFCodeFirst.DTOs;
using EFCodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PrescriptionsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription(PrescriptionRequestDto dto)
        {
            try
            {
                await _dbService.AddPrescriptionAsync(dto); //dodanie recepty
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }    
}
