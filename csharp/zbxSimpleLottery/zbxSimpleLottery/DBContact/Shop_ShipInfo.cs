//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBContact
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shop_ShipInfo
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string RegionCode { get; set; }
        public string Address { get; set; }
        public string LinkMan { get; set; }
        public string LinkManTel { get; set; }
        public string Mobile { get; set; }
        public string ZipCode { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<int> CreateUser { get; set; }
    }
}