using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlexeysShopModel;

namespace AlexeysShopService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Part> Parts { get; set; }

        public List<Builder> Builders { get; set; }

        public List<Contract> Contracts { get; set; }

        public List<Article> Articles { get; set; }

        public List<ArticlePart> ArticleParts { get; set; }

        public List<Storage> Storages { get; set; }

        public List<StoragePart> StorageParts { get; set; }

        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Parts = new List<Part>();
            Builders = new List<Builder>();
            Contracts = new List<Contract>();
            Articles = new List<Article>();
            ArticleParts = new List<ArticlePart>();
            Storages = new List<Storage>();
            StorageParts = new List<StoragePart>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }

    }
}
