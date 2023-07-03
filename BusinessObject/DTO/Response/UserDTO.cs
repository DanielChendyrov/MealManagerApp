using AutoMapper.Configuration.Conventions;
using DataAccessLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.Response
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public int DepID { get; set; }
        public virtual Department Dep { get; set; } = null!;
        public int CompRoleID { get; set; }
        public virtual CompanyRole CompRole { get; set; } = null!;
        public int SysRoleID { get; set; }
        public virtual SystemRole SysRole { get; set; } = null!;
    }
}
