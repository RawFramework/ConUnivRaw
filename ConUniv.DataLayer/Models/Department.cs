using System;
using Needletail.DataAccess.Attributes;
using DataAccess.Scaffold.Attributes;

namespace ConUniv.Models
{
    public class Department
    {
        
        [Required][TableKey(CanInsertKey = true)]
        public Guid Id { get; set; }
        
        [MaxLen(50)]
        public string Name { get; set; }
        
        [Required]
        public Decimal Budget { get; set; }
        
        
        public DateTime StartDate { get; set; }
        
        
        public Guid? InstructorID { get; set; }
        
    }
}
