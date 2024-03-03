using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Models.Models.DTOs.UserLoginHistoryDTOs;
using Serilog;
using Sugamta.API.MappingConfig.UserLoginHistoryProfile;
using Sugamta.API.Repository.Interface;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sugamta.API.Controllers
{
    [Route("api/LoginHistory")]
    [ApiController]
    public class UserLoginHistoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMappingConfig _mappingConfig;

        public UserLoginHistoryController(IUnitOfWork unitOfWork, IConfiguration configuration, IMappingConfig mappingConfig)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mappingConfig = mappingConfig;
        }

        [HttpGet("getalluserloginhistory")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserLoginHistory>> GetAllUserLoginHistory()
        {
            try
            {
                //IEnumerable<UserLoginHistory> userLoginHistories = _unitOfWork.UserLoginHistory.GetAllLoginHistory();
                var userLoginHistories = _mappingConfig.MapUserLoginHistoryToUserLoginHistoryDto();
                if (userLoginHistories == null || userLoginHistories.Any() == false)
                {
                    Log.Information("No Data is available in User Login History Table in Database.");
                    ModelState.AddModelError("Empty Data", "No Login History Found.");
                    return NotFound(ModelState);
                }

                Log.Information("User Login History: ");
                foreach (var item in userLoginHistories)
                {
                    Log.Information($"User Id: {item.LoginHistoryId} , User Email: {item.Email}, Last Login Time: {item.LastLoginTime}, Role Id: {item.RoleId}, Role Type: {item.RoleType}");
                }

                return Ok(userLoginHistories);
            }
            catch (Exception ex)
            {
                Log.Information($"{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getuserloginhistorybyemail")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserLoginHistory>> GetUserLoginHistoryByEmail([EmailAddress] string email)
        {
            try
            {
                //IEnumerable<UserLoginHistory> userLogins = _unitOfWork.UserLoginHistory.GetLoginHistoryByEmail(e => e.Email == email).ToList();
                var userLogins = _mappingConfig.MapSingleUserLoginHistoryDto(email);
                if (userLogins == null || userLogins.Any() == false)
                {
                    ModelState.AddModelError("Empty", "No Login History found for " + email);
                    return NotFound(ModelState);
                }
                return Ok(userLogins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createloginhistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private ActionResult CreateUserLoginHistory(CreateUserLoginHistoryDto createUserLoginHistory)
        {
            try
            {
                if (createUserLoginHistory == null)
                {
                    Log.Error("User Details are not provided in CreateUserLoginHistory function to create login entry in Database.");
                    return BadRequest();
                }
                else if (createUserLoginHistory.Email == null)
                {
                    Log.Error("User Email is null which is required to create login entry.");
                    ModelState.AddModelError("Email Error", "Email cannot be blank.");
                    return BadRequest(ModelState);
                }

                var user = _unitOfWork.user.GetUser(createUserLoginHistory.Email);
                if (user == null)
                {
                    Log.Error($"There is no user with mail as {createUserLoginHistory.Email} in User Table in Database. Create a user first with same email to create user login history record.");
                    ModelState.AddModelError("No User Error", $"User with the email {createUserLoginHistory.Email} is not found.");
                    return NotFound(ModelState);
                }

                var mappedUserLoginHistory = _mappingConfig.MapCreateUserLoginHistoryDtoToUserLoginHistory(createUserLoginHistory);

                _unitOfWork.UserLoginHistory.CreateLoginHistory(mappedUserLoginHistory);
                _unitOfWork.Save();

                Log.Information($"Login entry has been created for user {mappedUserLoginHistory.Email} at {mappedUserLoginHistory.LastLoginTime} as {mappedUserLoginHistory.RoleType} Role.");
                return Ok("User Login History is Created Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                User _user = _unitOfWork.user.GetUser(userLoginDto.Email);

                User user = new()
                {
                    UserID = _user.UserID,
                    Email = _user.Email,
                    IsDeleted = _user.IsDeleted,
                    RoleId = _user.RoleId,
                    Password = _user.Password,
                    Name = _user.Name
                };

                if (user == null)
                {
                    Log.Warning("User with the current email does not exist.");
                    ModelState.AddModelError("User Null Error", "User does not exist. Please Register your account first.");
                    return NotFound(ModelState);
                }

                if (user.IsDeleted == 1)
                {
                    Log.Information($"Account with {user.Email} and {user.UserID} has been deleted but it might exist in database.");
                    return BadRequest("Your account has been deleted.");
                }

                

                if (BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
                {
                    var tokenString = GenerateJwtToken(user/*, userDetails*/);

                    // Log successful login
                    Log.Information($"User successfully logged in. {user.Email}");

                    var roleType = _unitOfWork.Role.GetRoleById(i => i.RoleId == user.RoleId).RoleType;

                    var createUserLoginHistoryDto = _mappingConfig.MapUserToCreateUserLoginHistoryDto(user);
                    createUserLoginHistoryDto.LastLoginTime = DateTime.Now;
                    createUserLoginHistoryDto.RoleType = roleType;

                    CreateUserLoginHistory(createUserLoginHistoryDto);

                    // Return the token 
                    Log.Information($"Generated Jwt Token for {user.Email}: {tokenString}");
                    return Ok(new { Token = tokenString });
                }
                else
                {
                    // Passwords do not match
                    Log.Error("Account password did not match with email provided for login.");
                    return Unauthorized("Invalid password");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(User user/*, UserDetails userDetails*/)
        {
            var key = _configuration["Jwt:Key"];

            var tokenHandler = new JwtSecurityTokenHandler();

            var roleType = _unitOfWork.Role.GetRoleById(i => i.RoleId == user.RoleId).RoleType;
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.UserID.ToString()),
                    new Claim("email", user.Email),
                    new Claim("role", roleType.ToString()),
                    new Claim("name", user.Name),
                    
                }),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(2), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("generate-link")]
        public IActionResult GenerateLink([FromForm] LinkGenerationDto link)
        {
            if (link != null)
            {
                link.LinkGenerationDate = DateTime.Now;
                link.IsActive = 1;

                LinkGeneration generatedLink = new()
                {
                    RegistrationLink = link.RegistrationLink,
                    LinkGenerationDate = DateTime.Now,
                    IsActive = 1
                };

                _unitOfWork.LinkGenerate.GenerateLink(generatedLink);
                _unitOfWork.Save();
                return Ok();
            }

            return NotFound("Generated Link is null");
        }

        [HttpGet("check-generated-link/{generatedLink}")]
        public IActionResult CheckGeneratedLink(string generatedLink)
        {
            if (generatedLink != null)
            {
                var encodedLink = Uri.UnescapeDataString(generatedLink);

                string decodedUniqueCode = Uri.UnescapeDataString(encodedLink.Replace(" ", "+"));

                var link = _unitOfWork.LinkGenerate.GetGeneratedLinkByLink(l => l.RegistrationLink == decodedUniqueCode);

                if (link != null && link.IsActive == 1)
                {
                    TimeSpan timeDifference = DateTime.Now - link.LinkGenerationDate;
                    if (timeDifference.TotalHours <= 2)
                    {
                        return Ok();
                    }
                    return BadRequest("Link is expired");
                }
            }

            return NotFound("Generated Link is null");
        }

        [HttpPost]
        public void SetExpiredLinksInactive()
        {
            var expiredLinks = _unitOfWork.LinkGenerate.GetLinksWhere(l => l.IsActive == 1 && l.LinkGenerationDate < DateTime.Now.AddHours(-2));

            foreach (var link in expiredLinks)
            {
                link.IsActive = 0;
            }

            _unitOfWork.Save();
        }
    }
}
