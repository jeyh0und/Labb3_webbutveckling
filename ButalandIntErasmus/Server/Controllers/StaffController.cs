using ButalandIntErasmus.Server.DataAccess;
using ButalandIntErasmus.Server.DataAccess.Models;
using ButalandIntErasmus.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ButalandIntErasmus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffController : Controller
{
    private readonly StaffDbContext _db;
    public StaffController(StaffDbContext db)
    {
        _db = db;
    }
    [HttpGet("allstaff")]
    public async Task<List<StaffDto>> GetAllAsync()
    {
        return (await _db.Staff.ToListAsync()).Select(s => new StaffDto { Name = s.Name, Description = s.Description, Email = s.Email }).ToList();
    }
    [HttpPost("add-staff")]
    public async Task AddNewStaffAsync(StaffDto staffDto)
    {
        try
        {
            await _db.AddAsync(new StaffModel { Id = new Guid(), Name = staffDto.Name, Description = staffDto.Description, Email = staffDto.Email });
            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


}
