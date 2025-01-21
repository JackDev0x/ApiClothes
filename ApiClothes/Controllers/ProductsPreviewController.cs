using ApiClothes.DtoModels;
using ApiClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ApiClothes.Controllers
{
    [Route("api/productPreview")]
    public class ProductsPreviewController : ControllerBase
    {
        private readonly IProductCatalog _productCatalog;

        public ProductsPreviewController(IProductCatalog productCatalog)
        {
            _productCatalog = productCatalog;
        }

        [HttpGet("getAnnById/{id}")]
        public async Task<IActionResult> GetAnnouncement([FromRoute] int id)
        {
            var announcementDto = await _productCatalog.GetById(id);


            return Ok(announcementDto);
        }



        [HttpGet("getAnnBySlug/{slug}")]
        public async Task<IActionResult> GetAnnouncementBySlug([FromRoute] string slug)
        {
            var announcementDto = await _productCatalog.GetBySlug(slug);


            return Ok(announcementDto);
        }

    }
}