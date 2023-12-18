using ArchiAgile.Server.Data.Entity;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Parameter = ArchiAgile.Server.Data.Entity.Parameter;

namespace ArchiAgile.Server.Data
{
    public class ApplicationDBContext : DbContext, IDataProtectionKeyContext
    {
        private int _userID;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _userID = _httpContextAccessor.HttpContext == null ? 0 :
                (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value) ?
                int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value) : 0);
        }
        public override int SaveChanges()
        {
            var dt = DateTime.Now;
            var addedEntityList = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(t => t.Entity).ToList();

            foreach (var item in addedEntityList)
            {
                if (item is BaseEntity baseEntity)
                {
                    baseEntity.InsertDateTime = dt;
                    baseEntity.InsertUserId = _userID;
                }
            }

            var modifiedEntityList = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(t => t.Entity).ToList();

            foreach (var item in modifiedEntityList)
            {
                if (item is BaseEntity baseEntity)
                {
                    baseEntity.UpdateDateTime = dt;
                    baseEntity.UpdateUserId = _userID;
                }
            }

            return base.SaveChanges();
        }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<User> User { get; set; }        
        public DbSet<Parameter> Parameter { get; set; }        
        public DbSet<Personnel> Personnel { get; set; }        
        public DbSet<PersonnelRole> PersonnelRole { get; set; }        
        public DbSet<Journal> Journal { get; set; }        
    }
}
