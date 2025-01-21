//using ApiClothes.DtoModels;
//using ApiClothes.Entities;
//using ApiClothes.Services.Interfaces;
//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;

//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using ApiClothes.Entities;
//using ApiClothes.DtoModels;
//using ApiClothes.RequestsModels;
//using ApiClothes.Services.Services;
//using ApiClothes.Services.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.JsonWebTokens;
//using RestSharp;
//using System.Formats.Asn1;
//using System.Globalization;
//using System.Linq;
//using System.Security.Claims;


//[Route("api/filters")]
//public class FiltersController : ControllerBase
//{
//    private readonly PlatformDbContext _dbContext;
//    private readonly IProductCatalog _productCatalog;
//    private readonly IMapper _mapper;

//    public FiltersController(PlatformDbContext dbContext, IProductCatalog productCatalog, IMapper mapper)
//    {
//        _dbContext = dbContext;
//        _productCatalog = productCatalog;
//        _mapper = mapper;
//    }

//    private string ToKebabCase(string input)
//    {
//        return string.IsNullOrEmpty(input) ? input : string.Join("-", input.Split(' ').Select(s => s.ToLower()));
//    }

//    [HttpPost("filterAnn")]
//    public async Task<IActionResult> FilterAnnouncements([FromBody] FilterRequest filter)
//    {
//        var query = _dbContext.Announcements
//            .AsNoTracking()
//            .Include(a => a.User)
//            .Include(a => a.Images)
//            .AsQueryable();

//        if (filter.Brands != null && filter.Brands.Any())
//        {
//            var kebabCaseBrands = filter.Brands.Select(b => ToKebabCase(b)).ToList();
//            query = query.Where(a => filter.Brands.Contains(a.Brand) || filter.Brands.Contains(a.Brand.ToLower().Replace(" ", "-")));
//        }

//        if (filter.Models != null && filter.Models.Any())
//        {
//            var kebabCaseModels = filter.Models.Select(m => ToKebabCase(m)).ToList();
//            query = query.Where(a => filter.Models.Contains(a.Model) || filter.Models.Contains(a.Model.ToLower().Replace(" ", "-")));
//        }

//        if (filter.MinYear.HasValue)
//        {
//            query = query.Where(a => a.ProductionYear >= filter.MinYear.Value);
//        }

//        if (filter.MaxYear.HasValue)
//        {
//            query = query.Where(a => a.ProductionYear <= filter.MaxYear.Value);
//        }

//        if (filter.MinMileage.HasValue)
//        {
//            query = query.Where(a => a.Mileage >= filter.MinMileage.Value);
//        }

//        if (filter.MaxMileage.HasValue)
//        {
//            query = query.Where(a => a.Mileage <= filter.MaxMileage.Value);
//        }

//        if (filter.MinPrice.HasValue)
//        {
//            query = query.Where(a => a.Price >= filter.MinPrice.Value);
//        }

//        if (filter.MaxPrice.HasValue)
//        {
//            query = query.Where(a => a.Price <= filter.MaxPrice.Value);
//        }


//        if (filter.BodyTypes != null && filter.BodyTypes.Any())
//        {
//            query = query.Where(a => filter.BodyTypes.Contains(a.BodyType));
//        }


//        if (filter.MinPower.HasValue)
//        {
//            query = query.Where(a => a.Power >= filter.MinPower.Value);
//        }

//        if (filter.MaxPower.HasValue)
//        {
//            query = query.Where(a => a.Power <= filter.MaxPower.Value);
//        }
//        if (filter.MultimediaFeatures != null && filter.MultimediaFeatures.Any())
//        {
//            foreach (var featureId in filter.MultimediaFeatures)
//            {
//                query = query.Where(a => a.Multimedia.Any(m => m.featureId == featureId));
//            }
//        }

//        if (filter.SafetyFeatures != null && filter.SafetyFeatures.Any())
//        {
//            foreach (var featureId in filter.SafetyFeatures)
//            {
//                query = query.Where(a => a.Safety.Any(s => s.featureId == featureId));
//            }
//        }

//        if (filter.DriverAssistanceSystemsFeatures != null && filter.DriverAssistanceSystemsFeatures.Any())
//        {
//            foreach (var featureId in filter.DriverAssistanceSystemsFeatures)
//            {
//                query = query.Where(a => a.DriverAssistanceSystems.Any(d => d.featureId == featureId));
//            }
//        }

//        if (filter.PerformanceFeatures != null && filter.PerformanceFeatures.Any())
//        {
//            foreach (var featureId in filter.PerformanceFeatures)
//            {
//                query = query.Where(a => a.Performance.Any(p => p.featureId == featureId));
//            }
//        }

//        if (filter.OtherFeatures != null && filter.OtherFeatures.Any())
//        {
//            foreach (var featureId in filter.OtherFeatures)
//            {
//                query = query.Where(a => a.Other.Any(o => o.featureId == featureId));
//            }
//        }

//        var announcements = await query.ToListAsync();

//        var projectedResults = announcements.Select(a => new AnnPreview
//        {
//            Id = a.AnId,
//            Slug = a.Slug,
//            Brand = a.Brand,
//            Model = a.Model,
//            Description = a.Description,
//            summary = a.Summary,
//            User = new UserDto
//            {
//                UserId = a.User.UserId,
//                Name = a.User.Name,
//                Surname = a.User.Surname,
//                Phone = a.User.Phone,
//                Email = a.User.Email,
//                lat = a.User.lat,
//                lng = a.User.lng,
//                Voivodeship = a.User.Voivodeship,
//                City = a.User.City,
//            },
//            Price = a.Price,
//            Power = a.Power,
//            Engine = a.Engine ?? string.Empty,
//            FuelType = a.FuelType,
//            Mileage = a.Mileage,
//            ProductionYear = a.ProductionYear,
//            LikedBy = _dbContext.FavoriteAnnouncements
//                        .Where(fa => fa.AnnouncementAnId == a.AnId)
//                        .Select(fa => fa.UserId)
//                        .ToList(),
//            Images = a.Images.Select(i => new AnnouncementImagesDto
//            {
//                AnId = i.AnId,
//                ImageUrl = i.ImageUrl
//            }).ToList()
//        }).ToList();

//        return Ok(projectedResults);
//    }



//    [HttpPost("getFilteredAnnCount")]
//    public async Task<IActionResult> FilterAnnouncementsCount([FromBody] FilterRequest filter)
//    {
//        var query = _dbContext.Announcements
//            .AsNoTracking()
//            .Include(a => a.User)
//            .Include(a => a.Images)
//            .AsQueryable();

//        if (filter.Brands != null && filter.Brands.Any())
//        {
//            var kebabCaseBrands = filter.Brands.Select(b => ToKebabCase(b)).ToList();
//            query = query.Where(a => filter.Brands.Contains(a.Brand) || filter.Brands.Contains(a.Brand.ToLower().Replace(" ", "-")));
//        }

//        if (filter.Models != null && filter.Models.Any())
//        {
//            var kebabCaseModels = filter.Models.Select(m => ToKebabCase(m)).ToList();
//            query = query.Where(a => filter.Models.Contains(a.Model) || filter.Models.Contains(a.Model.ToLower().Replace(" ", "-")));
//        }

//        if (filter.MinYear.HasValue)
//        {
//            query = query.Where(a => a.ProductionYear >= filter.MinYear.Value);
//        }

//        if (filter.MaxYear.HasValue)
//        {
//            query = query.Where(a => a.ProductionYear <= filter.MaxYear.Value);
//        }

//        if (filter.MinMileage.HasValue)
//        {
//            query = query.Where(a => a.Mileage >= filter.MinMileage.Value);
//        }

//        if (filter.MaxMileage.HasValue)
//        {
//            query = query.Where(a => a.Mileage <= filter.MaxMileage.Value);
//        }

//        if (filter.MinPrice.HasValue)
//        {
//            query = query.Where(a => a.Price >= filter.MinPrice.Value);
//        }

//        if (filter.MaxPrice.HasValue)
//        {
//            query = query.Where(a => a.Price <= filter.MaxPrice.Value);
//        }

//        if (filter.BodyTypes != null && filter.BodyTypes.Any())
//        {
//            query = query.Where(a => filter.BodyTypes.Contains(a.BodyType));
//        }

//        if (filter.MinPower.HasValue)
//        {
//            query = query.Where(a => a.Power >= filter.MinPower.Value);
//        }

//        if (filter.MaxPower.HasValue)
//        {
//            query = query.Where(a => a.Power <= filter.MaxPower.Value);
//        }

//        if (filter.MultimediaFeatures != null && filter.MultimediaFeatures.Any())
//        {
//            foreach (var featureId in filter.MultimediaFeatures)
//            {
//                query = query.Where(a => a.Multimedia.Any(m => m.featureId == featureId));
//            }
//        }

//        if (filter.SafetyFeatures != null && filter.SafetyFeatures.Any())
//        {
//            foreach (var featureId in filter.SafetyFeatures)
//            {
//                query = query.Where(a => a.Safety.Any(s => s.featureId == featureId));
//            }
//        }

//        if (filter.DriverAssistanceSystemsFeatures != null && filter.DriverAssistanceSystemsFeatures.Any())
//        {
//            foreach (var featureId in filter.DriverAssistanceSystemsFeatures)
//            {
//                query = query.Where(a => a.DriverAssistanceSystems.Any(d => d.featureId == featureId));
//            }
//        }

//        if (filter.PerformanceFeatures != null && filter.PerformanceFeatures.Any())
//        {
//            foreach (var featureId in filter.PerformanceFeatures)
//            {
//                query = query.Where(a => a.Performance.Any(p => p.featureId == featureId));
//            }
//        }

//        if (filter.OtherFeatures != null && filter.OtherFeatures.Any())
//        {
//            foreach (var featureId in filter.OtherFeatures)
//            {
//                query = query.Where(a => a.Other.Any(o => o.featureId == featureId));
//            }
//        }

//        var results = await query
//            .Select(a => new AnnPreview
//            {
//                Id = a.AnId,
//                Slug = a.Slug,
//                Brand = a.Brand,
//                Model = a.Model,
//                Description = a.Description,
//                summary = a.Summary,
//                User = new UserDto
//                {
//                    UserId = a.User.UserId,
//                    Name = a.User.Name,
//                    Surname = a.User.Surname,
//                    Phone = a.User.Phone,
//                    Email = a.User.Email,
//                    lan = a.User.lat,
//                    lng = a.User.lng,
//                    Voivodeship = a.User.Voivodeship,
//                    City = a.User.City,
//                },
//                Price = a.Price,
//                Power = a.Power,
//                Engine = a.Engine ?? string.Empty,
//                FuelType = a.FuelType,
//                Mileage = a.Mileage,
//                ProductionYear = a.ProductionYear,

//                Images = a.Images.Select(i => new AnnouncementImagesDto
//                {
//                    AnId = i.AnId,
//                    ImageUrl = i.ImageUrl
//                }).ToList()
//            })
//            .ToListAsync();

//        return Ok(results.Count);
//    }
//}
