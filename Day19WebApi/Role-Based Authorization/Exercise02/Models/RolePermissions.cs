namespace Exercise02.Models
{
    public class RolePermissions
    {

        public static readonly Dictionary<string, List<string>> permissions = new()
    {
        {
            Roles.Admin, new List<string>
            {

                Permissions.ProductsView  ,
                Permissions.ProductsCreate,
                Permissions.ProductsEdit,
                Permissions.ProductsDelete,
                Permissions.UsersView,
                Permissions.UsersManage,
                Permissions.ReportsView,
                Permissions.ReportsExport
            }
        },
        {
            Roles.Manager, new List<string>
            {

                Permissions.ProductsView,
                Permissions.ProductsCreate,
                Permissions.ProductsEdit,
                Permissions.ProductsDelete,
                Permissions.ReportsView
            }
        },
        {
            Roles.User, new List<string>
            {

                Permissions.ProductsView,
                Permissions.ReportsView
            }
        },
        {
            Roles.Guest, new List<string>
            {

                Permissions.ProductsView
            }
        }
    };
    }


}