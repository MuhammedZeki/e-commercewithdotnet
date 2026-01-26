using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet_db.Models;


public class DataContext : IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Slider> Sliders { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Slider>().HasData(
            new List<Slider>()
            {
                new(){Id=1,Description="Lorem ipsum 1",Image="slider-1.jpeg",Index=0,IsActive=true,Title="Slider-1"},
                new(){Id=2,Description="Lorem ipsum 2",Image="slider-2.jpeg",Index=1,IsActive=true,Title="Slider-2"},
                new(){Id=3,Description="Lorem ipsum 3",Image="slider-3.jpeg",Index=2,IsActive=true,Title="Slider-3"},
            }
        );

        modelBuilder.Entity<Category>().HasData(
            new List<Category>()
            {
                new(){Id=1,CategoryName="Telefon",Url="telefon"},
                new(){Id=2,CategoryName="Elektronik",Url="elektronik"},
                new(){Id=3,CategoryName="Beyaz Eşya",Url="beyaz-esya"},
                new(){Id=4,CategoryName="Kozmetik",Url="kozmetik"},
                new(){Id=5,CategoryName="Kategori-1",Url="kategori-1"},
                new(){Id=6,CategoryName="Kategori-2",Url="kategori-2"},
                new(){Id=7,CategoryName="Kategori-3",Url="kategori-3"},
                new(){Id=8,CategoryName="Kategori-4",Url="kategori-4"},
                new(){Id=9,CategoryName="Kategori-5",Url="kategori-5"},
                new(){Id=10,CategoryName="Kategori-6",Url="kategori-6"},
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new List<Product>()
            {
                new() {
                    Id = 1,
                    Name = "Iphone 13",
                    Price = 10000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "1.jpeg",
                    IsHome = true,
                    CategoryId = 1
                },
                new () {
                    Id = 2,
                    Name = "Iphone 14",
                    Price = 20000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "2.jpeg",
                    IsHome = false,
                    CategoryId = 1
                },
                new () {
                    Id = 3,
                    Name = "Iphone 15",
                    Price = 30000,
                    IsActive = false,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "3.jpeg",
                    IsHome = true,
                    CategoryId = 2
                },
                new () {
                    Id = 4,
                    Name = "Iphone 16",
                    Price = 40000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "4.jpeg",
                    IsHome = true,
                    CategoryId = 2
                },
                new () {
                    Id = 5,
                    Name = "Iphone 17",
                    Price = 50000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "5.jpeg",
                    IsHome = true,
                    CategoryId = 2
                },
                new () {
                    Id = 6,
                    Name = "Iphone 17",
                    Price = 60000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "6.jpeg",
                    IsHome = false,
                    CategoryId = 3
                },
                new () {
                    Id = 7,
                    Name = "Iphone 17",
                    Price = 70000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "7.jpeg",
                    IsHome = false,
                    CategoryId = 4
                },
                new () {
                    Id = 8,
                    Name = "Iphone 17",
                    Price = 80000,
                    IsActive = true,
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae ullam dicta sapiente recusandae? Accusamus, suscipit consequuntur doloribus magnam corrupti sed obcaecati quas vel ipsam eos exercitationem modi deserunt alias voluptas?",
                    Img = "8.jpeg",
                    IsHome = false,
                    CategoryId = 4
                }
            }
        );
    }

}