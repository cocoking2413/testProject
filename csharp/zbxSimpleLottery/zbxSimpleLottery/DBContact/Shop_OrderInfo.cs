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
    
    public partial class Shop_OrderInfo
    {
        public int ID { get; set; }
        public string OrderNO { get; set; }
        public Nullable<int> UserID { get; set; }
        public int OrderType { get; set; }
        public Nullable<int> LuckID { get; set; }
        public Nullable<int> ProductCount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> OrderState { get; set; }
        public Nullable<int> ShipID { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
