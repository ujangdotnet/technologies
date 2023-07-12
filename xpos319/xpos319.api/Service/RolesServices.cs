using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.api.Service
{
    public class RolesServices
    {
        private readonly XPOS_319Context db;
        private VMResponse respon = new VMResponse();

        public RolesServices(XPOS_319Context _db)
        {
            this.db = _db;
        }

        public async Task<List<VMMenuAccess>> GetMenuAccessParentChildByRoleID(int IdRole, int MenuParent, bool OnlySelected = false)
        {
            List<VMMenuAccess> result = new List<VMMenuAccess>();
            List<TblMenu> data = db.TblMenus.Where(a => a.MenuParent == MenuParent && a.IsDelete == false).ToList();

            foreach (TblMenu item in data)
            {
                VMMenuAccess list = new VMMenuAccess();
                list.IdMenu = item.Id;
                list.MenuName = item.MenuName;
                list.IsParent = item.IsParent;
                list.MenuParent = item.MenuParent;
                list.is_selected = db.TblMenuAccesses.Where(a => a.IdRole == IdRole && a.MenuId == item.Id && a.IsDelete == false);
                list.ListChild = await GetMenuAccessParentChildByRoleID();

            }
        }
    }
}
