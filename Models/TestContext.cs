using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class TestContext : IdentityDbContext<User>
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public TestContext(DbContextOptions<TestContext> options) : base (options)
        {

        }
    }
}
