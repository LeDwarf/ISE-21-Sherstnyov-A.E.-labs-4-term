using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.ViewModels
{
    public class StorageViewModel
    {
        public int Id { get; set; }
        public string StorageName { get; set; }
        public List<StoragePartViewModel> StorageParts { get; set; }
    }
}
