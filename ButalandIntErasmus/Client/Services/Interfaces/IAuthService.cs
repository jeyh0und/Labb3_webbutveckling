using ButalandIntErasmus.Shared.DTOs;
using ButalandIntErasmus.Shared;

namespace ButalandIntErasmus.Client.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse<int>?> RegisterAsync(UserRegisterDto userDto);
    Task<ServiceResponse<string>?> LoginAsync(UserLoginDto userDto);
}