using AutoMapper;
using Entities.Dtos.UserDtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Data;

namespace WebApi.Services.AuthotizationService
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IConfiguration _configuration;

        public AuthorizationService(DataContext context, IMapper mapper, IConfiguration configuration) : base(context, mapper) 
        {
            _configuration = configuration;
        }

        public async Task<ServiceResponce<GetUserDto>> GetUserByAuthAsync(AuthUserDto authUser)
        {
            var responce = new ServiceResponce<GetUserDto>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Username == authUser.Username)
                    ?? throw new Exception("The user does not exist or the password is incorrect.");

                if(user.PasswordHash != authUser.PasswordHash)
                    throw new Exception("The user does not exist or the password is incorrect.");

                var token = CreateToken(user);

                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken, user);

                user.Token = token;

                await _context.SaveChangesAsync();

                responce.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<GetUserDto>> ClearTokenAsync(int id)
        {
            var responce = new ServiceResponce<GetUserDto>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == id)
                    ?? throw new Exception($"User with Id '{id}' not found!");

                user.Token = string.Empty;
                user.RefreshToken = string.Empty;

                await _context.SaveChangesAsync();

                responce.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            }

            return responce;
        }

        public async Task<ServiceResponce<GetUserDto>> RefreshTokenAsync(TokenUserDto user)
        {
            var responce = new ServiceResponce<GetUserDto>();

            try
            {
                var requestedUser = await _context.Users
                    .FirstOrDefaultAsync(n => n.Id == user.Id)
                    ?? throw new Exception($"User with Id '{user.Id}' not found!");

                var refreshToken = user.RefreshToken;

                if (user.RefreshToken.Equals(refreshToken))
                    throw new Exception("Invalid Refresh Token.");

                else if (user.TokenExpires < DateTime.Now)
                    throw new Exception("Token Expired.");

                string token = CreateToken(requestedUser);

                requestedUser.Token = token;

                var newRefreshToken = GenerateRefreshToken();
                SetRefreshToken(newRefreshToken, requestedUser);

                await _context.SaveChangesAsync();

                responce.Data = _mapper.Map<GetUserDto>(requestedUser);
            }
            catch (Exception ex)
            {
                responce.IsSuccessful = false;
                responce.Message = ex.Message;
            } 

            return responce;
        }

        private void SetRefreshToken(RefreshToken refreshToken, User user)
        {
            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.Expires;
            user.TokenCreated = refreshToken.Created;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddHours(1),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims
                , expires: DateTime.Now.AddHours(1)
                , signingCredentials: creds);

            var JWT = new JwtSecurityTokenHandler().WriteToken(token);

            return JWT;
        }
    }
}
