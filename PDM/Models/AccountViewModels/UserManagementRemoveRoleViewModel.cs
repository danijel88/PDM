using System.Collections.Generic;

namespace PDM.Models.AccountViewModels
{
    public class UserManagementRemoveRoleViewModel
    {
        public IList<string> Roles { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string RoleId { get; set; }
    }
}
