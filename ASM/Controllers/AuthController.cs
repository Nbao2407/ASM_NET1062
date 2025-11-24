using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.DTOs;
using ASM.Models;
using ASM.Services;

namespace ASM.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtService _jwtService;
    private readonly IGoogleAuthService _googleAuthService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ApplicationDbContext context,
        IPasswordHashService passwordHashService,
        IJwtService jwtService,
        IGoogleAuthService googleAuthService,
        ILogger<AuthController> logger)
    {
        _context = context;
        _passwordHashService = passwordHashService;
        _jwtService = jwtService;
        _googleAuthService = googleAuthService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new customer account
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check for duplicate email
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (existingUser != null)
        {
            return BadRequest(new { message = "Email address is already registered" });
        }

        // Hash password
        var passwordHash = _passwordHashService.HashPassword(request.Password);

        // Create new user
        var user = new User
        {
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            DateOfBirth = request.DateOfBirth,
            Role = "Customer",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New user registered: {Email}", user.Email);

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        });
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Find user by email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Verify password
        if (!_passwordHashService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Account is inactive" });
        }

        _logger.LogInformation("User logged in: {Email}", user.Email);

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        });
    }

    /// <summary>
    /// Login or register with Google OAuth
    /// </summary>
    [HttpPost("google")]
    public async Task<ActionResult<AuthResponse>> GoogleAuth([FromBody] GoogleAuthRequest request)
    {
        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verify Google ID token
        var payload = await _googleAuthService.VerifyGoogleTokenAsync(request.IdToken);
        if (payload == null)
        {
            return Unauthorized(new { message = "Invalid Google token" });
        }

        // Check if user exists with this Google ID
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

        if (user == null)
        {
            // Check if user exists with this email
            user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == payload.Email.ToLower());

            if (user != null)
            {
                // Link Google account to existing user
                user.GoogleId = payload.Subject;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Google account linked to existing user: {Email}", user.Email);
            }
            else
            {
                // Create new user from Google account
                user = new User
                {
                    Email = payload.Email.ToLower(),
                    PasswordHash = _passwordHashService.HashPassword(Guid.NewGuid().ToString()), // Random password for OAuth users
                    FullName = payload.Name ?? payload.Email,
                    PhoneNumber = "", // Will need to be updated by user
                    Address = null,
                    DateOfBirth = null,
                    GoogleId = payload.Subject,
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New user created via Google OAuth: {Email}", user.Email);
            }
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Account is inactive" });
        }

        _logger.LogInformation("User logged in via Google: {Email}", user.Email);

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        });
    }
}
