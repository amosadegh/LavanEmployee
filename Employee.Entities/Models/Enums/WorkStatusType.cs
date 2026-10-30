using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Enums
{
    public enum WorkStatusType
    {
        /// <summary>کار در خشکی</summary>
        OnshoreWork = 0,

        /// <summary>کار در سکو</summary>
        OffshoreWork = 1,

        /// <summary>آماده به کار</summary>
        Standby = 2,

        /// <summary>استراحت</summary>
        Rest = 3,

        /// <summary>مرخصی</summary>
        Leave = 4,

        /// <summary>بیماری</summary>
        Sick = 5,

        /// <summary>غیبت</summary>
        Absent = 6,

        /// <summary>ماموریت</summary>
        Mission = 7
    }
}
