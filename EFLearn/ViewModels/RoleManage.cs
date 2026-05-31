using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFLearn.ViewModels
{
    public class RoleManage
    {
        public List<SelectListItem> RolesList { get; set; }
        public List<string> UserRoles { get; set; }
        public string UserEmail { get; set; }

    }
}
