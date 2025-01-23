using ApiClothes.DtoModels;
using ApiClothes.RequestsModels;
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
        private readonly IAccountManager _accountManager;
        private readonly ILogger<ProductsPreviewController> _logger;

        public ProductsPreviewController(IProductCatalog productCatalog, IAccountManager accountManager, ILogger<ProductsPreviewController> logger)
        {
            _productCatalog = productCatalog;
            _accountManager = accountManager;
            _logger = logger;
        }

        [HttpGet("getAnnById/{id}")]
        public async Task<IActionResult> GetAnnouncement([FromRoute] int id)
        {
            var announcementDto = await _productCatalog.GetById(id);


            return Ok(announcementDto);
        }
        [HttpGet("getCommentsByAnnId/{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            var comments = await _productCatalog.GetCommentsByAnnId(id);


            return Ok(comments);
        }

        [HttpGet("getAnnByUsrId/{id}")]
        public async Task<IActionResult> GetAnnByUsrId([FromRoute] int id)
        {
            var anss = await _productCatalog.GetAnnsByUsrId(id);


            return Ok(anss);
        }

        [HttpGet("GetFvAnnsByUsrId/{id}")]
        public async Task<IActionResult> GetFvAnnsByUsrId([FromRoute] int id)
        {
            var comments = await _accountManager.GetFvAnnsByUsrId(id);


            return Ok(comments);
        }



        [HttpGet("getAnnBySlug/{slug}")]
        public async Task<IActionResult> GetAnnouncementBySlug([FromRoute] string slug)
        {
            var announcementDto = await _productCatalog.GetBySlug(slug);


            return Ok(announcementDto);
        }

        [HttpGet("getAnns")]
        public async Task<IActionResult> GetAnnouncementsList([FromQuery] PaginationParameters paginationParameters)
        {
            _logger.LogInformation("GetAnnouncementsList called with PageNumber: {PageNumber}, PageSize: {PageSize}",
                                   paginationParameters.PageNumber, paginationParameters.PageSize);

            var result = await _productCatalog.GetAnnouncementsAsync(paginationParameters);

            _logger.LogInformation("Returning {TotalCount} announcements (Page {CurrentPage}/{TotalPages})",
                                   result.TotalCount, result.CurrentPage, result.TotalPages);

            return Ok(result);
        }


    }
}