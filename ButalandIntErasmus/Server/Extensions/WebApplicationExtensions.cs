using ButalandIntErasmus.Server.DataAccess.Models;
using ButalandIntErasmus.Server.Services.Interfaces;
using ButalandIntErasmus.Shared.DTOs;
using ButalandIntErasmus.Server.DataAccess;
using Microsoft.AspNetCore.Identity;


namespace ButalandIntErasmus.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/user/register", RegisterHandler);
        app.MapPost("/user/login", LoginHandler);
        return app;
    }

    private static async Task<IResult> LoginHandler(IAuthService authService, UserLoginDto user)
    {
        var response = await authService.Login(user.Email, user.Password);
        return response.Success ? Results.Ok(response) : Results.BadRequest(response);
    }

    private static async Task<IResult> RegisterHandler(IAuthService authService, UserRegisterDto userDto)
    {
        var user = new UserModel() { Email = userDto.Email };
        var response = await authService.RegisterUserAsync(user, userDto.Password);
        return response.Success ? Results.Ok(response) : Results.BadRequest(response);
    }
}