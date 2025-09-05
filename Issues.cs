using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServicesApp
{
    class Issues
    {
        public int Id { get; set; }
        public string ReporterName { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Received"; // Default status

    }
}
