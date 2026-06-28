using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Common
{
    public class BaseEntity<Tkey>
    {
        public Tkey Id { get; set; } = default!;

    }
}
