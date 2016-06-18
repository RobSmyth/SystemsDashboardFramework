using System.Collections.Generic;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteripUsers : TeamCitySharp.ActionTypes.IUsers
    {
        public bool AddPassword(string username, string password)
        {
            return false;
        }

        public List<User> All()
        {
            return new List<User>();
        }

        public List<Group> AllGroupsByUserName(string userName)
        {
            return new List<Group>();
        }

        public List<Role> AllRolesByUserName(string userName)
        {
            return new List<Role>();
        }

        public List<Group> AllUserGroups()
        {
            return new List<Group>();
        }

        public List<Role> AllUserRolesByUserGroup(string userGroupName)
        {
            return new List<Role>();
        }

        public List<User> AllUsersByUserGroup(string userGroupName)
        {
            return new List<User>();
        }

        public bool Create(string username, string name, string email, string password)
        {
            return false;
        }

        public User Details(string userName)
        {
            return null;
        }
    }
}