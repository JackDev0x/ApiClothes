using ApiClothes.DtoModels;
using ApiClothes.Entities;
using AutoMapper;
using ApiClothes.Entities;
using ApiClothes.DtoModels;
using Microsoft.VisualBasic.FileIO;

namespace ApiClothes
{
    public class AutomovieMappingProfile : Profile
    {
        public AutomovieMappingProfile()
        {
            CreateMap<Announcement, AnnouncementDto>()
                       .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                       .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                       .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname));
            CreateMap<AnnouncementImages, AnnouncementImagesDto>();
            CreateMap<FavoriteAnnouncements, FavoriteAnnouncementsDto>()
                .ForMember(dest => dest.AnnouncementId, opt => opt.MapFrom(src => src.AnnouncementAnId));
            CreateMap<User, UserDto>();
        }
    }
}