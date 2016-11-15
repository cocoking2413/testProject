using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace sqlGenerateTest.Contact {
	public class DBContext : baseDBContext {
		public DBContext() : base("data source=115.28.179.103;initial catalog=zms3.02_develop;user id=sa_zms302_develop;password=zhiboxing_zms302_develop_123") {
		}
		public DbSet<oa_ApproveFlow> Flows { get; set; }
		public DbSet<oa_ApproveInfo> Infos { get; set; }
	}
}
