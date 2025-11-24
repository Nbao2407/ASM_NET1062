using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.DTOs;
using ASM.Models;

namespace ASM.Controllers;

[ApiController]
[Route("api/items")]
public class FoodItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FoodItemsController> _logger;

    public FoodItemsController(
        ApplicationDbContext context,
        ILogger<FoodItemsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all food items with optional category filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodItemResponse>>> GetFoodItems([FromQuery] string? category = null)
    {
        var query = _context.FoodItems.Where(f => f.IsActive);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(f => f.Category.ToLower() == category.ToLower());
        }

        var foodItems = await query
            .OrderBy(f => f.Category)
            .ThenBy(f => f.Name)
            .ToListAsync();

        var response = foodItems.Select(f => new FoodItemResponse
        {
            FoodItemId = f.FoodItemId,
            Name = f.Name,
            Description = f.Description,
            Price = f.Price,
            Category = f.Category,
            Theme = f.Theme,
            ImageUrl = f.ImageUrl,
            Ingredients = f.Ingredients,
            NutritionalInfo = f.NutritionalInfo,
            AllergenWarnings = f.AllergenWarnings,
            IsActive = f.IsActive,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt
        });

        return Ok(response);
    }

    /// <summary>
    /// Get a specific food item by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<FoodItemResponse>> GetFoodItem(int id)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);

        if (foodItem == null || !foodItem.IsActive)
        {
            return NotFound(new { message = "Food item not found" });
        }

        var response = new FoodItemResponse
        {
            FoodItemId = foodItem.FoodItemId,
            Name = foodItem.Name,
            Description = foodItem.Description,
            Price = foodItem.Price,
            Category = foodItem.Category,
            Theme = foodItem.Theme,
            ImageUrl = foodItem.ImageUrl,
            Ingredients = foodItem.Ingredients,
            NutritionalInfo = foodItem.NutritionalInfo,
            AllergenWarnings = foodItem.AllergenWarnings,
            IsActive = foodItem.IsActive,
            CreatedAt = foodItem.CreatedAt,
            UpdatedAt = foodItem.UpdatedAt
        };

        return Ok(response);
    }

    /// <summary>
    /// Create a new food item (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FoodItemResponse>> CreateFoodItem([FromBody] CreateFoodItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var foodItem = new FoodItem
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            Theme = request.Theme,
            ImageUrl = request.ImageUrl,
            Ingredients = request.Ingredients,
            NutritionalInfo = request.NutritionalInfo,
            AllergenWarnings = request.AllergenWarnings,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.FoodItems.Add(foodItem);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Food item created: {Name} (ID: {Id})", foodItem.Name, foodItem.FoodItemId);

        var response = new FoodItemResponse
        {
            FoodItemId = foodItem.FoodItemId,
            Name = foodItem.Name,
            Description = foodItem.Description,
            Price = foodItem.Price,
            Category = foodItem.Category,
            Theme = foodItem.Theme,
            ImageUrl = foodItem.ImageUrl,
            Ingredients = foodItem.Ingredients,
            NutritionalInfo = foodItem.NutritionalInfo,
            AllergenWarnings = foodItem.AllergenWarnings,
            IsActive = foodItem.IsActive,
            CreatedAt = foodItem.CreatedAt,
            UpdatedAt = foodItem.UpdatedAt
        };

        return CreatedAtAction(nameof(GetFoodItem), new { id = foodItem.FoodItemId }, response);
    }

    /// <summary>
    /// Update an existing food item (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FoodItemResponse>> UpdateFoodItem(int id, [FromBody] UpdateFoodItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var foodItem = await _context.FoodItems.FindAsync(id);

        if (foodItem == null)
        {
            return NotFound(new { message = "Food item not found" });
        }

        foodItem.Name = request.Name;
        foodItem.Description = request.Description;
        foodItem.Price = request.Price;
        foodItem.Category = request.Category;
        foodItem.Theme = request.Theme;
        foodItem.ImageUrl = request.ImageUrl;
        foodItem.Ingredients = request.Ingredients;
        foodItem.NutritionalInfo = request.NutritionalInfo;
        foodItem.AllergenWarnings = request.AllergenWarnings;
        foodItem.IsActive = request.IsActive;
        foodItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Food item updated: {Name} (ID: {Id})", foodItem.Name, foodItem.FoodItemId);

        var response = new FoodItemResponse
        {
            FoodItemId = foodItem.FoodItemId,
            Name = foodItem.Name,
            Description = foodItem.Description,
            Price = foodItem.Price,
            Category = foodItem.Category,
            Theme = foodItem.Theme,
            ImageUrl = foodItem.ImageUrl,
            Ingredients = foodItem.Ingredients,
            NutritionalInfo = foodItem.NutritionalInfo,
            AllergenWarnings = foodItem.AllergenWarnings,
            IsActive = foodItem.IsActive,
            CreatedAt = foodItem.CreatedAt,
            UpdatedAt = foodItem.UpdatedAt
        };

        return Ok(response);
    }

    /// <summary>
    /// Delete a food item (Admin only, soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteFoodItem(int id)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);

        if (foodItem == null)
        {
            return NotFound(new { message = "Food item not found" });
        }

        // Soft delete - set IsActive to false
        foodItem.IsActive = false;
        foodItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Food item soft deleted: {Name} (ID: {Id})", foodItem.Name, foodItem.FoodItemId);

        return Ok(new { message = "Food item deleted successfully" });
    }

    /// <summary>
    /// Search food items with basic and advanced criteria
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FoodItemResponse>>> SearchFoodItems(
        [FromQuery] string? name = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? category = null,
        [FromQuery] string? description = null,
        [FromQuery] string? theme = null)
    {
        var query = _context.FoodItems.Where(f => f.IsActive);

        // Basic search by name (case-insensitive)
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(f => f.Name.ToLower().Contains(name.ToLower()));
        }

        // Advanced search criteria
        if (minPrice.HasValue)
        {
            query = query.Where(f => f.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(f => f.Price <= maxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(f => f.Category.ToLower() == category.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            query = query.Where(f => f.Description != null && f.Description.ToLower().Contains(description.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(theme))
        {
            query = query.Where(f => f.Theme != null && f.Theme.ToLower() == theme.ToLower());
        }

        var foodItems = await query
            .OrderBy(f => f.Category)
            .ThenBy(f => f.Name)
            .ToListAsync();

        var response = foodItems.Select(f => new FoodItemResponse
        {
            FoodItemId = f.FoodItemId,
            Name = f.Name,
            Description = f.Description,
            Price = f.Price,
            Category = f.Category,
            Theme = f.Theme,
            ImageUrl = f.ImageUrl,
            Ingredients = f.Ingredients,
            NutritionalInfo = f.NutritionalInfo,
            AllergenWarnings = f.AllergenWarnings,
            IsActive = f.IsActive,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt
        });

        return Ok(response);
    }

    /// <summary>
    /// Get all available categories
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var categories = await _context.FoodItems
            .Where(f => f.IsActive)
            .Select(f => f.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        return Ok(categories);
    }
}
