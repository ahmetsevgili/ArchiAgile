namespace ArchiAgile.Server.Data.Entity
{
    public class Journal : BaseEntity
    {
        public DateTime Date { get; set; }
        public int PersonnelID { get; set; }
        public string Note { get; set; }
        public int ConferPersonnelID { get; set; }
        public int ConferPersonnelRoleID { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
    }
}
