using DataAccessLayer.Data;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Models.DTOs.PrimaryClientDTOs;
using Models.Models.DTOs.SecondaryClientDetailsDTOs;
using Models.Models.DTOs.SecondaryClientDTOs;
using Models.Models.DTOs.UserDTOs;
using Sugamta.API.DTOs.UserDTOs;
using Sugamta.API.Repository;
using Sugamta.API.Repository.Interface;

namespace Sugamta.API.Controllers
{
    [Route("api/PrimaryClient")]
    [ApiController]
    public class PrimaryClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserDbContext _dbContext;

        public PrimaryClientController(IUnitOfWork unitOfWork, UserDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        [HttpGet("get-primary-client-image/{email}")]
        public IActionResult GetPrimaryClientImage(string email)
        {
            try
            {
                var primaryClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(email);

                if (primaryClient == null || primaryClient.ImageUrl == null || primaryClient.ImageUrl.Length == 0)
                {
                    return NotFound();
                }

                var base64String = Convert.ToBase64String(primaryClient.ImageUrl);

                // Return the base64-encoded string as part of the response
                return Ok(base64String);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Failed to retrieve and display image: {ex.Message}");
            }
        }

        [HttpGet("get-primary-client-aadhar-image/{email}")]
        public IActionResult GetPrimaryClientAadharImage(string email)
        {
            try
            {
                var primaryClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(email);

                if (primaryClient == null || primaryClient.PrimaryClientAadharImageUrl == null || primaryClient.PrimaryClientAadharImageUrl.Length == 0)
                {
                    return NotFound();
                }

                var base64String = Convert.ToBase64String(primaryClient.PrimaryClientAadharImageUrl);

                // Return the base64-encoded string as part of the response
                return Ok(base64String);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Failed to retrieve and Aadhar image: {ex.Message}");
            }
        }

        [HttpGet("get-primary-client-pan-image/{email}")]
        public IActionResult GetPrimaryClientPanImage(string email)
        {
            try
            {
                var primaryClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(email);

                if (primaryClient == null || primaryClient.PrimaryClientPanImageUrl == null || primaryClient.PrimaryClientPanImageUrl.Length == 0)
                {
                    return NotFound();
                }

                var base64String = Convert.ToBase64String(primaryClient.PrimaryClientPanImageUrl);

                // Return the base64-encoded string as part of the response
                return Ok(base64String);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Failed to retrieve and Pan image: {ex.Message}");
            }
        }

        [HttpGet("get-primary-client/{email}")]
        public ActionResult GetPrimaryClientDetails(string email)
        {
            try
            {
                var primaryClientDetails = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(email);

                if (primaryClientDetails == null)
                {
                    PrimaryClientEditDto primaryClientDetailsEditDtos = new PrimaryClientEditDto();
                    return NotFound(primaryClientDetailsEditDtos);
                }

                var primaryClientDetailsDto = primaryClientDetails.Adapt<PrimaryClientEditDto>();


                var existingCountry = _unitOfWork.Country.GetCountries();
                if (existingCountry == null)
                {
                    return BadRequest("Country Not Found.");
                }
                var existingState = _unitOfWork.State.GetStates();
                if (existingState == null)
                {
                    return BadRequest("State Not Found");
                }

                if (primaryClientDetailsDto != null)
                {
                    if (primaryClientDetailsDto.CountryId != 0)
                    {
                        for (var i = 0; i < existingCountry.Count; i++)
                        {
                            if (primaryClientDetailsDto != null)
                            {
                                if (primaryClientDetailsDto.CountryId == existingCountry[i].CountryId)
                                {
                                    primaryClientDetailsDto.Country = existingCountry[i].CountryName;
                                }
                            }
                        }
                    }
                }


                if (primaryClientDetailsDto != null)
                {
                    if (primaryClientDetailsDto.StateId != 0)
                    {
                        for (var i = 0; i < existingState.Count; i++)
                        {
                            if (primaryClientDetailsDto != null)
                            {
                                if (primaryClientDetailsDto.StateId == existingState[i].StateId)
                                {
                                    primaryClientDetailsDto.State = existingState[i].StateName;
                                }
                            }
                        }
                    }
                }

                return Ok(primaryClientDetailsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve PrimaryClientDetails: {ex.Message}");
            }
        }

        [HttpGet("get-primary-client-all")]
        public IActionResult GetPrimaryClient()
        {
            try
            {
                var result = _unitOfWork.PrimaryClient.GetPrimaryClient();
                var data = result.Adapt<List<PrimaryClientDto>>();
                var newData = new List<PrimaryClientDto>();

                foreach (var primaryClient in data)
                {
                   
                    var agency = _unitOfWork.Agency.GetAgencyByEmail(primaryClient.AgencyEmail);
                    if (agency != null)
                    {
                        primaryClient.AgencyName = agency.Name;
                    }

                    var role = _unitOfWork.Role.GetRoleById(i => i.RoleId == primaryClient.RoleId);
                    if (role != null)
                    {
                        primaryClient.RoleType = role.RoleType;
                    }

                    var state = _unitOfWork.State.GetStateById((int)primaryClient.StateId);
                    if (state != null)
                    {
                        primaryClient.State = state.StateName;
                    }

                    var country = _unitOfWork.Country.GetCountryById((int)primaryClient.CountryId);
                    if(country != null)
                    {
                        primaryClient.Country = country.CountryName;
                    }

                    newData.Add(primaryClient);
                }

                return Ok(newData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve PrimaryClientDetails: {ex.Message}");
            }
        }


        [HttpPost("add-primary-client")]
        public ActionResult AddPrimaryClient([FromForm] PrimaryClientCreateOrUpdateDto primaryClientCreateDto)
        {
            try
            {
                var primaryClientDto = primaryClientCreateDto.Adapt<PrimaryClient>();
                var existingPrimaryClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryClientCreateDto.PrimaryClientEmail);

                if (existingPrimaryClient != null)
                {
                    return BadRequest("This Primary Client already added. Please go for updating Primary Client.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(primaryClientCreateDto.Password);
                primaryClientCreateDto.Password = hashedPassword;
                primaryClientCreateDto.CreationDate = DateTime.Now;
                var isAgencyEmailMatch = _unitOfWork.Agency.GetAgencyByEmail(primaryClientCreateDto.AgencyEmail);
                if (isAgencyEmailMatch == null)
                {
                    return NotFound($"Agency With Email {primaryClientCreateDto.AgencyEmail} does not exist");
                }
                if (primaryClientCreateDto.formFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        primaryClientCreateDto.formFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        primaryClientCreateDto.ImageUrl = imageBytes;
                    }
                }
                if (primaryClientCreateDto.PrimaryClientPanformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        primaryClientCreateDto.PrimaryClientPanformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        primaryClientCreateDto.PrimaryClientPanImageUrl = imageBytes;
                    }
                }
                if (primaryClientCreateDto.PrimaryClientAadharformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        primaryClientCreateDto.PrimaryClientAadharformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        primaryClientCreateDto.PrimaryClientAadharImageUrl = imageBytes;
                    }
                }
                _unitOfWork.PrimaryClient.InsertPrimaryClient(primaryClientCreateDto);
                var createDto = primaryClientDto.Adapt<UserCreateDto>();
                createDto.Password = hashedPassword;
                _unitOfWork.user.CreateUser(createDto);

                return Ok("PrimaryClient Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Add PrimaryClientDetails: {ex.Message}");
            }
        }

        //        [HttpPut("update-primary-client")]
        //        public ActionResult UpdatePrimaryClient([FromForm] PrimaryClientCreateOrUpdateDto primaryUpdateDto)
        //        {
        //            try
        //            {
        //                if (primaryUpdateDto.Password == null)
        //                {
        //                    var password = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);
        //                    if (password.Password != null)
        //                    {
        //                        primaryUpdateDto.Password = password.Password;

        //                    }

        //                    if (primaryUpdateDto.RoleId == null || primaryUpdateDto.RoleId == 0)
        //                    {
        //                        var role = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);
        //                        if (role.RoleId != null)
        //                        {
        //                            primaryUpdateDto.RoleId = role.RoleId;
        //                        }
        //                    }

        //                    if (primaryUpdateDto.AgencyEmail == null)
        //                    {
        //                        var agencyEmail = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);
        //                        if (agencyEmail.AgencyEmail != null)
        //                        {
        //                            primaryUpdateDto.AgencyEmail = agencyEmail.AgencyEmail;
        //                        }
        //                    }

        //                }
        //                PrimaryClientDto primaryClientDto = new()
        //                {
        //                    PrimaryClientEmail = primaryUpdateDto.PrimaryClientEmail,
        //                    FirstName = primaryUpdateDto.FirstName,
        //                    LastName = primaryUpdateDto.LastName,
        //                    Password = primaryUpdateDto.Password,
        //                    CreationDate = primaryUpdateDto.CreationDate,
        //                    RoleId = primaryUpdateDto.RoleId,
        //                    AgencyEmail = primaryUpdateDto.AgencyEmail,
        //                    IsDeleted = primaryUpdateDto.IsDeleted,
        //                    Locality = primaryUpdateDto.Locality,
        //                    Society = primaryUpdateDto.Society,
        //                    City = primaryUpdateDto.City,
        //                    StateId = primaryUpdateDto.StateId,
        //                    CountryId = primaryUpdateDto.CountryId,
        //                    PhoneNumber = primaryUpdateDto.PhoneNumber,
        //                    AlternatePhoneNumber = primaryUpdateDto.AlternatePhoneNumber,
        //                    DOB = primaryUpdateDto.DOB,
        //                    UpdationDate = DateTime.Now,
        //                    ImageUrl = primaryUpdateDto.ImageUrl,
        //                    formFile = primaryUpdateDto.formFile,
        //                    PrimaryClientPanImageUrl = primaryUpdateDto.PrimaryClientPanImageUrl,
        //                    PrimaryClientAadharImageUrl = primaryUpdateDto.PrimaryClientAadharImageUrl,
        //                    PrimaryClientPanformFile = primaryUpdateDto.PrimaryClientPanformFile,
        //                    PrimaryClientAadharformFile = primaryUpdateDto.PrimaryClientAadharformFile,
        //                    PrimaryClientAadharNumber = primaryUpdateDto.PrimaryClientAadharNumber,
        //                    PrimaryClientPanNumber = primaryUpdateDto.PrimaryClientPanNumber,
        //                    EnrolledId = primaryUpdateDto.EnrolledId,
        //                    CIFNumber = primaryUpdateDto.CIFNumber
        //                };

        //                //var primaryClient=primaryClientDto.Adapt<PrimaryClient>();
        //                // var existingClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryClient.ClientEmail);
        //                // _dbContext.Entry(existingClient).State = EntityState.Detached;
        //                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(primaryUpdateDto.Password);
        //                primaryUpdateDto.Password = hashedPassword;
        //                if (primaryUpdateDto.formFile != null)
        //                {
        //                    using (var memoryStream = new MemoryStream())
        //                    {
        //                        primaryUpdateDto.formFile.CopyToAsync(memoryStream);
        //                        var imageBytes = memoryStream.ToArray();

        //                        primaryUpdateDto.ImageUrl = imageBytes;
        //                    }
        //                }
        //                if (primaryUpdateDto.PrimaryClientPanformFile != null)
        //                {
        //                    using (var memoryStream = new MemoryStream())
        //                    {
        //                        primaryUpdateDto.PrimaryClientPanformFile.CopyToAsync(memoryStream);
        //                        var imageBytes = memoryStream.ToArray();

        //                        primaryUpdateDto.PrimaryClientPanImageUrl = imageBytes;
        //                    }
        //                }
        //                if (primaryUpdateDto.PrimaryClientAadharformFile != null)
        //                {
        //                    using (var memoryStream = new MemoryStream())
        //                    {
        //                        primaryUpdateDto.PrimaryClientAadharformFile.CopyToAsync(memoryStream);
        //                        var imageBytes = memoryStream.ToArray();

        //                        primaryUpdateDto.PrimaryClientAadharImageUrl = imageBytes;
        //                    }
        //                }
        //               /* var existingClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryClientDto.PrimaryClientEmail);
        //                _dbContext.Entry(existingClient).State = EntityState.Detached;
        //                primaryClientDto.CreationDate = existingClient.CreationDate;
        //                primaryClientDto.UpdationDate = DateTime.Now;
        //*/
        //                _unitOfWork.PrimaryClient.UpdatePrimaryClient(primaryUpdateDto);
        //                return Ok("PrimaryClient Updated Successfully");
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode(500, $"Failed to Update PrimaryClientDetails: {ex.Message}");
        //            }
        //        }

        [HttpPut("update-primary-client")]
        public ActionResult UpdatePrimaryClient([FromForm] PrimaryClientCreateOrUpdateDto primaryUpdateDto)
        {
            try
            {
                if (primaryUpdateDto.Password == null)
                {
                    var password = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);
                    if (password.Password != null)
                    {
                        primaryUpdateDto.Password = password.Password;

                    }

                    if (primaryUpdateDto.RoleId == null || primaryUpdateDto.RoleId == 0)
                    {
                        var role = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);
                        if (role.RoleId != null)
                        {
                            primaryUpdateDto.RoleId = role.RoleId;
                        }
                    }

                    if (primaryUpdateDto.AgencyEmail == null)
                    {
                        var agencyEmail = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);
                        if (agencyEmail.AgencyEmail != null)
                        {
                            primaryUpdateDto.AgencyEmail = agencyEmail.AgencyEmail;
                        }
                    }

                    var existingPrimaryClient = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(primaryUpdateDto.PrimaryClientEmail);

                    if (existingPrimaryClient != null)
                    {
                        // If an existing image exists, keep it; otherwise, update with the new image
                        if (existingPrimaryClient.ImageUrl != null)
                        {
                            primaryUpdateDto.ImageUrl = existingPrimaryClient.ImageUrl;
                        }
                    }

                    if (primaryUpdateDto.formFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            primaryUpdateDto.formFile.CopyToAsync(memoryStream);
                            var imageBytes = memoryStream.ToArray();

                            primaryUpdateDto.ImageUrl = imageBytes;
                        }
                    }

                }
                var primaryClientDto = primaryUpdateDto.Adapt<PrimaryClient>();

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(primaryUpdateDto.Password);
                primaryUpdateDto.Password = hashedPassword;

                if (primaryUpdateDto.PrimaryClientPanformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        primaryUpdateDto.PrimaryClientPanformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        primaryUpdateDto.PrimaryClientPanImageUrl = imageBytes;
                    }
                }
                if (primaryUpdateDto.PrimaryClientAadharformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        primaryUpdateDto.PrimaryClientAadharformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        primaryUpdateDto.PrimaryClientAadharImageUrl = imageBytes;
                    }
                }
                _unitOfWork.PrimaryClient.UpdatePrimaryClient(primaryUpdateDto);
                return Ok("PrimaryClient Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Update PrimaryClientDetails: {ex.Message}");
            }
        }

        [HttpDelete("delete-primary-client/{email}")]
        public ActionResult DeletePrimaryClient(string email)
        {
            try
            {
                var primaryClientDto = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(email);
                if(primaryClientDto == null)
                {
                    return NotFound();
                }
                _unitOfWork.PrimaryClient.DeletePrimaryClient(primaryClientDto);
                _unitOfWork.user.DeleteUser(email);
                return Ok("PrimaryClient Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Delete PrimaryClientDetails: {ex.Message}");
            }
        }
    }
}
