using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AirLineManager.dal;
using AirLineManager.dal.Entities;
using AirLineManager.webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirLineManager.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly Mapper _map;
        private readonly IUnitOfWork _repo;

        public AuthenticateController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUnitOfWork repo,
            Mapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _map = mapper;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username)) return BadRequest("Username is not valid");
            if (string.IsNullOrEmpty(model.Password)) return BadRequest("Password is not valid");

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password) && !string.IsNullOrEmpty(user.UserName))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var token = GetToken(authClaims);

                var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role,
                    username = user.UserName
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.Username)) return BadRequest("Username is not valid");
            if (string.IsNullOrEmpty(model.Password)) return BadRequest("Password is not valid");

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await _roleManager.RoleExistsAsync(UserRoles.User)) await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if (await _roleManager.RoleExistsAsync(UserRoles.User)) await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (model.Username == null || model.Username == string.Empty) return BadRequest("Username is not valid");
            if (model.Password == null || model.Password == string.Empty) return BadRequest("Password is not valid");

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin)) await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User)) await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));


            if (await _roleManager.RoleExistsAsync(UserRoles.Admin)) await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserModel> result = new();
            foreach (User user in users) result.Add(_map.MapEntityToModel(user, _userManager.GetRolesAsync(user).Result.ToArray()[0]));
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("GetSingle")]
        public IActionResult GetSingle(string id)
        {
            User? entity = _repo.UserRepo.Get(id);
            if (entity == null) return NotFound("Entity not found");
            return Ok(_map.MapEntityToModel(entity, _userManager.GetRolesAsync(entity).Result.ToArray()[0]));
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Update")]
        public IActionResult Update(UserModel model)
        {
            User? old = _repo.UserRepo.Get(model.UserId);
            if (old == null || old.IsDeleted) NotFound("Entity not found");

            ReturnRequest result = _repo.UserRepo.Update(_map.MapModelToEntity(model));
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpPut]
        /*[Authorize]*/
        [Route("SelfUpdate")]
        public IActionResult SelfUpdate(UserModel model)
        {

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return NotFound("UserId not found");
            List<User>? entity = _repo.UserRepo.GetByFilter(u => u.UserName == userName);
            if (entity == null || entity.Count == 0) return NotFound("User not found");
            model.UserId = entity[0].Id;

            ReturnRequest result = _repo.UserRepo.Update(_map.MapModelToEntity(model));
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpDelete]
        /*[Authorize]*/
        [Route("SelfDelete")]
        public IActionResult SelfDelete()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName)) return NotFound("UserId not found");
            List<User>? entity = _repo.UserRepo.GetByFilter(u => u.UserName == userName);
            if (entity == null || entity.Count == 0) return NotFound("User not found");

            entity[0].IsDeleted = true;

            ReturnRequest result = _repo.UserRepo.Update(entity[0]);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Delete")]
        public IActionResult Delete(string id)
        {
            User? user = _repo.UserRepo.Get(id);
            if (user == null || user.IsDeleted) return NotFound("Entity not found");

            user.IsDeleted = true;

            ReturnRequest result = _repo.UserRepo.Update(user);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Restore")]
        public IActionResult Restore(string id)
        {
            User? user = _repo.UserRepo.Get(id);
            if (user == null || !user.IsDeleted) return NotFound("Entity not found");

            user.IsDeleted = false;

            ReturnRequest result = _repo.UserRepo.Update(user);
            if (result.Return) return Ok(result.MessageReturn);
            return NotFound(result.MessageReturn);
        }

        [HttpGet]
        [Route("Test")]
        public IActionResult Test() => Ok("It works");
    }
}