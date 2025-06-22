using AutoMapper;
using RecordStore.Core.Dtos;
using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecordStore.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Record mappings
            CreateMap<Record, RecordDto>()
                .ForMember(dest => dest.Artists, opt => opt.MapFrom(src => src.ArtistRecords))
                .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventories.FirstOrDefault()));

            CreateMap<CreateRecordDto, Record>();

            CreateMap<UpdateRecordDto, Record>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Artist mappings
            CreateMap<Artist, ArtistDto>();
            CreateMap<CreateArtistDto, Artist>();
            CreateMap<UpdateArtistDto, Artist>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ArtistRecord mappings
            CreateMap<ArtistRecord, ArtistRecordDto>()
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist.Name));

            // Inventory mappings
            CreateMap<Inventory, InventoryDto>();
            CreateMap<CreateInventoryDto, Inventory>();
            CreateMap<UpdateInventoryDto, Inventory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                    src.OrderRecords.Sum(or => or.Quantity * or.Record.Inventories.FirstOrDefault().Price)));

            CreateMap<OrderRecord, OrderRecordDto>()
                .ForMember(dest => dest.RecordName, opt => opt.MapFrom(src => src.Record.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Record.Inventories.FirstOrDefault().Price))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantity * src.Record.Inventories.FirstOrDefault().Price));

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.RecordName, opt => opt.MapFrom(src => src.Record.Name));

            CreateMap<CreateReviewDto, Review>();

            CreateMap<UpdateReviewDto, Review>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ArtistRecord Management mappings
            CreateMap<ArtistRecord, ArtistRecordDto>()
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist.Name))
                .ForMember(dest => dest.RecordName, opt => opt.MapFrom(src => src.Record.Name));

            CreateMap<CreateArtistRecordDto, ArtistRecord>();

            CreateMap<UpdateArtistRecordDto, ArtistRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // OrderRecord Management mappings
            CreateMap<OrderRecord, OrderRecordDto>()
                .ForMember(dest => dest.RecordName, opt => opt.MapFrom(src => src.Record.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Record.Inventories.FirstOrDefault().Price))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantity * src.Record.Inventories.FirstOrDefault().Price));

            CreateMap<CreateOrderRecordDto, OrderRecord>();

            CreateMap<UpdateOrderRecordDto, OrderRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
