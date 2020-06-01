using System;
using System.Collections.Generic;
using System.Text;
using GradesTrackerLib;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RelationshipTrackerLib;

namespace FrontendApp.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<LetterGradeScale> LetterGradeScales { get; set; }
    }
}
