namespace ArchiAgile.Server.Data.Entity
{
    public class Personnel : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PersonnelRoleID { get; set; }
        public bool IsActive { get; set; }
    }
}
