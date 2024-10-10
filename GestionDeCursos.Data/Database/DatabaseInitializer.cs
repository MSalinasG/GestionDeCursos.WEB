using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GestionDeCursos.Data.Database
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = 
                new ApplicationDbContext
                (serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                CreateUsers(serviceProvider, context);
            }
        }

        public static void CreateUsers(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            var passwordHasher = serviceProvider.GetService<IPasswordHasher<AppUser>>();
            var hashedPassword = passwordHasher.HashPassword(new AppUser(), "password");

            var adminRole = context.Roles.FirstOrDefault(x => x.Name == GlobalHelper.Role.Administrator);
            var studentRole = context.Roles.FirstOrDefault(x => x.Name == GlobalHelper.Role.Student);
            var instructorRole = context.Roles.FirstOrDefault(x => x.Name == GlobalHelper.Role.Instructor);

            if (adminRole == null)
            {
                context.Roles.Add(new AppRole
                {
                    Name = GlobalHelper.Role.Administrator
                });
            }
            if (studentRole == null)
            {
                context.Roles.Add(new AppRole
                {
                    Name = GlobalHelper.Role.Student
                });
            }
            if (instructorRole == null)
            {
                context.Roles.Add(new AppRole
                {
                    Name = GlobalHelper.Role.Instructor
                });
            }

            bool hasUsers = context.Users.Any();

            if (!hasUsers)
            {
                var usersToCreate = new List<AppUser>
                {
                    new AppUser
                    {
                        Username = "admin1",
                        IsActive = true,
                        Role = adminRole,
                        Password = hashedPassword
                    },
                    new AppUser
                    {
                        Username = "student1",
                        IsActive = true,
                        Role = studentRole,
                        Password = hashedPassword
                    },
                    new AppUser
                    {
                        Username = "instructor1",
                        IsActive = true,
                        Role = instructorRole,
                        Password = hashedPassword
                    },
                };

                context.Users.AddRange(usersToCreate);
            }

            context.SaveChanges();
        }
    }
}
