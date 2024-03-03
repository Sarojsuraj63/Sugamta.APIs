//using DataAccessLayer.Data;
//using Mapster;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting.Internal;
//using Models.Models;
//using Models.Models.DTOs.PrimaryClientDetailsDTOs;
//using Models.Models.DTOs.SecondaryClientDetailsDTOs;
//using Models.Models.DTOs.SecondaryClientDTOs;
//using Models.Models.DTOs.UserDetailsDTOs;
//using Sugamta.API.DTOs.UserDetailsDTOs;
//using Sugamta.API.DTOs.UserDTOs;
//using Sugamta.API.Repository.Interface;

//namespace Sugamta.API.Controllers
//{
//    [ApiController]
//    [Route("api/secondary-client")]
//    public class SecondaryClientDetailController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly UserDbContext _userDbContext;
//        private readonly IWebHostEnvironment _hostingEnvironment;
//        public SecondaryClientDetailController(IUnitOfWork unitOfWork, UserDbContext userDbContext, IWebHostEnvironment hostingEnvironment)
//        {
//            _unitOfWork = unitOfWork;
//            _userDbContext = userDbContext;
//            _hostingEnvironment = hostingEnvironment;
//        }


//        [HttpGet("secondory-client-image/{email}")]
//        public IActionResult DisplayImage(string email)
//        {
//            try
//            {
//                //var secondaryClientDetail = _unitOfWork.SecondaryClientDetail.GetSecondaryClientDetail(email);

//                //if (secondaryClientDetail == null || secondaryClientDetail.ImageUrl == null || secondaryClientDetail.ImageUrl.Length == 0)
//                //{
//                //    return NotFound();
//                //}

//                //var base64String = Convert.ToBase64String(secondaryClientDetail.ImageUrl);

//                // Return the base64-encoded string as part of the response
//                return Ok("base64String");
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions, log errors, etc.
//                return StatusCode(500, $"Failed to retrieve and display image: {ex.Message}");
//            }
//        }
       

//        //[HttpGet("get-secondary-client-details/{email}")]
//        //public ActionResult GetSecondaryClientDetails(string email)
//        //{
//        //    try
//        //    {
//        //        var userDetails = _unitOfWork.SecondaryClient.GetSecondaryClients(email);
//        //        var secondaryClientDetails = _unitOfWork.SecondaryClientDetail.GetSecondaryClientDetail(email);
                
//        //        if (userDetails == null)
//        //        {
//        //            SecondaryClientDetailsEditDtos secondaryClientDetailsEditDtos= new SecondaryClientDetailsEditDtos();
//        //            return NotFound(secondaryClientDetailsEditDtos);
//        //        }


//        //        //var secondaryClientDetailsDto = secondaryClientDetails.Adapt<SecondaryClientDetailsEditDtos?>();

//        //        SecondaryClientDetailsEditDtos secondaryClientDetailsDto = new()
//        //        {
//        //               SecondaryClientEmail = userDetails?.SecondaryClientEmail,
//        //               FirstName = userDetails?.FirstName,
//        //               LastName = userDetails?.LastName,
//        //              // Address = secondaryClientDetails.Address,
//        //              // Gender = secondaryClientDetails.Gender,
//        //             //  Age = secondaryClientDetails.Age,
//        //              Society=secondaryClientDetails?.Society,
//        //              Locality=secondaryClientDetails?.Locality,
//        //               City = secondaryClientDetails?.City,
//        //               StateId = secondaryClientDetails?.StateId,
//        //               CountryId = secondaryClientDetails?.CountryId,
//        //               PhoneNumber = secondaryClientDetails?.PhoneNumber,
//        //               AlternatePhoneNumber = secondaryClientDetails?.AlternatePhoneNumber,
//        //               DOB= secondaryClientDetails?.DOB,
//        //               CreationDate = secondaryClientDetails?.CreationDate,
//        //               UpdationDate = secondaryClientDetails?.UpdationDate,
//        //               ImageUrl = secondaryClientDetails?.ImageUrl,
//        //               SecondaryClientPanImageUrl= secondaryClientDetails?.SecondaryClientPanImageUrl,
//        //               SecondaryClientAadharImageUrl=secondaryClientDetails?.SecondaryClientAadharImageUrl,
//        //               EnrolledId = secondaryClientDetails?.EnrolledId,
//        //               CIFNumber = secondaryClientDetails?.CIFNumber,
//        //               SecondaryClientAadharNumber = secondaryClientDetails?.SecondaryClientAadharNumber,
//        //               SecondaryClientPanNumber = secondaryClientDetails?.SecondaryClientPanNumber
//        //        };


//        //        var existingCountry = _unitOfWork.Country.GetCountries();
//        //        if (existingCountry == null)
//        //        {
//        //            return BadRequest("Country Not Found.");
//        //        }
//        //        var existingState = _unitOfWork.State.GetStates();
//        //        if (existingState == null)
//        //        {
//        //            return BadRequest("State Not Found");
//        //        }

//        //        if(secondaryClientDetailsDto != null)
//        //        {
//        //            if (secondaryClientDetailsDto.CountryId != 0)
//        //            {
//        //                for (var i = 0; i < existingCountry.Count; i++)
//        //                {
//        //                    if(secondaryClientDetails != null)
//        //                    {
//        //                        if (secondaryClientDetails.CountryId == existingCountry[i].CountryId)
//        //                        {
//        //                            secondaryClientDetailsDto.Country = existingCountry[i].CountryName;
//        //                        }
//        //                    }
//        //                }
//        //            }
//        //        }
                

//        //        if(secondaryClientDetailsDto != null)
//        //        {
//        //            if (secondaryClientDetailsDto.StateId != 0)
//        //            {
//        //                for (var i = 0; i < existingState.Count; i++)
//        //                {
//        //                    if(secondaryClientDetails != null)
//        //                    {
//        //                        if (secondaryClientDetails.StateId == existingState[i].StateId)
//        //                        {
//        //                            secondaryClientDetailsDto.State = existingState[i].StateName;
//        //                        }
//        //                    }
//        //                }
//        //            }
//        //        }

//        //        return Ok(secondaryClientDetailsDto);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return StatusCode(500, $"Failed to retrieve SecondaryClientDetails: {ex.Message}");
//        //    }
//        //}

//        //[HttpGet("get-secondary-client-details-for-create-or-update/{email}")]
//        //public IActionResult GetSecondaryClientDetailsForCreateorUpdate(string email)
//        //{
//        //    try
//        //    {
//        //        var secondaryClient = _unitOfWork.SecondaryClientDetail.GetSecondaryClientDetail(email);
//        //        var secondaryClientDetailsDto = secondaryClient.Adapt<SecondaryClientDetailsDtos>();
//        //        if (secondaryClient == null)
//        //        {
//        //            return NotFound();
//        //        }
//        //        var existingCountry = _unitOfWork.Country.GetCountries();
//        //        if (existingCountry == null)
//        //        {
//        //            return BadRequest("Country Not Found.");
//        //        }
//        //        var existingState = _unitOfWork.State.GetStates();
//        //        if (existingState == null)
//        //        {
//        //            return BadRequest("State Not Found");
//        //        }
//        //        return Ok(secondaryClientDetailsDto);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return StatusCode(500, $"Failed to retrieve Primary Client Details: {ex.Message}");
//        //    }
//        //}

//        [HttpPost("add-secondary-client-details")]
//        public ActionResult AddSecondaryClientDetails([FromForm] SecondaryClientDetailsCreateOrUpdateDto _secondaryClientDetailsDtos)
//        {
//            try
//            {
//                // var secondaryClientDetailsDtos = _secondaryClientDetailsDtos.Adapt<SecondaryClientDetailsDtos>();
//                SecondaryClientDetailsDtos secondaryClientDetailsDtos = new SecondaryClientDetailsDtos()
//                {
//                    SecondaryClientEmail = _secondaryClientDetailsDtos.SecondaryClientEmail,
//                    FirstName = _secondaryClientDetailsDtos.FirstName,
//                    LastName = _secondaryClientDetailsDtos.LastName,
//                    Society = _secondaryClientDetailsDtos.Society,
//                    Locality = _secondaryClientDetailsDtos.Locality,
//                    City = _secondaryClientDetailsDtos.City,
//                    StateId = _secondaryClientDetailsDtos.StateId,
//                    CountryId = _secondaryClientDetailsDtos.CountryId,
//                    PhoneNumber = _secondaryClientDetailsDtos.PhoneNumber,
//                    AlternatePhoneNumber = _secondaryClientDetailsDtos.AlternatePhoneNumber,
//                    DOB = _secondaryClientDetailsDtos.DOB,
//                    CreationDate = DateTime.Now,
//                 //  UpdationDate = _secondaryClientDetailsDtos.UpdationDate,
//                    UpdationDate = _secondaryClientDetailsDtos.UpdationDate,
//                    ImageUrl = _secondaryClientDetailsDtos.ImageUrl,
//                    formFile = _secondaryClientDetailsDtos.formFile,
//                    SecondaryClientPanImageUrl = _secondaryClientDetailsDtos.SecondaryClientPanImageUrl,
//                    SecondaryClientAadharImageUrl = _secondaryClientDetailsDtos.SecondaryClientAadharImageUrl,
//                    SecondaryClientPanformFile = _secondaryClientDetailsDtos.SecondaryClientPanformFile,
//                    SecondaryClientAadharformFile = _secondaryClientDetailsDtos.SecondaryClientAadharformFile,
//                    EnrolledId = _secondaryClientDetailsDtos.EnrolledId,
//                    CIFNumber = _secondaryClientDetailsDtos.CIFNumber,
//                    SecondaryClientAadharNumber = _secondaryClientDetailsDtos.SecondaryClientAadharNumber,
//                    SecondaryClientPanNumber = _secondaryClientDetailsDtos.SecondaryClientPanNumber

//                };
//                 var existingClient = _unitOfWork.SecondaryClientDetail.GetSecondaryClientDetail(secondaryClientDetailsDtos.SecondaryClientEmail);

//                if (existingClient != null)
//                {
//                    return BadRequest("This UserDetails already added. Please go for updating UserDetails.");
//                }

//                if (_secondaryClientDetailsDtos.formFile != null)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        _secondaryClientDetailsDtos.formFile.CopyToAsync(memoryStream);
//                        var imageBytes = memoryStream.ToArray();

//                        secondaryClientDetailsDtos.ImageUrl = imageBytes;
//                    }
//                }

//                if (_secondaryClientDetailsDtos.SecondaryClientPanformFile != null)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        _secondaryClientDetailsDtos.SecondaryClientPanformFile.CopyToAsync(memoryStream);
//                        var imageBytes = memoryStream.ToArray();

//                        secondaryClientDetailsDtos.SecondaryClientPanImageUrl = imageBytes;
//                    }
//                }
//                if (_secondaryClientDetailsDtos.SecondaryClientAadharformFile != null)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        _secondaryClientDetailsDtos.SecondaryClientAadharformFile.CopyToAsync(memoryStream);
//                        var imageBytes = memoryStream.ToArray();

//                        secondaryClientDetailsDtos.SecondaryClientAadharImageUrl = imageBytes;
//                    }
//                }

//                // Checking the existence of countries and states
//                var existingCountry = _unitOfWork.Country.GetCountries();
//                if (existingCountry == null)
//                {
//                    return BadRequest("Country Not Found.");
//                }
//                var existingState = _unitOfWork.State.GetStates();
//                if (existingState == null)
//                {
//                    return BadRequest("State Not Found");
//                }

//                // Setting the creation date
//               secondaryClientDetailsDtos.CreationDate = DateTime.Now;

//                // Inserting secondary client details into the database
//                _unitOfWork.SecondaryClientDetail.InsertSecondaryclientDetails(secondaryClientDetailsDtos);
//                _unitOfWork.Save();

//                return Ok("SecondaryClientDetails added successfully");
//            }
//            catch (Exception ex)
//            {
//                // Handling exceptions and returning an error response
//                return StatusCode(500, $"Failed to add SecondaryClientDetails: {ex.Message}");
//            }
//        }

//        [HttpPut("update-secondary-client-details")]
//        public ActionResult UpdateSecondaryClientDetails([FromForm] SecondaryClientDetailsCreateOrUpdateDto _secondaryClientDetailsDtos)
//        {
//            try
//            {
//                //var secondaryClientDetailsDtos = secondaryClientDetailsDto.Adapt<SecondaryClientDetailsDtos>();
//                SecondaryClientDetailsDtos secondaryClientDetailsDtos = new SecondaryClientDetailsDtos()
//                {
//                    SecondaryClientEmail = _secondaryClientDetailsDtos.SecondaryClientEmail,
//                    FirstName = _secondaryClientDetailsDtos.FirstName,
//                    LastName = _secondaryClientDetailsDtos.LastName,
//                    Society = _secondaryClientDetailsDtos.Society,
//                    Locality = _secondaryClientDetailsDtos.Locality,
//                    City = _secondaryClientDetailsDtos.City,
//                    StateId = _secondaryClientDetailsDtos.StateId,
//                    CountryId = _secondaryClientDetailsDtos.CountryId,
//                    PhoneNumber = _secondaryClientDetailsDtos.PhoneNumber,
//                    AlternatePhoneNumber = _secondaryClientDetailsDtos.AlternatePhoneNumber,
//                    DOB = _secondaryClientDetailsDtos.DOB,
//                    CreationDate = _secondaryClientDetailsDtos.CreationDate,
//                    UpdationDate = DateTime.Now,
//                    ImageUrl = _secondaryClientDetailsDtos.ImageUrl,
//                    formFile = _secondaryClientDetailsDtos.formFile,
//                    SecondaryClientPanImageUrl = _secondaryClientDetailsDtos.SecondaryClientPanImageUrl,
//                    SecondaryClientAadharImageUrl = _secondaryClientDetailsDtos.SecondaryClientAadharImageUrl,
//                    SecondaryClientPanformFile = _secondaryClientDetailsDtos.SecondaryClientPanformFile,
//                    SecondaryClientAadharformFile = _secondaryClientDetailsDtos.SecondaryClientAadharformFile,
//                    EnrolledId = _secondaryClientDetailsDtos.EnrolledId,
//                    CIFNumber = _secondaryClientDetailsDtos.CIFNumber,
//                    SecondaryClientAadharNumber = _secondaryClientDetailsDtos.SecondaryClientAadharNumber,
//                    SecondaryClientPanNumber = _secondaryClientDetailsDtos.SecondaryClientPanNumber
//                };


//                /* if (existingClient != null)
//                 {
//                     return BadRequest("This UserDetails already added. Please go for updating UserDetails.");
//                 }*/

//                if (_secondaryClientDetailsDtos.formFile != null)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        _secondaryClientDetailsDtos.formFile.CopyToAsync(memoryStream);
//                        var imageBytes = memoryStream.ToArray();

//                        secondaryClientDetailsDtos.ImageUrl = imageBytes;
//                    }
//                }
//                if (_secondaryClientDetailsDtos.SecondaryClientPanformFile != null)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        _secondaryClientDetailsDtos.SecondaryClientPanformFile.CopyToAsync(memoryStream);
//                        var imageBytes = memoryStream.ToArray();

//                        secondaryClientDetailsDtos.SecondaryClientPanImageUrl = imageBytes;
//                    }
//                }
//                if (_secondaryClientDetailsDtos.SecondaryClientAadharformFile != null)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        _secondaryClientDetailsDtos.SecondaryClientAadharformFile.CopyToAsync(memoryStream);
//                        var imageBytes = memoryStream.ToArray();

//                        secondaryClientDetailsDtos.SecondaryClientAadharImageUrl = imageBytes;
//                    }
//                }

//                // Checking the existence of countries and states
//                var existingCountry = _unitOfWork.Country.GetCountries();
//                if (existingCountry == null)
//                {
//                    return BadRequest("Country Not Found.");
//                }
//                var existingState = _unitOfWork.State.GetStates();
//                if (existingState == null)
//                {
//                    return BadRequest("State Not Found");
//                }

//                var existingClient = _unitOfWork.SecondaryClientDetail.GetSecondaryClientDetail(secondaryClientDetailsDtos.SecondaryClientEmail);
//                _userDbContext.Entry(existingClient).State = EntityState.Detached;
//                 secondaryClientDetailsDtos.CreationDate = existingClient.CreationDate;
//                secondaryClientDetailsDtos.UpdationDate = DateTime.Now;
//                _unitOfWork.SecondaryClientDetail.UpdateSecondaryClientDetails(secondaryClientDetailsDtos);
//                _unitOfWork.Save();

//                return Ok("SecondaryClientDetails Updates successfully");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Failed to add SecondaryClientDetails: {ex.Message}");
//            }
//        }


       

//        [HttpDelete("delete-secondary-client-details/{email}")]
//        public ActionResult DeleteSecondaryClientDetails(string email)
//        {
//            try
//            {
//                _unitOfWork.SecondaryClientDetail.DeleteSecondaryClientDetails(email);

//                return Ok("SecondaryClientDetails deleted successfully");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Failed to delete SecondaryClientDetails: {ex.Message}");
//            }
//        }

        
//    }
//}
