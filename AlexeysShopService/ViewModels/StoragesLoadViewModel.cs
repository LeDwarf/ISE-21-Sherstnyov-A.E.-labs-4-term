using System;
using System.Collections.Generic;

namespace AlexeysShopService.ViewModels
{
    public class StoragesLoadViewModel
    {
        public string StorageName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Parts { get; set; }
    }
}