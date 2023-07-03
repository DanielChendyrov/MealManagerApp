using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.Request
{
    public class SignUpDTO
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int DepID { get; set; }
        public int CompRoleID { get; set; }
        public int SysRoleID { get; set; }
    }
}
