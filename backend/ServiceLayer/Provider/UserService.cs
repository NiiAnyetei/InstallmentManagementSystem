using DataLayer.Context;
using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RTools_NTS.Util;
using ServiceLayer.Extensions;
using ServiceLayer.Service;
using ServiceLayer.Utils.Interfaces;

namespace ServiceLayer.Provider
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMSDbContext _context;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(ILogger<UserService> logger, IMSDbContext context, IHasher hasher, ITokenGenerator tokenGenerator)
        {
            _logger = logger;
            _context = context;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginUserResponseDto> LoginAsync(LoginUserDto user)
        {
            try
            {
                var userFromDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);

                if (userFromDb == null) throw new Exception("Invalid credentials");

                var isValidPassword = _hasher.VerifyPassword(user.Password, userFromDb.Password, userFromDb.Salt);

                if (!isValidPassword) throw new Exception("Invalid credentials");

                var token = _tokenGenerator.GenerateToken(userFromDb.Username);

                UserRefreshToken userRefreshToken = new UserRefreshToken
                {
                    RefreshToken = token.RefreshToken,
                    UserName = userFromDb.Username
                };

                await InvalidateSavedRefreshTokens(userFromDb.Username);
                await AddUserRefreshToken(userRefreshToken);

                return new LoginUserResponseDto(token.AccessToken, token.RefreshToken, token.ExpiresAt, token.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while logging in");
                throw;
            }
        }

        public async Task<NewRefreshTokenResponseDto> RefreshTokenAsync(NewRefreshTokenDto newRefreshTokenDto)
        {
            try
            {
                var principal = _tokenGenerator.GetPrincipalFromExpiredToken(newRefreshTokenDto.AccessToken);
                var username = principal.GetUsername();
                var savedRefreshToken = await GetSavedRefreshTokens(username, newRefreshTokenDto.RefreshToken);

                if (savedRefreshToken.RefreshToken != newRefreshTokenDto.RefreshToken) throw new Exception("Invalid token");

                var token = _tokenGenerator.GenerateToken(username);

                UserRefreshToken userRefreshToken = new UserRefreshToken
                {
                    RefreshToken = token.RefreshToken,
                    UserName = username
                };

                await DeleteUserRefreshToken(username, newRefreshTokenDto.RefreshToken);
                await AddUserRefreshToken(userRefreshToken);

                return new NewRefreshTokenResponseDto(token.AccessToken, token.RefreshToken, token.ExpiresAt, token.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while refreshing token");
                throw;
            }
        }

        public async Task<UserDto> CreateAsync(NewUserDto user)
        {
            try
            {
                byte[] saltBytes = _hasher.GenerateSalt();
                string hashedPassword = _hasher.HashPassword(user.Password, saltBytes);
                string base64Salt = Convert.ToBase64String(saltBytes);

                var newUser = new User(user);
                newUser.Password = hashedPassword;
                newUser.Salt = base64Salt;

                if (await _context.Users.AnyAsync(u => u.Username == user.Username)) throw new Exception("Username unavailable");

                if (await _context.Users.AnyAsync(u => u.Email == user.Email)) throw new Exception("Email already taken");

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                var dto = newUser.ToUserDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating user");
                throw;
            }
        }

        public async Task<UserDto> GetAsync(string username)
        {
            try
            {
                var userFromDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

                if (userFromDb == null) throw new Exception("Invalid credentials");

                var token = _tokenGenerator.GenerateToken(userFromDb.Username);
                var dto = userFromDb.ToUserDto();

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting user");
                throw;
            }
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            return _context.Users.FirstAsync(x => x.Username == username);
        }

        public async Task<UserDto> UpdateAsync(string username, UpdatedUserDto updatedUser)
        {
            try
            {
                var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                if (userFromDb == null) throw new Exception("User not found");

                if (await _context.Users.AnyAsync(u => u.Username == updatedUser.Username)) throw new Exception("Username unavailable");

                if (await _context.Users.AnyAsync(u => u.Email == updatedUser.Email)) throw new Exception("Email already taken");

                userFromDb.UpdateUser(updatedUser);
                await _context.SaveChangesAsync();

                var token = _tokenGenerator.GenerateToken(userFromDb.Username);
                var dto = userFromDb.ToUserDto();

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while updating user");
                throw;
            }
        }

        private async Task AddUserRefreshToken(UserRefreshToken userRefreshToken)
        {
            try
            {
                await _context.UserRefreshTokens.AddAsync(userRefreshToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding user refresh token");
                throw;
            }
        }

        private async Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshToken)
        {
            try
            {
                var userRefreshToken =  await _context.UserRefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken && x.IsActive == true);

                if (userRefreshToken == null) throw new Exception("Invalid");

                return userRefreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching user refresh token");
                throw;
            }
        }
        
        private async Task InvalidateSavedRefreshTokens(string username)
        {
            try
            {
                var userRefreshTokens =  await _context.UserRefreshTokens.AsNoTracking().Where(x => x.UserName == username).ToListAsync();

                if (userRefreshTokens != null) await _context.BulkDeleteAsync(userRefreshTokens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while invalidating user refresh tokens");
                throw;
            }
        }

        private async Task DeleteUserRefreshToken(string username, string refreshToken)
        {
            try
            {
                var item = await _context.UserRefreshTokens.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken);
                if (item != null)
                {
                    _context.UserRefreshTokens.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while deleting user refresh token");
                throw;
            }
        }
    }
}
