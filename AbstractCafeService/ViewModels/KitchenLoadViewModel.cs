using System;
using System.Collections.Generic;

namespace AbstractCafeService.ViewModels
{
    public class KitchensLoadViewModel
    {
        public string KitchenName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Dishs { get; set; }
    }
}
