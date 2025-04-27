using System.ComponentModel.DataAnnotations;

namespace RestAPIDemo1
{
    public class UserInfo
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string PeopleId { get; set; }
    }
}
