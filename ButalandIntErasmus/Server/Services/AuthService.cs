using ButalandIntErasmus.Server.DataAccess;
using ButalandIntErasmus.Server.DataAccess.Models;
using ButalandIntErasmus.Server.Services.Interfaces;
using ButalandIntErasmus.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ButalandIntErasmus.Server.Services;

public class AuthService : IAuthService
{
    private readonly StaffDbContext _peopleContext;
    private readonly IConfiguration _configuration;

    public AuthService(StaffDbContext peopleContext, IConfiguration configuration)
    {
        _peopleContext = peopleContext;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<int>> RegisterUserAsync(UserModel user, string password)
    {
        if (await CheckUserExistsAsync(user.Email))
        {
            return new ServiceResponse<int>()
            {
                Success = false,
                Message = "User already exists!"
            };
        }
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _peopleContext.LoginUsers.AddAsync(user);
        await _peopleContext.SaveChangesAsync();

        return new ServiceResponse<int>()
        {
            Data = user.Id,
            Success = true,
            Message = "Registration Successful!"
        };
    }


    public async Task<bool> CheckUserExistsAsync(string email)
    {
        return await _peopleContext.LoginUsers
            .AnyAsync(user => user.Email
            .ToLower()
            .Equals(email.ToLower()));
    }
    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>();

        if (!await CheckUserExistsAsync(email))
        {
            response.Success = false;
            response.Message = "Wrong username or password!";
            return response;
        }
        var user = await _peopleContext.LoginUsers.FirstAsync(u => u.Email == email);

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong username or password!";
            return response;
        }

        response.Success = true;
        response.Message = "Login Successful!";

        response.Data = CreateToken(user);

        return response;
    }

    private string CreateToken(UserModel user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
        };

        var secret = _configuration.GetSection("AppSettings:Token").Value;

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        using (var hmac = new HMACSHA512(userPasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(userPasswordHash);
        }
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
