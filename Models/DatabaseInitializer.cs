using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace SplitTree.Models
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<SplitTreeDbContext>
    {
        protected override void Seed(SplitTreeDbContext context)
        {
            base.Seed(context);
            if (!context.Users.Any())
            {
                //create a few roles and store them in AspNetRoles table

                //create a roleManeger object will allow us to create roles and store them in the database
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if the Admin role doesn't exist
                if (!roleManager.RoleExists("Admin"))
                {
                    //create an Admin role
                    roleManager.Create(new IdentityRole("Admin"));
                }

                //if the Member role doesn't exist
                if (!roleManager.RoleExists("Member"))
                {
                    //create a Member role
                    roleManager.Create(new IdentityRole("Member"));
                }
                //save new roles to the database
                context.SaveChanges();


                //********************************************************
                //create some users now and assign them to different toles
                //********************************************************

                //the userManager object allows creating users and store them in the database
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                //if users with admin@splittree.com username doesn't exist then ->)
                if (userManager.FindByName("admin@splittree.com")==null)
                {
                    //super relaxed password validator
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };

                    //create a user Admin
                    var admin = new User()
                    {
                        UserName = "admin@splittree.com",
                        Email = "admin@splittree.com",
                        FirstName = "Jim",
                        LastName = "Smith",
                        Street = "56 High Street",
                        City = "Glasgow",
                        PostCode = "G1 67AD",
                        EmailConfirmed = true,
                        PhoneNumber = "07799112233"
                    };

                    //add the hashed password to user
                    userManager.Create(admin, "admin123");

                    //add the user to the role Admin
                    userManager.AddToRole(admin.Id, "Admin");

                    //create a few members
                    var member1 = new User()
                    {
                        UserName = "member1@splittree.com",
                        Email = "member1@splittree.com",
                        FirstName = "Paul",
                        LastName = "Goat",
                        Street = "5 Meerry Street",
                        City = "Glasgow",
                        PostCode = "G5 7AD",
                        EmailConfirmed = true,
                        PhoneNumber = "02244772233"
                    };

                    if (userManager.FindByName("member1@splittree.com") == null)
                    {
                        userManager.Create(member1, "password1");
                        userManager.AddToRole(member1.Id, "Member");
                    }

                    var member2 = new User()
                    {
                        UserName = "member2@splittree.com",
                        Email = "member2@splittree.com",
                        FirstName = "Luigi",
                        LastName = "Monk",
                        Street = "34 Confused Street",
                        City = "Edinburg",
                        PostCode = "8P9 7Y8",
                        EmailConfirmed = true,
                        PhoneNumber = "05588112233"
                    };

                    if (userManager.FindByName("member2@splittree.com") == null)
                    {
                        userManager.Create(member2, "password1");
                        userManager.AddToRole(member2.Id, "Member");
                    }
                    //save changes to the database
                    context.SaveChanges();

                    //**********************************
                    //seeding the Categories table
                    //**********************************

                    //creating a few categories
                    var cat1 = new Category() { Name = "Motors" };
                    var cat2 = new Category() { Name = "Property" };
                    var cat3 = new Category() { Name = "Jobs" };
                    var cat4 = new Category() { Name = "Services" };
                    var cat5 = new Category() { Name = "Pets" };
                    var cat6 = new Category() { Name = "For Sale" };

                    //add each category to the Categories table
                    context.Categories.Add(cat1);
                    context.Categories.Add(cat2);
                    context.Categories.Add(cat3);
                    context.Categories.Add(cat4);
                    context.Categories.Add(cat5);
                    context.Categories.Add(cat6);

                    //save changes to the database
                    context.SaveChanges();

                    //*******************************
                    //seeding the post table
                    //*******************************

                    var post1 = new Post()
                    {
                        Title = "House for sale",
                        Description = "Beautiful 5 bedrom detached hose",
                        Location = "Glasgow",
                        Price = 1000000m,
                        DatePosted = new DateTime(2019, 1, 1, 9, 0, 15),
                        DateExpired = new DateTime(2019, 1, 1, 9, 0, 15).AddDays(14),
                        User = member2,
                        Category = cat2
                    };

                    context.Posts.Add(post1);
                    var post2 = new Post()
                    {
                        Title = "Hyunday Tucson",
                        Description = "Beautiful 2016 Hyunday 5Dr",
                        Location = "Edinburgh",
                        Price = 14000m,
                        DatePosted = new DateTime(2019, 5, 25, 8, 0, 15),
                        DateExpired = new DateTime(2019, 5, 25, 8, 0, 15).AddDays(14),
                        User = member2,
                        Category = cat1
                    };   
                    context.Posts.Add(post2);

                    var post3 = new Post()
                    {
                        Title = "Audi Q5",
                        Description = "Beautiful 2019 Audi Q5",
                        Location = "Aberdeen",
                        Price = 56000m,
                        DatePosted = new DateTime(2019, 1, 25, 6, 0, 15),
                        DateExpired = new DateTime(2019, 1, 25, 6, 0, 15).AddDays(14),
                        User = member1,
                        Category = cat1
                    };
                    context.Posts.Add(post3);

                    var post4 = new Post()
                    {
                        Title = "Lhasso Apso",
                        Description = "Beautiful 2 years old Lhasso Apso",
                        Location = "Galsgow",
                        Price = 500m,
                        DatePosted = new DateTime(2019, 3, 5, 8, 0, 15),
                        DateExpired = new DateTime(2019, 3, 5, 8, 0, 15).AddDays(14),
                        User = member2,
                        Category = cat5
                    };
                    context.Posts.Add(post4);

                    var post5 = new Post()
                    {
                        Title = "Mercedes Benz A180",
                        Description = "Beautiful 2018 Mercedes Benz class A180",
                        Location = "Edinburgh",
                        Price = 34000m,
                        DatePosted = new DateTime(2019, 4, 5, 5, 0, 15),
                        DateExpired = new DateTime(2019, 4, 5, 5, 0, 15).AddDays(14),
                        User = member2,
                        Category = cat1
                    };
                    context.Posts.Add(post5);

                    var post6 = new Post()
                    {
                        Title = "Hyunday Tucson",
                        Description = "Beautiful 2017 Hyunday 5Dr",
                        Location = "Edinburgh",
                        Price = 14000m,
                        DatePosted = new DateTime(2018, 5, 25, 8, 0, 15),
                        DateExpired = new DateTime(2018, 5, 25, 8, 0, 15).AddDays(14),
                        User = member2,
                        Category = cat1
                    };
                    context.Posts.Add(post6);

                    //save the changes to the database
                    context.SaveChanges();

                }
            }
        }
    }
}