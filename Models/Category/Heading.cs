using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Category
{
    public class Heading
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}