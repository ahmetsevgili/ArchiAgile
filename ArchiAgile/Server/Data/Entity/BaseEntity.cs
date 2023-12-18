using System.ComponentModel.DataAnnotations;

namespace ArchiAgile.Server.Data.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        public int RecordID { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateUserId { get; set; }
    }
}
