using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shop.Entity.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Shop.Entity
{
    public class SmsDBContext : 
        IdentityDbContext<SmsUser, SmsRole, long, SmsUserClaim, SmsUserRole, SmsUserLogin, SmsRoleClaim, SmsUserToken>
    {
        public SmsDBContext(DbContextOptions<SmsDBContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SmsUser>().ToTable("SmsUser");
            builder.Entity<SmsRole>().ToTable("SmsRole");
            builder.Entity<SmsUserRole>().ToTable("SmsUserRole");
            builder.Entity<SmsUserClaim>().ToTable("SmsUserClaim");
            builder.Entity<SmsRoleClaim>().ToTable("SmsRoleClaim");
            builder.Entity<SmsUserLogin>().ToTable("SmsUserLogin");
            builder.Entity<SmsUserToken>().ToTable("SmsUserToken");

            builder.Entity<SmsUser>(build => {
                build.Property(m => m.NickName).HasMaxLength(100);
            });

            builder.Entity<SmsCategory>(action => {
                action.HasKey(m => m.CategoryId);
                action.Property(m => m.ParentPath).HasMaxLength(50);
                action.Property(m => m.CategoryName).HasMaxLength(50);
                action.HasData(new List<SmsCategory> {
                   new SmsCategory{ CategoryId = 1, CategoryName = "家用电器", Depth = 0, ParentId = 0, ParentPath = "0" },
                   new SmsCategory{ CategoryId = 2, CategoryName = "床上服务器", Depth = 0, ParentId = 0, ParentPath = "0" }
                });
            });

            builder.Entity<SmsGoods>(action => {
                //主键
                action.HasKey(m => m.GoodsId);
                action.Property(m => m.GoodsId).HasColumnType("bigint");
                action.Property(m => m.GoodsName).HasMaxLength(50).IsRequired();
                action.Property(m => m.GoodsPrice).HasColumnType("money");
                action.Property(m => m.GoodsPic).HasMaxLength(500);
                action.Property(m => m.AddTime).HasDefaultValueSql("getdate()");
                //外键
                action.HasOne<SmsCategory>(m => m.SmsCategory).WithMany(m => m.SmsGoods).HasForeignKey(m => m.CategoryId);
            });

            builder.Entity<SmsSysMenu>(option => {
                option.HasKey(m => m.MenuId);
                option.Property(m => m.MenuName).HasMaxLength(50).IsRequired();
                option.Property(m => m.ParentId).IsRequired();
                option.Property(m => m.ParentPath).HasMaxLength(50).IsRequired();
            });
        }

        public DbSet<SmsCategory> SmsCategory { get; set; }
        public DbSet<SmsGoods> SmsGoods { get; set; }
    }
}
