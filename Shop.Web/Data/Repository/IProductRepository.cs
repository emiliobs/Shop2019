﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable GetAllWithUser();

        IEnumerable<SelectListItem> GetComboProducts();
    }
}