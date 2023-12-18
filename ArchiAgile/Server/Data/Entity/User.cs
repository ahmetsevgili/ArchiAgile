namespace ArchiAgile.Server.Data.Entity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int UserRoleID { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }
    }
}
