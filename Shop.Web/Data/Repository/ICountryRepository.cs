﻿using Shop.Web.Data.Entities;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
		IQueryable GetCountriesWithCities();

		Task<Country> GetCountryWithCitiesAsync(int id);

		Task<City> GetCityAsync(int id);

		Task AddCityAsync(CityViewModel model);

		Task<int> UpdateCityAsync(City city);

		Task<int> DeleteCityAsync(City city);

	}
}