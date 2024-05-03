using Postgrest.Attributes;
using Postgrest.Models;

namespace UWO_DailyCustodian.ViewModel
{
    [Table("user_emails")]
    public class UserEmail : BaseModel
    {
        [PrimaryKey("id")]
        public int Id {  get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("role")]
        public string Role { get; set; }

        public UserEmail() { }
        public UserEmail(string email, string role)
        {
            Email = email;
            Role = role;
        }
    }
}
