using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject.Attributes;
using CourseProject.Models.DataModels;

namespace CourseProject.Models.DataModels
{

    [TableName("DeliveryType")]
    public class DeliveryType : Entity
    {
        public string Name { get; set; }

    }
}

