using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    public class UpdateActiveDaysAndPriceDTO
    {
        public int id { get; set; }

        public Nullable<Int32> activeDays { get; set; }

        public Nullable<Decimal> price { get; set; }
    }
}