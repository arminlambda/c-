using Microsoft.AspNetCore.Mvc;
using System

namespace VehiclePriceCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceCalculatorController : ControllerBase
    {
        [HttpPost("calculate")]
        public IActionResult CalculatePrice([FromBody] PriceRequest request)
        {
            if (request.BasePrice < 0)
            {
                return BadRequest("Cena ne more biti negatvina.");
            }
            if (request.EquipmentPrice < 0)
            {
                return BadRequest("Cena ne more biti negatvina.");
            }
            if (request.VAT < 0 || request.VAT > 100)
            {
                return BadRequest("Število mora biti med (1-100).");
            }

            try
            {
                decimal baseVATAmount = request.BasePrice * request.VAT / 100;
                decimal equipmentVATAmount = request.EquipmentPrice * request.VAT / 100;

                decimal grossBasePrice = request.BasePrice + baseVATAmount;
                decimal grossEquipmentPrice = request.EquipmentPrice + equipmentVATAmount;

                decimal netTotalPrice = request.BasePrice + request.EquipmentPrice;
                decimal grossTotalPrice = grossBasePrice + grossEquipmentPrice;

                return Ok(new 
                { 
                    BaseVehiclePrice = grossBasePrice, 
                    AdditionalEquipmentPrice = grossEquipmentPrice, 
                    TotalVehiclePrice = grossTotalPrice
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"neki ni prou {ex.Message}");
            }
        }
    }

    public class PriceRequest
    {
        public decimal BasePrice { get; set; }
        public decimal EquipmentPrice { get; set; }
        public decimal VAT { get; set; } 
    }
}
