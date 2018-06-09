using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.BindingModels
{
    public class ArticlePartBindingModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int PartId { get; set; }
        public int Count { get; set; }
    }
}
