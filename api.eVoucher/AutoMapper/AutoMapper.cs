using api.eVoucher.Dtos;
using api.eVoucher.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace api.eStore.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();

            CreateMap<PaymentMethod, PaymentMethodViewModel>();
            CreateMap<PaymentMethodViewModel, PaymentMethod>();
        }
    }
}
