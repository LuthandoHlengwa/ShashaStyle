using Microsoft.EntityFrameworkCore;
using ShashaStyle.Models;

namespace ShashaStyle.Data
{
    public static class StoreSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            // ==========================================
            // CATEGORIES
            // ==========================================

            var categoryNames = new[]
            {
                "T-Shirts",
                "Hoodies",
                "Jackets",
                "Cargo Pants",
                "Jeans",
                "Tracksuits",
                "Caps",
                "Accessories"
            };

            foreach (var categoryName in categoryNames)
            {
                if (!await context.Categories
                    .AnyAsync(c => c.Name == categoryName))
                {
                    context.Categories.Add(new Category
                    {
                        Name = categoryName,
                        IsActive = true
                    });
                }
            }

            await context.SaveChangesAsync();

            // ==========================================
            // CATEGORIES
            // ==========================================

            var tShirts = await context.Categories
                .FirstAsync(c => c.Name == "T-Shirts");

            var hoodies = await context.Categories
                .FirstAsync(c => c.Name == "Hoodies");

            var jackets = await context.Categories
                .FirstAsync(c => c.Name == "Jackets");

            var cargoPants = await context.Categories
                .FirstAsync(c => c.Name == "Cargo Pants");

            var jeans = await context.Categories
                .FirstAsync(c => c.Name == "Jeans");

            var tracksuits = await context.Categories
                .FirstAsync(c => c.Name == "Tracksuits");

            var caps = await context.Categories
                .FirstAsync(c => c.Name == "Caps");

            var accessories = await context.Categories
                .FirstAsync(c => c.Name == "Accessories");

            // ==========================================
            // PRODUCTS
            // ==========================================

            if (!await context.Products.AnyAsync())
            {
                var products = new List<Product>
                {
                    // T-SHIRTS
                    new Product
                    {
                        Name = "Shasha Signature Tee",
                        Description = "Premium ShashaStyle signature streetwear T-shirt.",
                        Price = 399,
                        CategoryId = tShirts.CategoryId,
                        ImageUrl = "/images/products/signature-tee.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Identity Graphic Tee",
                        Description = "Bold graphic streetwear T-shirt designed for everyday wear.",
                        Price = 449,
                        CategoryId = tShirts.CategoryId,
                        ImageUrl = "/images/products/identity-graphic-tee.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Essential Oversized Tee",
                        Description = "Relaxed oversized fit with a clean ShashaStyle finish.",
                        Price = 429,
                        CategoryId = tShirts.CategoryId,
                        ImageUrl = "/images/products/essential-oversized-tee.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // HOODIES
                    new Product
                    {
                        Name = "Shasha Core Hoodie",
                        Description = "Heavyweight premium hoodie built for everyday streetwear.",
                        Price = 799,
                        CategoryId = hoodies.CategoryId,
                        ImageUrl = "/images/products/shasha-core-hoodie.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Essential Pullover Hoodie",
                        Description = "Clean everyday pullover hoodie with a relaxed streetwear fit.",
                        Price = 749,
                        CategoryId = hoodies.CategoryId,
                        ImageUrl = "/images/products/essential-pullover-hoodie.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // JACKETS
                    new Product
                    {
                        Name = "Shasha Varsity Jacket",
                        Description = "Premium varsity-inspired streetwear jacket.",
                        Price = 1199,
                        CategoryId = jackets.CategoryId,
                        ImageUrl = "/images/products/shasha-varsity-jacket.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Urban Utility Jacket",
                        Description = "Functional utility jacket inspired by modern urban fashion.",
                        Price = 1099,
                        CategoryId = jackets.CategoryId,
                        ImageUrl = "/images/products/urban-utility-jacket.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // CARGO PANTS
                    new Product
                    {
                        Name = "Relaxed Cargo Pants",
                        Description = "Relaxed cargo pants with multiple utility pockets.",
                        Price = 699,
                        CategoryId = cargoPants.CategoryId,
                        ImageUrl = "/images/products/relaxed-cargo-pants.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Street Utility Cargo",
                        Description = "Modern utility cargo pants designed for everyday streetwear.",
                        Price = 749,
                        CategoryId = cargoPants.CategoryId,
                        ImageUrl = "/images/products/street-utility-cargo.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // JEANS
                    new Product
                    {
                        Name = "Classic Straight Denim",
                        Description = "Classic straight-leg denim with a timeless streetwear fit.",
                        Price = 799,
                        CategoryId = jeans.CategoryId,
                        ImageUrl = "/images/products/classic-straight-denim.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Baggy Street Denim",
                        Description = "Relaxed baggy denim designed for modern streetwear styling.",
                        Price = 849,
                        CategoryId = jeans.CategoryId,
                        ImageUrl = "/images/products/baggy-street-denim.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // TRACKSUITS
                    new Product
                    {
                        Name = "Shasha Essential Tracksuit",
                        Description = "Complete premium tracksuit for everyday streetwear.",
                        Price = 1299,
                        CategoryId = tracksuits.CategoryId,
                        ImageUrl = "/images/products/shasha-essential-tracksuit.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Premium Street Tracksuit",
                        Description = "Premium relaxed-fit tracksuit with a modern urban silhouette.",
                        Price = 1499,
                        CategoryId = tracksuits.CategoryId,
                        ImageUrl = "/images/products/premium-street-tracksuit.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // CAPS
                    new Product
                    {
                        Name = "Shasha Classic Cap",
                        Description = "Classic everyday ShashaStyle cap.",
                        Price = 299,
                        CategoryId = caps.CategoryId,
                        ImageUrl = "/images/products/shasha-classic-cap.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Signature Embroidered Cap",
                        Description = "Premium embroidered cap with a clean streetwear finish.",
                        Price = 349,
                        CategoryId = caps.CategoryId,
                        ImageUrl = "/images/products/signature-embroidered-cap.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    // ACCESSORIES
                    new Product
                    {
                        Name = "Shasha Crossbody Bag",
                        Description = "Compact everyday crossbody bag designed for urban movement.",
                        Price = 449,
                        CategoryId = accessories.CategoryId,
                        ImageUrl = "/images/products/shasha-crossbody-bag.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },

                    new Product
                    {
                        Name = "Shasha Everyday Socks",
                        Description = "Comfortable everyday socks with ShashaStyle branding.",
                        Price = 149,
                        CategoryId = accessories.CategoryId,
                        ImageUrl = "/images/products/shasha-everyday-socks.jpg",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Products.AddRange(products);

                await context.SaveChangesAsync();
            }

            // ==========================================
            // PRODUCT VARIANTS
            // ==========================================

            var allProducts = await context.Products
                .Include(p => p.Category)
                .ToListAsync();

            foreach (var product in allProducts)
            {
                string[] colours;
                string[] sizes;

                switch (product.Category?.Name)
                {
                    case "T-Shirts":

                        colours = new[]
                        {
                            "Black",
                            "White",
                            "Cream",
                            "Grey",
                            "Charcoal",
                            "Navy",
                            "Burgundy",
                            "Red",
                            "Olive",
                            "Forest Green",
                            "Royal Blue",
                            "Brown",
                            "Beige",
                            "Sand",
                            "Mustard",
                            "Pink"
                        };

                        sizes = new[]
                        {
                            "S",
                            "M",
                            "L",
                            "XL",
                            "XXL"
                        };

                        break;

                    case "Hoodies":

                        colours = new[]
                        {
                            "Black",
                            "White",
                            "Cream",
                            "Grey",
                            "Charcoal",
                            "Navy",
                            "Burgundy",
                            "Forest Green",
                            "Olive",
                            "Brown",
                            "Beige",
                            "Sand",
                            "Royal Blue"
                        };

                        sizes = new[]
                        {
                            "S",
                            "M",
                            "L",
                            "XL",
                            "XXL"
                        };

                        break;

                    case "Jackets":

                        colours = new[]
                        {
                            "Black",
                            "Cream",
                            "Grey",
                            "Charcoal",
                            "Navy",
                            "Olive",
                            "Burgundy",
                            "Brown",
                            "Beige",
                            "Royal Blue"
                        };

                        sizes = new[]
                        {
                            "S",
                            "M",
                            "L",
                            "XL",
                            "XXL"
                        };

                        break;

                    case "Cargo Pants":

                        colours = new[]
                        {
                            "Black",
                            "Washed Black",
                            "Khaki",
                            "Olive",
                            "Beige",
                            "Cream",
                            "Grey",
                            "Charcoal",
                            "Brown",
                            "Navy"
                        };

                        sizes = new[]
                        {
                            "28",
                            "30",
                            "32",
                            "34",
                            "36",
                            "38",
                            "40"
                        };

                        break;

                    case "Jeans":

                        colours = new[]
                        {
                            "Black",
                            "Washed Black",
                            "Dark Blue",
                            "Mid Blue",
                            "Light Blue",
                            "Grey",
                            "Charcoal",
                            "Cream"
                        };

                        sizes = new[]
                        {
                            "28",
                            "30",
                            "32",
                            "34",
                            "36",
                            "38",
                            "40"
                        };

                        break;

                    case "Tracksuits":

                        colours = new[]
                        {
                            "Black",
                            "White",
                            "Grey",
                            "Charcoal",
                            "Navy",
                            "Burgundy",
                            "Forest Green",
                            "Olive",
                            "Cream",
                            "Royal Blue"
                        };

                        sizes = new[]
                        {
                            "S",
                            "M",
                            "L",
                            "XL",
                            "XXL"
                        };

                        break;

                    case "Caps":

                        colours = new[]
                        {
                            "Black",
                            "White",
                            "Grey",
                            "Navy",
                            "Red",
                            "Burgundy",
                            "Forest Green",
                            "Olive",
                            "Cream",
                            "Beige",
                            "Royal Blue"
                        };

                        sizes = new[]
                        {
                            "One Size"
                        };

                        break;

                    case "Accessories":

                        colours = new[]
                        {
                            "Black",
                            "White",
                            "Grey",
                            "Cream",
                            "Beige",
                            "Brown",
                            "Navy",
                            "Olive",
                            "Burgundy"
                        };

                        sizes = new[]
                        {
                            "One Size"
                        };

                        break;

                    default:

                        colours = new[]
                        {
                            "Black"
                        };

                        sizes = new[]
                        {
                            "One Size"
                        };

                        break;
                }

                foreach (var colour in colours)
                {
                    foreach (var size in sizes)
                    {
                        bool variantExists =
                            await context.ProductVariants.AnyAsync(v =>
                                v.ProductId == product.ProductId &&
                                v.Size == size &&
                                v.Colour == colour);

                        if (variantExists)
                        {
                            continue;
                        }

                        string colourCode = new string(
                            colour
                                .Where(char.IsLetterOrDigit)
                                .ToArray())
                            .ToUpper();

                        string sizeCode = new string(
                            size
                                .Where(char.IsLetterOrDigit)
                                .ToArray())
                            .ToUpper();

                        string sku =
                            $"SS-{product.ProductId:D3}-{sizeCode}-{colourCode}";

                        context.ProductVariants.Add(
                            new ProductVariant
                            {
                                ProductId = product.ProductId,

                                Size = size,

                                Colour = colour,

                                SKU = sku,

                                StockQuantity = 20,

                                IsActive = true,

                                CreatedAt = DateTime.UtcNow
                            });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}