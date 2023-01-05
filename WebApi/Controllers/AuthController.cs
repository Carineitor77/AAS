using Application.DTOs;
using Application.Services.Interfaces;
using Domain;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IRepository<User> userRepository, IRepository<Role> roleRepository, 
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email)) return BadRequest("Email is taken");

            var role = await GetRole(registerDto.Role.ToLower());
            if (role == null) return BadRequest($"Role {registerDto.Role} don't exist");
            if (role.Name == "director") return BadRequest("Director can't be registered");

            using var hmac = new HMACSHA512();

            // here must be mapping!!!
            var user = new User()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                RoleId = role.Id
            };

            var res = await _userRepository.AddAsync(user);

            if (res != null)
            {
                return Ok(new UserDto
                {
                    Email = res.Email,
                    Role = res.Role!.Name.ToLower(),
                    Token = _tokenService.CreateToken(res)
                });
            }
            return BadRequest();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository
                .Query()
                .Include(user => user.Role)
                .Include(user => user.Lesson)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt!);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash![i])
                {
                    return Unauthorized("Invalid password");
                }
            }

            return Ok(new UserDto
            {
                Email = user.Email,
                Role = user.Role!.Name.ToLower(),
                Token = _tokenService.CreateToken(user)
            });
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(u => u.Email == email) != null;
        }
            
        private async Task<Role?> GetRole(string role)
        {
            return await _roleRepository.FirstOrDefaultAsync(r => r.Name == role);
        }
    }
}
