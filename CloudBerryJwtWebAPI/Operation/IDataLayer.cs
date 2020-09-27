using CloudBerryJwtWebAPI.Model;
using System.Collections.Generic;

namespace CloudBerryJwtWebAPI.Operation
{
    public interface IDataLayer
    {
        public List<AdminModel> AdminList();
        public KeyValuePair<bool, AdminModel> CheckAdmin(AdminModel requestModel);
        public KeyValuePair<bool, string> CreateUser(UserModel users);
        public KeyValuePair<bool, string> DeleteUser(int id);
        public KeyValuePair<bool, string> UpdateUser(UserModel users);
    }
}
