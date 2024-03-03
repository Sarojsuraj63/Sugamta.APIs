
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.DTOs.AgencyDTOs;
using Sugamta.API.Repository.Interface;

namespace Sugamta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AgencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        [HttpGet("get-agency-all")]
        public IActionResult GetAgency()
        {
            try
            {
                var result = _unitOfWork.Agency.GetAgencies();
                if (result == null)
                {
                    return BadRequest("Agency Data Not Found");
                }
                return Ok(result);
            }  
            
             catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("get-agency/{email}")]

        public IActionResult GetAgencyByEmail(string email)
        {
            try
            {
                var result = _unitOfWork.Agency.GetAgencyByEmail(email);
                if (result == null)
                {
                    return NotFound("Ageny not Found using This Email");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("update-agency-details")]
        public ActionResult UpdateAgencyDetails([FromForm] AgencyProfileDto agencyDto)
        {
            try
            {
                var user = _unitOfWork.user.GetUser(agencyDto.Email);

                if (agencyDto.Password == null)
                {
                    if (agencyDto.Email != null) // Check if email is not null before fetching password
                    {
                        if (user != null && user.Password != null) // Check if password is not null
                        {
                            agencyDto.Password = user.Password;
                        }
                    }
                }

                if (agencyDto.formFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        agencyDto.formFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        agencyDto.ImageUrl = imageBytes;
                    }
                }

                var agency = _unitOfWork.Agency.GetAgencyByEmail(agencyDto.Email);

                if (agency != null)
                {
                    agency.Email = agencyDto.Email;
                    agency.Name = agencyDto.Name;
                    agency.Password = user.Password;
                    agency.CreationDate = agency.CreationDate;
                    agency.IsDeleted = agencyDto.IsDeleted;
                    agency.Locality = agencyDto.Locality;
                    agency.Society = agencyDto.Society;
                    agency.City = agencyDto.City;
                    agency.StateId = agencyDto.StateId;
                    agency.CountryId = agencyDto.CountryId;
                    agency.PhoneNumber = agencyDto.PhoneNumber;
                    agency.AlternatePhoneNumber = agencyDto.AlternatePhoneNumber;
                    agency.UpdationDate = DateTime.Now;
                    if(agencyDto.formFile == null)
                    {
                        agency.ImageUrl = agency.ImageUrl;
                    }
                    else
                    {
                        agency.ImageUrl = agencyDto.ImageUrl;
                    }
                };

                

                user.Email = agencyDto.Email;
                user.Name = agencyDto.Name;
                user.Password = agencyDto.Password;
                user.IsDeleted = agencyDto.IsDeleted;
                user.CreationDate = agencyDto.CreationDate;
                

                _unitOfWork.user.UpdateUserWhenAgencyProfileUpdate(user);
                _unitOfWork.Agency.UpdateAgency(agency);
                return Ok("Agency Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Update Agency: {ex.Message}");
            }
        }
    }
}
