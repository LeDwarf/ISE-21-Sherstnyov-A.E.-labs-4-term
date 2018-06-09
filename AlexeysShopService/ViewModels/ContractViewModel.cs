using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.ViewModels
{
    public class ContractViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFIO { get; set; }
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public int? BuilderId { get; set; }
        public string BuilderName { get; set; }
        public int Count { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public string DateBegin { get; set; }
        public string DateBuilt { get; set; }

    }
}
