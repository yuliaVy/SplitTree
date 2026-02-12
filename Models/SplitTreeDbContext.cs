using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SplitTree.Models
{
    public class SplitTreeDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public SplitTreeDbContext()
            : base("aspnet-SplitTreeV1", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public static SplitTreeDbContext Create()
        {
            return new SplitTreeDbContext();
        }
    }
}