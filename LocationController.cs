using Microsoft.AspNetCore.Mvc;
using Models.Models.DTOs.AgencyDTOs;
using Models.Models;
using Sugamta.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Sugamta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[HttpPost("add-country-location")]
        //public ActionResult AddCountryLocation([FromForm] CountryLocations countryLocationsDto)
        //{
        //    try
        //    {
        //        if (countryLocationsDto.SocietyName == null || countryLocationsDto.CityName == null ||
        //            countryLocationsDto.LocalityName == null || countryLocationsDto.PinCode == null)
        //        {
        //            return BadRequest();
        //        }

        //        if(_unitOfWork.cities.GetCityIdByCityName(c => c.CityName == countryLocationsDto.CityName) != null)
        //        {
        //            if (!((_unitOfWork.cities.GetCityIdByCityName(c => c.CityName == countryLocationsDto.CityName)).CityName == countryLocationsDto.CityName))
        //            {
        //                City city = new()
        //                {
        //                    CityName = countryLocationsDto.CityName
        //                };

        //                _unitOfWork.cities.CreateCity(city);
        //                _unitOfWork.Save();
        //            }
        //        } else
        //        {
        //            City city = new()
        //            {
        //                CityName = countryLocationsDto.CityName
        //            };

        //            _unitOfWork.cities.CreateCity(city);
        //            _unitOfWork.Save();
        //        }

        //        if (_unitOfWork.Localities.GetLocalityIdByLocalityName(c => c.LocalityName == countryLocationsDto.LocalityName) != null)
        //        {
        //            if (!((_unitOfWork.Localities.GetLocalityIdByLocalityName(c => c.LocalityName == countryLocationsDto.LocalityName)).LocalityName == countryLocationsDto.LocalityName))
        //            {
        //                Locality locality = new()
        //                {
        //                    LocalityName = countryLocationsDto.LocalityName,
        //                    PinCode = countryLocationsDto.PinCode
        //                };

        //                _unitOfWork.Localities.CreateLocality(locality);
        //                _unitOfWork.Save();
        //            }
        //        } else
        //        {
        //            Locality locality = new()
        //            {
        //                LocalityName = countryLocationsDto.LocalityName,
        //                PinCode = countryLocationsDto.PinCode
        //            };

        //            _unitOfWork.Localities.CreateLocality(locality);
        //            _unitOfWork.Save();
        //        }

        //        if (_unitOfWork.Socities.GetSocietyIdBySocietyName(c => c.SocietyName == countryLocationsDto.SocietyName) != null)
        //        {
        //            if (!((_unitOfWork.Socities.GetSocietyIdBySocietyName(c => c.SocietyName == countryLocationsDto.SocietyName)).SocietyName == countryLocationsDto.SocietyName))
        //            {
        //                Society society = new()
        //                {
        //                    SocietyName = countryLocationsDto.SocietyName
        //                };

        //                _unitOfWork.Socities.CreateSociety(society);
        //                _unitOfWork.Save();
        //            }
        //        } else
        //        {
        //            Society society = new()
        //            {
        //                SocietyName = countryLocationsDto.SocietyName
        //            };

        //            _unitOfWork.Socities.CreateSociety(society);
        //            _unitOfWork.Save();
        //        }

        //        var countryInfo = _unitOfWork.Country.GetCountryById(countryLocationsDto.CountryId);
        //        var stateInfo = _unitOfWork.State.GetStateById(countryLocationsDto.StateId);
        //        var cityInfo = _unitOfWork.cities.GetCityIdByCityName(c => c.CityName == countryLocationsDto.CityName);
        //        var localityInfo = _unitOfWork.Localities.GetLocalityIdByLocalityName(c => c.LocalityName == countryLocationsDto.LocalityName);
        //        var societyInfo = _unitOfWork.Socities.GetSocietyIdBySocietyName(c => c.SocietyName == countryLocationsDto.SocietyName);

        //        CountryLocations countryLocations = new()
        //        {
        //            CountryId = countryLocationsDto.CountryId,
        //            StateId = countryLocationsDto.StateId,
        //            CityId = cityInfo.CityId,
        //            LocalityId = localityInfo.LocalityId,
        //            SocietyId = societyInfo.SocietyId,
        //            CountryName = countryInfo.CountryName,
        //            StateName = stateInfo.StateName,
        //            CityName = countryLocationsDto.CityName,
        //            LocalityName = countryLocationsDto.LocalityName,
        //            PinCode = countryLocationsDto.PinCode,
        //            SocietyName = countryLocationsDto.SocietyName,
        //        };

        //        _unitOfWork.CountryLocations.CreateCouhtryLocation(countryLocations);
        //        _unitOfWork.Save();

        //        return Ok("Location Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Failed to Update Agency: {ex.Message}");
        //    }
        //}

        [HttpPost("add-country-location")]
        public ActionResult AddCountryLocation([FromForm] CountryLocations countryLocationsDto)
        {
            try
            {
                if (countryLocationsDto.SocietyName == null || countryLocationsDto.CityName == null ||
                    countryLocationsDto.LocalityName == null || countryLocationsDto.PinCode == null)
                {
                    return BadRequest();
                }

                if (_unitOfWork.cities.GetCityIdByCityName(c => c.CityName == countryLocationsDto.CityName) != null)
                {
                    if (!((_unitOfWork.cities.GetCityIdByCityName(c => c.CityName == countryLocationsDto.CityName)).CityName == countryLocationsDto.CityName))
                    {
                        City city = new()
                        {
                            CityName = countryLocationsDto.CityName,
                            StateId = countryLocationsDto.StateId
                        };

                        _unitOfWork.cities.CreateCity(city);
                        _unitOfWork.Save();
                    }
                } 
                else
                {
                    City city = new()
                    {
                        CityName = countryLocationsDto.CityName,
                        StateId = countryLocationsDto.StateId
                    };

                    _unitOfWork.cities.CreateCity(city);
                    _unitOfWork.Save();

                }

                var cityInfo = _unitOfWork.cities.GetCityIdByCityName(c => c.CityName == countryLocationsDto.CityName);

                if (_unitOfWork.Localities.GetLocalityIdByLocalityName(c => c.LocalityName == countryLocationsDto.LocalityName) != null)
                {
                    if (!((_unitOfWork.Localities.GetLocalityIdByLocalityName(c => c.LocalityName == countryLocationsDto.LocalityName)).LocalityName == countryLocationsDto.LocalityName))
                    {
                        Locality locality = new()
                        {
                            LocalityName = countryLocationsDto.LocalityName,
                            PinCode = countryLocationsDto.PinCode,
                            CityId = cityInfo.CityId
                        };

                        _unitOfWork.Localities.CreateLocality(locality);
                        _unitOfWork.Save();
                    }
                }
                else
                {
                    Locality locality = new()
                    {
                        LocalityName = countryLocationsDto.LocalityName,
                        PinCode = countryLocationsDto.PinCode,
                        CityId = cityInfo.CityId
                    };

                    _unitOfWork.Localities.CreateLocality(locality);
                    _unitOfWork.Save();
                }

                var localityInfo = _unitOfWork.Localities.GetLocalityIdByLocalityName(c => c.LocalityName == countryLocationsDto.LocalityName);

                if (_unitOfWork.Socities.GetSocietyIdBySocietyName(c => c.SocietyName == countryLocationsDto.SocietyName) != null)
                {
                    if (!((_unitOfWork.Socities.GetSocietyIdBySocietyName(c => c.SocietyName == countryLocationsDto.SocietyName)).SocietyName == countryLocationsDto.SocietyName))
                    {
                        Society society = new()
                        {
                            SocietyName = countryLocationsDto.SocietyName,
                            LocalityId = localityInfo.LocalityId
                        };

                        _unitOfWork.Socities.CreateSociety(society);
                        _unitOfWork.Save();
                    }
                }
                else
                {
                    Society society = new()
                    {
                        SocietyName = countryLocationsDto.SocietyName,
                        LocalityId = localityInfo.LocalityId
                    };

                    _unitOfWork.Socities.CreateSociety(society);
                    _unitOfWork.Save();
                }

                var countryInfo = _unitOfWork.Country.GetCountryById(countryLocationsDto.CountryId);
                var stateInfo = _unitOfWork.State.GetStateById(countryLocationsDto.StateId);
                var societyInfo = _unitOfWork.Socities.GetSocietyIdBySocietyName(c => c.SocietyName == countryLocationsDto.SocietyName);

                CountryLocations countryLocations = new()
                {
                    CountryId = countryLocationsDto.CountryId,
                    StateId = countryLocationsDto.StateId,
                    CityId = cityInfo.CityId,
                    LocalityId = localityInfo.LocalityId,
                    SocietyId = societyInfo.SocietyId,
                    CountryName = countryInfo.CountryName,
                    StateName = stateInfo.StateName,
                    CityName = countryLocationsDto.CityName,
                    LocalityName = countryLocationsDto.LocalityName,
                    PinCode = countryLocationsDto.PinCode,
                    SocietyName = countryLocationsDto.SocietyName,
                };

                _unitOfWork.CountryLocations.CreateCouhtryLocation(countryLocations);
                _unitOfWork.Save();

                return Ok("Location Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Update Agency: {ex.Message}");
            }
        }

        [HttpGet("search-city/{query}")]
        public IActionResult SearchCity(string query)
        {
            var allCities = _unitOfWork.cities.GetAllCities();


            var matchingCities = allCities.Where(c => c.CityName.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                                .Select(c => c.CityName)
                                .ToList();

            if (matchingCities.Count > 0)
            {
                return Ok(matchingCities);
            }

            return NotFound();
        }

        [HttpGet("search-locality/{query}")]
        public IActionResult SearchLocality(string query)
        {
            var allLocalities = _unitOfWork.Localities.GetAllLocalities();
            var matchingLocalities = allLocalities.Where(c => c.LocalityName.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                                .Select(c => c.LocalityName)
                                .ToList();

            if (matchingLocalities.Count > 0)
            {
                return Ok(matchingLocalities);
            }

            return NotFound();
        }

        [HttpGet("search-society/{query}")]
        public IActionResult SearchSociety(string query)
        {
            var allSocieties = _unitOfWork.Socities.GetAllSocieties();

            var matchingSocieties = allSocieties.Where(c => c.SocietyName.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                                .Select(c => c.SocietyName)
                                .ToList();

            if (matchingSocieties.Count > 0)
            {
                return Ok(matchingSocieties);
            }

            return NotFound();
        }
    }
}
