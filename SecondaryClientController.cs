using DataAccessLayer.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Models.DTOs.PrimaryClientDTOs;
using Models.Models.DTOs.SecondaryClientDetailsDTOs;
using Models.Models.DTOs.SecondaryClientDTOs;
using Models.Models.DTOs.UserDTOs;
using Sugamta.API.DTOs.UserDTOs;
using Sugamta.API.Repository.Interface;

namespace Sugamta.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class SecondaryClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserDbContext _userDbContext;

        public SecondaryClientController(IUnitOfWork unitOfWork, UserDbContext userDbContext)
        {
            _unitOfWork = unitOfWork;
            _userDbContext = userDbContext;
        }

        [HttpGet("secondory-client-image/{email}")]
        public IActionResult DisplayImage(string email)
        {
            try
            {
                var secondaryClientDetail = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(email);

                if (secondaryClientDetail == null || secondaryClientDetail.ImageUrl == null || secondaryClientDetail.ImageUrl.Length == 0)
                {
                    return NotFound();
                }

                var base64String = Convert.ToBase64String(secondaryClientDetail.ImageUrl);

                // Return the base64-encoded string as part of the response
                return Ok(base64String);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Failed to retrieve and display image: {ex.Message}");
            }
        }


        [HttpGet("get-secondary-client/{email}")]
        public ActionResult GetSecondaryClientDetails(string email)
        {
            try
            {
                //var userDetails = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(email);
                var secondaryClientDetails = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(email);

                if (secondaryClientDetails == null)
                {
                    SecondaryClientEditDto secondaryClientDetailsEditDtos = new SecondaryClientEditDto();
                    return NotFound(secondaryClientDetailsEditDtos);
                }


                var secondaryClientDetailsDto = secondaryClientDetails.Adapt<SecondaryClientEditDto>();


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

                if (secondaryClientDetailsDto != null)
                {
                    if (secondaryClientDetailsDto.CountryId != 0)
                    {
                        for (var i = 0; i < existingCountry.Count; i++)
                        {
                            if (secondaryClientDetailsDto != null)
                            {
                                if (secondaryClientDetailsDto.CountryId == existingCountry[i].CountryId)
                                {
                                    secondaryClientDetailsDto.Country = existingCountry[i].CountryName;
                                }
                            }
                        }
                    }
                }


                if (secondaryClientDetailsDto != null)
                {
                    if (secondaryClientDetailsDto.StateId != 0)
                    {
                        for (var i = 0; i < existingState.Count; i++)
                        {
                            if (secondaryClientDetailsDto != null)
                            {
                                if (secondaryClientDetailsDto.StateId == existingState[i].StateId)
                                {
                                    secondaryClientDetailsDto.State = existingState[i].StateName;
                                }
                            }
                        }
                    }
                }

                return Ok(secondaryClientDetailsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve SecondaryClientDetails: {ex.Message}");
            }
        }

        

        [HttpGet("get-secondary-client-all")]
        public IActionResult GetSecondaryClient()
        {
            try
            {
                var result = _unitOfWork.SecondaryClient.GetSecondaryClient();
                var data = result.Adapt<List<SecondaryClientDto>>();
                var newData = new List<SecondaryClientDto>();

                foreach (var secondaryClient in data)
                {
                    var role = _unitOfWork.Role.GetRoleById(i => i.RoleId == secondaryClient.RoleId);
                    if (role != null)
                    {
                        secondaryClient.RoleType = role.RoleType;
                    }

                    var state = _unitOfWork.State.GetStateById((int)secondaryClient.StateId);
                    if (state != null)
                    {
                        secondaryClient.State = state.StateName;
                    }

                    var country = _unitOfWork.Country.GetCountryById((int)secondaryClient.CountryId);
                    if (country != null)
                    {
                        secondaryClient.Country = country.CountryName;
                    }

                    newData.Add(secondaryClient);
                }

                return Ok(newData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve SecondaryClientDetails: {ex.Message}");
            }
        }




        [HttpPost("add-secondary-client")]
        public ActionResult AddSecondaryClient([FromForm] SecondaryClientCreateOrUpdateDto secondaryClientCreateDto)
        {
            try
            {
                var secondaryClientDto = secondaryClientCreateDto.Adapt<SecondaryClient>();
            
                var existingSecondaryClient = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(secondaryClientCreateDto.SecondaryClientEmail);

                if (existingSecondaryClient != null)
                {
                    return BadRequest("This Secondary Client already added. Please go for updating Secondary Client.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(secondaryClientCreateDto.Password);
                secondaryClientCreateDto.Password = hashedPassword;
                secondaryClientCreateDto.CreationDate = DateTime.Now;
                var isPrimaryClientEmailMatch = _unitOfWork.PrimaryClient.GetPrimaryClientByEmail(secondaryClientCreateDto.PrimaryClientEmail);
                if (isPrimaryClientEmailMatch == null)
                {
                    return NotFound($"primayClient With Email {secondaryClientCreateDto.PrimaryClientEmail} does not exist");
                }
                if (secondaryClientCreateDto.formFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        secondaryClientCreateDto.formFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        secondaryClientCreateDto.ImageUrl = imageBytes;
                    }
                }
                if (secondaryClientCreateDto.SecondaryClientPanformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        secondaryClientCreateDto.SecondaryClientPanformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        secondaryClientCreateDto.SecondaryClientPanImageUrl = imageBytes;
                    }
                }
                if (secondaryClientCreateDto.SecondaryClientAadharformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        secondaryClientCreateDto.SecondaryClientAadharformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        secondaryClientCreateDto.SecondaryClientAadharImageUrl = imageBytes;
                    }
                }
                _unitOfWork.SecondaryClient.InsertSecondaryClient(secondaryClientCreateDto);
                var createDto = secondaryClientDto.Adapt<UserCreateDto>();
                createDto.Password = hashedPassword;
                _unitOfWork.user.CreateUser(createDto);

                return Ok("SecondaryClient Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Add SecondaryClientDetails: {ex.Message}");
            }
        }

        [HttpPut("update-secondary-client")]
        public ActionResult UpdateSecondaryClient([FromForm] SecondaryClientCreateOrUpdateDto secondaryUpdateDto)
        {
            try
            {
                if (secondaryUpdateDto.Password == null)
                {
                    var password = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(secondaryUpdateDto.SecondaryClientEmail);
                    if (password.Password != null)
                    {
                        secondaryUpdateDto.Password = password.Password;

                    }

                    if (secondaryUpdateDto.RoleId == null || secondaryUpdateDto.RoleId == 0)
                    {
                        var role = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(secondaryUpdateDto.SecondaryClientEmail);
                        if (role.RoleId != null)
                        {
                            secondaryUpdateDto.RoleId = role.RoleId;
                        }
                    }

                    if (secondaryUpdateDto.PrimaryClientEmail == null)
                    {
                        var primaryClientEmail = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(secondaryUpdateDto.SecondaryClientEmail);
                        if (primaryClientEmail.PrimaryClientEmail != null)
                        {
                            secondaryUpdateDto.PrimaryClientEmail = primaryClientEmail.PrimaryClientEmail;
                        }
                    }
                }

                var existingSecondaryClient = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(secondaryUpdateDto.SecondaryClientEmail);

                if (existingSecondaryClient != null)
                {
                    
                    if (existingSecondaryClient.ImageUrl != null)
                    {
                        secondaryUpdateDto.ImageUrl = existingSecondaryClient.ImageUrl;
                    }
                }


                if (secondaryUpdateDto.formFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        secondaryUpdateDto.formFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        secondaryUpdateDto.ImageUrl = imageBytes;
                    }
                }

                var secondaryClientDto = secondaryUpdateDto.Adapt<SecondaryClient>();

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(secondaryUpdateDto.Password);
                secondaryUpdateDto.Password = hashedPassword;

               
                if (secondaryUpdateDto.SecondaryClientPanformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        secondaryUpdateDto.SecondaryClientPanformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        secondaryUpdateDto.SecondaryClientPanImageUrl = imageBytes;
                    }
                }
                if (secondaryUpdateDto.SecondaryClientAadharformFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        secondaryUpdateDto.SecondaryClientAadharformFile.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        secondaryUpdateDto.SecondaryClientAadharImageUrl = imageBytes;
                    }
                }
                
                _unitOfWork.SecondaryClient.UpdateSecondaryClient(secondaryUpdateDto);
                return Ok("SecondaryClient Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Update SecondaryClientDetails: {ex.Message}");
            }
        }

        [HttpDelete("delete-secondary-client/{email}")]
        public ActionResult DeleteSecondaryClient(string email)
        {
            try
            {
                var secondaryClientDto = _unitOfWork.SecondaryClient.GetSecondaryClientByEmail(email);
                if (secondaryClientDto == null)
                {
                    return NotFound();
                }
                
                _unitOfWork.SecondaryClient.DeleteSecondaryClient(secondaryClientDto);
                _unitOfWork.user.DeleteUser(email);
                return Ok("SecondaryClient Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to Delete SecondaryClientDetails: {ex.Message}");
            }
        }
    }
}
