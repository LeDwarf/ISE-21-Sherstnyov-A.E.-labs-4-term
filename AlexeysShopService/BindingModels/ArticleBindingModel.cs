using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.BindingModels
{
    public class ArticleBindingModel
    {
        public int Id { get; set; }
        public string ArticleName { get; set; }
        public decimal Cost { get; set; }
        public List<ArticlePartBindingModel> ArticleParts { get; set; }

    }
}
