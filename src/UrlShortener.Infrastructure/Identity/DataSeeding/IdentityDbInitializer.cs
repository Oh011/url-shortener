using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Application.Common.Interfaces;
using Project.Infrastructure.Identity.Entities;
using Project.Infrastructure.Persistence.Context;

namespace Project.Infrastructure.Identity.DataSeeding
{
    internal class IdentityDbInitializer(ApplicationDbContext _dbContext,



     UserManager<ApplicationUser> _userManager




        ) : IdentityInitializer
    {













        public async Task InitializeAsync()
        {
            try
            {



                if (_dbContext.Database.GetPendingMigrations().Any())
                {

                    await _dbContext.Database.MigrateAsync();
                }



                if (!_userManager.Users.Any())
                {


                    var Admin = new ApplicationUser()
                    {

                        Email = "Admin@Gmail.com",

                        UserName = "AdminUser"


                    };




                    var result = await _userManager.CreateAsync(Admin, "Admin#123");

                    if (result.Succeeded)
                    {
                        Console.WriteLine("Yes");
                    }

                    await _userManager.AddToRoleAsync(Admin, "Admin");




                }


            }




            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

            }
        }
    }
}

