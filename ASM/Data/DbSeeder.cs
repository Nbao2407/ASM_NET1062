using ASM.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (await context.Users.AnyAsync() || await context.FoodItems.AnyAsync())
        {
            return; // Database has been seeded
        }

        // Seed Users
        var users = new List<User>
        {
            new User
            {
                Email = "admin@fastfood.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                FullName = "System Administrator",
                PhoneNumber = "555-0100",
                Address = "123 Admin Street",
                DateOfBirth = new DateTime(1985, 1, 1),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Email = "customer1@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer123!"),
                FullName = "John Doe",
                PhoneNumber = "555-0101",
                Address = "456 Customer Lane",
                DateOfBirth = new DateTime(1990, 5, 15),
                Role = "Customer",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Email = "customer2@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer123!"),
                FullName = "Jane Smith",
                PhoneNumber = "555-0102",
                Address = "789 Buyer Boulevard",
                DateOfBirth = new DateTime(1992, 8, 20),
                Role = "Customer",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        // Seed Food Items - Burgers
        var burgers = new List<FoodItem>
        {
            new FoodItem
            {
                Name = "Classic Burger",
                Description = "Juicy beef patty with lettuce, tomato, onion, and special sauce",
                Price = 8.99m,
                Category = "Burgers",
                Theme = "Classic",
                ImageUrl = "/images/classic-burger.jpg",
                Ingredients = "Beef patty, lettuce, tomato, onion, pickles, special sauce, sesame bun",
                NutritionalInfo = "Calories: 540, Protein: 28g, Carbs: 42g, Fat: 26g",
                AllergenWarnings = "Contains: Gluten, Dairy, Soy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Cheese Burger",
                Description = "Classic burger with melted cheddar cheese",
                Price = 9.99m,
                Category = "Burgers",
                Theme = "Classic",
                ImageUrl = "/images/cheese-burger.jpg",
                Ingredients = "Beef patty, cheddar cheese, lettuce, tomato, onion, pickles, ketchup, mustard, sesame bun",
                NutritionalInfo = "Calories: 610, Protein: 32g, Carbs: 43g, Fat: 31g",
                AllergenWarnings = "Contains: Gluten, Dairy, Soy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Bacon Burger",
                Description = "Loaded with crispy bacon and cheese",
                Price = 11.99m,
                Category = "Burgers",
                Theme = "Premium",
                ImageUrl = "/images/bacon-burger.jpg",
                Ingredients = "Beef patty, bacon, cheddar cheese, lettuce, tomato, onion, BBQ sauce, sesame bun",
                NutritionalInfo = "Calories: 720, Protein: 38g, Carbs: 44g, Fat: 40g",
                AllergenWarnings = "Contains: Gluten, Dairy, Soy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Veggie Burger",
                Description = "Plant-based patty with fresh vegetables",
                Price = 9.49m,
                Category = "Burgers",
                Theme = "Healthy",
                ImageUrl = "/images/veggie-burger.jpg",
                Ingredients = "Plant-based patty, lettuce, tomato, onion, avocado, vegan mayo, whole wheat bun",
                NutritionalInfo = "Calories: 420, Protein: 18g, Carbs: 48g, Fat: 18g",
                AllergenWarnings = "Contains: Gluten, Soy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.FoodItems.AddRangeAsync(burgers);
        await context.SaveChangesAsync();

        // Seed Food Items - Pizza
        var pizzas = new List<FoodItem>
        {
            new FoodItem
            {
                Name = "Margherita Pizza",
                Description = "Classic pizza with tomato sauce, mozzarella, and basil",
                Price = 12.99m,
                Category = "Pizza",
                Theme = "Classic",
                ImageUrl = "/images/margherita-pizza.jpg",
                Ingredients = "Pizza dough, tomato sauce, mozzarella cheese, fresh basil, olive oil",
                NutritionalInfo = "Calories: 680, Protein: 28g, Carbs: 82g, Fat: 24g",
                AllergenWarnings = "Contains: Gluten, Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Pepperoni Pizza",
                Description = "Loaded with pepperoni and extra cheese",
                Price = 14.99m,
                Category = "Pizza",
                Theme = "Classic",
                ImageUrl = "/images/pepperoni-pizza.jpg",
                Ingredients = "Pizza dough, tomato sauce, mozzarella cheese, pepperoni",
                NutritionalInfo = "Calories: 780, Protein: 34g, Carbs: 84g, Fat: 32g",
                AllergenWarnings = "Contains: Gluten, Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Supreme Pizza",
                Description = "Topped with pepperoni, sausage, peppers, onions, and mushrooms",
                Price = 16.99m,
                Category = "Pizza",
                Theme = "Premium",
                ImageUrl = "/images/supreme-pizza.jpg",
                Ingredients = "Pizza dough, tomato sauce, mozzarella, pepperoni, sausage, bell peppers, onions, mushrooms, olives",
                NutritionalInfo = "Calories: 880, Protein: 38g, Carbs: 88g, Fat: 38g",
                AllergenWarnings = "Contains: Gluten, Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.FoodItems.AddRangeAsync(pizzas);
        await context.SaveChangesAsync();

        // Seed Food Items - Chicken
        var chicken = new List<FoodItem>
        {
            new FoodItem
            {
                Name = "Crispy Chicken Tenders",
                Description = "Golden fried chicken tenders with dipping sauce",
                Price = 9.99m,
                Category = "Chicken",
                Theme = "Classic",
                ImageUrl = "/images/chicken-tenders.jpg",
                Ingredients = "Chicken breast, breading, vegetable oil",
                NutritionalInfo = "Calories: 520, Protein: 42g, Carbs: 38g, Fat: 20g",
                AllergenWarnings = "Contains: Gluten, Eggs",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Buffalo Wings",
                Description = "Spicy chicken wings with blue cheese dip",
                Price = 11.99m,
                Category = "Chicken",
                Theme = "Spicy",
                ImageUrl = "/images/buffalo-wings.jpg",
                Ingredients = "Chicken wings, buffalo sauce, butter, blue cheese dressing",
                NutritionalInfo = "Calories: 640, Protein: 48g, Carbs: 12g, Fat: 44g",
                AllergenWarnings = "Contains: Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Grilled Chicken Sandwich",
                Description = "Grilled chicken breast with lettuce and mayo",
                Price = 10.49m,
                Category = "Chicken",
                Theme = "Healthy",
                ImageUrl = "/images/grilled-chicken-sandwich.jpg",
                Ingredients = "Grilled chicken breast, lettuce, tomato, mayo, whole wheat bun",
                NutritionalInfo = "Calories: 420, Protein: 38g, Carbs: 36g, Fat: 14g",
                AllergenWarnings = "Contains: Gluten, Eggs",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.FoodItems.AddRangeAsync(chicken);
        await context.SaveChangesAsync();

        // Seed Food Items - Beverages
        var beverages = new List<FoodItem>
        {
            new FoodItem
            {
                Name = "Coca-Cola",
                Description = "Classic Coca-Cola soft drink",
                Price = 2.49m,
                Category = "Beverages",
                Theme = "Classic",
                ImageUrl = "/images/coca-cola.jpg",
                Ingredients = "Carbonated water, high fructose corn syrup, caramel color, phosphoric acid, natural flavors, caffeine",
                NutritionalInfo = "Calories: 140, Protein: 0g, Carbs: 39g, Fat: 0g",
                AllergenWarnings = "None",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Orange Juice",
                Description = "Freshly squeezed orange juice",
                Price = 3.49m,
                Category = "Beverages",
                Theme = "Healthy",
                ImageUrl = "/images/orange-juice.jpg",
                Ingredients = "100% pure orange juice",
                NutritionalInfo = "Calories: 110, Protein: 2g, Carbs: 26g, Fat: 0g",
                AllergenWarnings = "None",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Milkshake - Vanilla",
                Description = "Creamy vanilla milkshake",
                Price = 4.99m,
                Category = "Beverages",
                Theme = "Premium",
                ImageUrl = "/images/vanilla-milkshake.jpg",
                Ingredients = "Milk, vanilla ice cream, sugar, vanilla extract",
                NutritionalInfo = "Calories: 420, Protein: 10g, Carbs: 62g, Fat: 16g",
                AllergenWarnings = "Contains: Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Iced Coffee",
                Description = "Cold brew coffee over ice",
                Price = 3.99m,
                Category = "Beverages",
                Theme = "Classic",
                ImageUrl = "/images/iced-coffee.jpg",
                Ingredients = "Coffee, ice, milk, sugar",
                NutritionalInfo = "Calories: 80, Protein: 2g, Carbs: 14g, Fat: 2g",
                AllergenWarnings = "Contains: Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.FoodItems.AddRangeAsync(beverages);
        await context.SaveChangesAsync();

        // Seed Food Items - Desserts
        var desserts = new List<FoodItem>
        {
            new FoodItem
            {
                Name = "Chocolate Brownie",
                Description = "Rich chocolate brownie with fudge",
                Price = 4.49m,
                Category = "Desserts",
                Theme = "Classic",
                ImageUrl = "/images/chocolate-brownie.jpg",
                Ingredients = "Chocolate, butter, sugar, eggs, flour, cocoa powder",
                NutritionalInfo = "Calories: 380, Protein: 4g, Carbs: 48g, Fat: 20g",
                AllergenWarnings = "Contains: Gluten, Dairy, Eggs",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Apple Pie",
                Description = "Classic apple pie with cinnamon",
                Price = 5.49m,
                Category = "Desserts",
                Theme = "Classic",
                ImageUrl = "/images/apple-pie.jpg",
                Ingredients = "Apples, sugar, cinnamon, pie crust, butter",
                NutritionalInfo = "Calories: 320, Protein: 3g, Carbs: 52g, Fat: 12g",
                AllergenWarnings = "Contains: Gluten, Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new FoodItem
            {
                Name = "Ice Cream Sundae",
                Description = "Vanilla ice cream with chocolate sauce and whipped cream",
                Price = 4.99m,
                Category = "Desserts",
                Theme = "Premium",
                ImageUrl = "/images/ice-cream-sundae.jpg",
                Ingredients = "Vanilla ice cream, chocolate sauce, whipped cream, cherry",
                NutritionalInfo = "Calories: 450, Protein: 6g, Carbs: 58g, Fat: 22g",
                AllergenWarnings = "Contains: Dairy",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.FoodItems.AddRangeAsync(desserts);
        await context.SaveChangesAsync();

        // Seed Combos
        var combo1 = new Combo
        {
            Name = "Classic Burger Meal",
            Description = "Classic burger with fries and a drink",
            Price = 10.99m, // Sum: $11.48, Saves: $0.49
            ImageUrl = "/images/burger-meal.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var combo2 = new Combo
        {
            Name = "Family Pizza Deal",
            Description = "Large pepperoni pizza with 4 drinks",
            Price = 22.99m, // Sum: $24.95, Saves: $1.96
            ImageUrl = "/images/family-pizza-deal.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var combo3 = new Combo
        {
            Name = "Chicken Feast",
            Description = "Chicken tenders, wings, and 2 drinks",
            Price = 19.99m,
            ImageUrl = "/images/chicken-feast.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var combo4 = new Combo
        {
            Name = "Dessert Combo",
            Description = "Brownie, ice cream sundae, and 2 coffees",
            Price = 14.99m,
            ImageUrl = "/images/dessert-combo.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Combos.AddRangeAsync(new[] { combo1, combo2, combo3, combo4 });
        await context.SaveChangesAsync();

        // Get food items for combo associations
        var classicBurger = await context.FoodItems.FirstAsync(f => f.Name == "Classic Burger");
        var cocaCola = await context.FoodItems.FirstAsync(f => f.Name == "Coca-Cola");
        var pepperoniPizza = await context.FoodItems.FirstAsync(f => f.Name == "Pepperoni Pizza");
        var chickenTenders = await context.FoodItems.FirstAsync(f => f.Name == "Crispy Chicken Tenders");
        var buffaloWings = await context.FoodItems.FirstAsync(f => f.Name == "Buffalo Wings");
        var brownie = await context.FoodItems.FirstAsync(f => f.Name == "Chocolate Brownie");
        var iceCreamSundae = await context.FoodItems.FirstAsync(f => f.Name == "Ice Cream Sundae");
        var icedCoffee = await context.FoodItems.FirstAsync(f => f.Name == "Iced Coffee");

        // Seed Combo Items
        var comboItems = new List<ComboItem>
        {
            // Classic Burger Meal
            new ComboItem { ComboId = combo1.ComboId, FoodItemId = classicBurger.FoodItemId, Quantity = 1 },
            new ComboItem { ComboId = combo1.ComboId, FoodItemId = cocaCola.FoodItemId, Quantity = 1 },

            // Family Pizza Deal
            new ComboItem { ComboId = combo2.ComboId, FoodItemId = pepperoniPizza.FoodItemId, Quantity = 1 },
            new ComboItem { ComboId = combo2.ComboId, FoodItemId = cocaCola.FoodItemId, Quantity = 4 },

            // Chicken Feast
            new ComboItem { ComboId = combo3.ComboId, FoodItemId = chickenTenders.FoodItemId, Quantity = 1 },
            new ComboItem { ComboId = combo3.ComboId, FoodItemId = buffaloWings.FoodItemId, Quantity = 1 },
            new ComboItem { ComboId = combo3.ComboId, FoodItemId = cocaCola.FoodItemId, Quantity = 2 },

            // Dessert Combo
            new ComboItem { ComboId = combo4.ComboId, FoodItemId = brownie.FoodItemId, Quantity = 1 },
            new ComboItem { ComboId = combo4.ComboId, FoodItemId = iceCreamSundae.FoodItemId, Quantity = 1 },
            new ComboItem { ComboId = combo4.ComboId, FoodItemId = icedCoffee.FoodItemId, Quantity = 2 }
        };

        await context.ComboItems.AddRangeAsync(comboItems);
        await context.SaveChangesAsync();
    }
}
