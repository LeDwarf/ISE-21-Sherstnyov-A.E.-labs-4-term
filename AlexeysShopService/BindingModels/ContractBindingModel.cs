using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.BindingModels
{
    public class ContractBindingModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ArticleId { get; set; }
        public int? BuilderId { get; set; }
        public int Count { get; set; }
        public decimal Cost { get; set; }

    }
}
