using AlexeysShopModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AlexeysShopService
{
	[Table("AlexeysDatabase")]
	public class AlexeysDbContext : DbContext
	{
		public AlexeysDbContext()
		{
			Configuration.ProxyCreationEnabled = false;
			Configuration.LazyLoadingEnabled = false;
			var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
		}

		public virtual DbSet<Customer> Customers { get; set; }

		public virtual DbSet<Part> Parts { get; set; }

		public virtual DbSet<Builder> Builders { get; set; }

		public virtual DbSet<Contract> Contracts { get; set; }

		public virtual DbSet<Article> Articles { get; set; }

		public virtual DbSet<ArticlePart> ArticleParts { get; set; }

		public virtual DbSet<Storage> Storages { get; set; }

		public virtual DbSet<StoragePart> StorageParts { get; set; }
	}
}
