using CloudBerryJwtWebAPI.Model;

namespace CloudBerryJwtWebAPI.Operation
{
    public partial class DataLayer : IDataLayer
    {
        public string UserCreateQuery(UserModel users)
        {
            var query = "INSERT INTO Users(FirstName,LastName,Email,Phone) Values('" + users.FirstName + "','" + users.LastName + "','" + users.Email + "','" + users.Phone + "')";
            return query;
        }
        public string UserUpdateQuery(UserModel users)
        {
            var query = "UPDATE Users SET FirstName='" + users.FirstName + "',LastName='" + users.LastName + "',Email='" + users.Email + "',Phone='" + users.Phone + "' WHERE Id=" + users.Id + "";
            return query;
        }
        public string UserDeleteQuery(int id)
        {
            var query = "DELETE FROM Users  WHERE Id=" + id + "";
            return query;
        }
        public string AdminListQuery()
        {
            return "Select * from Admin";
        }
        public string AdminCheckQuery(AdminModel requestModel)
        {
            return "Select * from Admin where Email='" + requestModel.Email + "' and Password='" + requestModel.Password + "'";
        }

    }
}
