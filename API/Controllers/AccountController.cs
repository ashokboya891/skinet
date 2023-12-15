

using System.Security.Claims;
using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entites.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
        ITokenService tokenService,IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;

        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // this line cmntd after addinf below line var email=User.FindFirstValue(ClaimTypes.Email);  //=httpcontext.User?.Claims?.FirstOrDefaults(x=>x.Type=claimtypes.Email)?.value;
            var user =await _userManager.FindByEmailFromClaimsPrinciple(User);
            return new UserDto{
                Email=user.Email,
                DisplayName=user.DisplayName,
                Token=_tokenService.CreateToken(user)
            } ;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if(user==null) return Unauthorized(new ApiResponse(401));
            var result=await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return new UserDto
            {
                Email=user.Email,
                DisplayName=user.DisplayName,
                Token=_tokenService.CreateToken(user)
                
            };
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors=new [] {"Email Address is in Use"}});
            }
            var user=new AppUser
            {
                DisplayName=registerDto.DisplayName,
                Email=registerDto.Email,
                UserName=registerDto.Email
            };
            var result=await _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));
            return new UserDto{
                Email=registerDto.Email,
                DisplayName=registerDto.DisplayName,
                Token=_tokenService.CreateToken(user)
            };
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery]string email)
        {
            return await _userManager.FindByEmailAsync(email)!=null;
        }
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            //  this line cmntd after usermangerextension  var email=User.FindFirstValue(ClaimTypes.Email);  //=httpcontext.User?.Claims?.FirstOrDefaults(x=>x.Type=claimtypes.Email)?.value;
            var user= await _userManager.FindUserByClaimsPrincipleWithAddressAsync(User);
            return _mapper.Map<Address,AddressDto>(user.Address);

        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user= await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);
            user.Address=_mapper.Map<AddressDto,Address>(address);
            var result= await _userManager.UpdateAsync(user);
            if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));
            return BadRequest("Problem updating user");

        }
    }
}