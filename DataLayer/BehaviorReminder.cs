//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class BehaviorReminder
    {
        public int BehaviourReminderId { get; set; }
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public int BehaviourCalcId { get; set; }
        public Nullable<bool> DismissStatus { get; set; }
    }
}