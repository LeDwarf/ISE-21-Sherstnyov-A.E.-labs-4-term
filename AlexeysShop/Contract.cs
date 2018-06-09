using AlexeysShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopModel
{
    public class Contract
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ArticleId { get; set; }
        public int? BuilderId { get; set; }
        public int Count { get; set; }
        public decimal Cost { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime? DateBuilt { get; set; }        

    }
}
