using DataAccessLayer.Data;
//using DataAccessLayer.Migrations;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Models.Models;
using Models.Models.DTOs.UserDetailsDTOs;
using Sugamta.API.DTOs.UserDetailsDTOs;
using Sugamta.API.Repository;
using Sugamta.API.Repository.Interface;
using System;

namespace Sugamta.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserDbContext _userDbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserDetailsController(IUnitOfWork unitOfWork, UserDbContext userDbContext, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userDbContext = userDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

       

        [HttpGet("get-country-list")]
        public IActionResult GetCountry()
        {
            var data=_unitOfWork.Country.GetCountries();
            return Ok(data);
        }
        [HttpGet("get-state-list")]
        public IActionResult GetState()
        {
            var state=_unitOfWork.State.GetStates();
            return Ok(state);
        }


       

    }
}
