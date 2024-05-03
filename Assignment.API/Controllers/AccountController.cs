using Assignment.Common;
using Assignment.Model.Domain;
using Assignment.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Assignment.API.Controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly IConfiguration _iconfiguration;
        private readonly SignInManager<Users> _signInManager;

        public AccountController(UserManager<Users> userManager, IConfiguration iconfiguration, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _iconfiguration = iconfiguration;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReqDto reqDto)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Check Model Validation
                if (ModelState.IsValid)
                {
                    //Assign Values To User Domain
                    Users user = new Users()
                    {
                        Email = reqDto.Email,
                        UserName = reqDto.Email
                    };

                    //Create User
                    var identityUserResp = await _userManager.CreateAsync(user, reqDto.Password);

                    //Check IdentityUser Result
                    if(identityUserResp.Succeeded) 
                    {
                        responseDto.Status = ResponseStatus.Success;
                        responseDto.Message = "User Register Successfully";
                        return this.StatusCode(StatusCodes.Status201Created, responseDto);
                    }
                    else
                    {
                        //Return IdentityUser Validation 
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = string.Join(',', identityUserResp.Errors.Select(s => s.Description).ToList());
                        return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
                    }
                }
                else
                {
                    //Return ModelState Validation
                    responseDto.Status = ResponseStatus.Fail;
                    responseDto.Message = string.Join(',', ModelState.Values.SelectMany(sm => sm.Errors).Select(s => s.ErrorMessage).ToList());
                    return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);

                }
            }
            catch (Exception ex)
            {
                //Return Exception
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDto reqDto)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Check Model Validation
                if (ModelState.IsValid)
                {
                    //Check Email Existing
                    var user = await _userManager.FindByEmailAsync(reqDto.Email);
                    if(user != null ) 
                    {
                        //Check Password
                        var isValidPassword = await _signInManager.CheckPasswordSignInAsync(user, reqDto.Password, true);
                        if(isValidPassword.Succeeded)
                        {
                            //Create JWT Claims
                            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                    new Claim(ClaimTypes.Name, user.UserName),
                                    new Claim(ClaimTypes.Email, user.Email),
                                }),
                                Expires = DateTime.UtcNow.AddMinutes(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                            };

                            //Create JWT Token
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                                                        
                            //Return JWT Token 
                            responseDto.Status = ResponseStatus.Success;
                            responseDto.Message = "Success";
                            responseDto.Data = new TokenDto{ Email = user.Email, AccessToken = tokenHandler.WriteToken(token), TokenType = "Bearer" };
                            return this.StatusCode(StatusCodes.Status201Created, responseDto);
                        }
                        else
                        {
                            //Return Fail Response
                            responseDto.Status = ResponseStatus.Fail;
                            responseDto.Message = "Invalid Email Address and Password";
                            return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
                        }
                    }
                    else
                    {
                        //Return Fail Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Invalid Email Address";
                        return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
                    }
                }
                else
                {
                    //Return ModelState Validation
                    responseDto.Status = ResponseStatus.Fail;
                    responseDto.Message = string.Join(',', ModelState.Values.SelectMany(sm => sm.Errors).Select(s => s.ErrorMessage).ToList());
                    return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);

                }
            }
            catch (Exception ex)
            {
                //Return Exception
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
    }
}
