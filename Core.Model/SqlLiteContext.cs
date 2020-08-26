using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class SqlLiteContext: DbContext
    {
        /// <summary>
        /// 去重表
        /// </summary>
        public DbSet<RepeatingTable> RepeatingTables { get; set; }

        /// <summary>
        /// 位置记录
        /// </summary>
        public DbSet<LocationRecords> LocationRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Physical.db");
        }
    }
}
