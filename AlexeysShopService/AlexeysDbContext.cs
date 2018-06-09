using AlexeysShop;
using AlexeysShopModel;
using System;
using System.Data.Entity;

namespace AlexeysShopService
{
    public class AlexeysDbContext : DbContext
    {
        public AlexeysDbContext() : base("AlexeysDatabase")
        {
            //настройки конфигурации для entity
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

        public virtual DbSet<MessageInfo> MessageInfos { get; set; }

        /// <summary>
        /// Перегружаем метод созранения изменений. Если возникла ошибка - очищаем все изменения
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }
    }
}
