using System;
using System.Linq;
using System.Text;

namespace KittyHelper.Options
{
    public abstract class ViewOptions
    {
    }

    public abstract class CreateOptions
    {
        private readonly CreateOptionsAuthenticationOptions _authenticate;

        protected CreateOptions(Type t, string baseType, CreateOptionsAuthenticationOptions authenticate = null)
        {
            _authenticate = authenticate;
            BaseType = baseType;
            ReturnType = $"{BaseType}Response";
            RequestType = $"{BaseType}Request";
            ServiceType = $"{BaseType}Service";
            var an = new StringBuilder();
            if (_authenticate is {Authenticate: true}) an.AppendLine("[Authenticate]");

            if (_authenticate is {AllowedRoles: { }})
                an.AppendLine($"[RequiredRoles({FormatRoles(_authenticate.AllowedRoles)}]");


            Annotations = an.ToString();
        }

        public string UserIdField { get; set; } = "UserId";

        public string HttpVerb { get; set; } = "Post";
        public string ReturnType { get; set; }
        public string RequestType { get; set; }
        public string RequestObjectName { get; set; } = "request";

        public string Annotations { get; set; } = "";
        public string ServiceType { get; set; }
        protected string BaseType { get; set; }


        public string GenerateAuthIfNeeded()
        {
            return _authenticate is not {CheckUserOwnerShip: true}
                ? ""
                : $"a.{_authenticate.UserIdField} == {_authenticate.UserIdVariable} && ";
        }

        public string GenerateAssignToUser()
        {
            return _authenticate is not {CheckUserOwnerShip: true}
                ? ""
                : $"{RequestObjectName}.{UserIdField} = {_authenticate.UserIdVariable}.Id";
        }

        public string GenerateUserLookUp()
        {
            return _authenticate is not {CheckUserOwnerShip: true}
                ? ""
                : @$"var customUserSession = SessionAs<{_authenticate.SessionType}>();
 
                var {_authenticate.UserIdVariable} = ({_authenticate.UserType}) AuthRepository.GetUserAuth(customUserSession.UserAuthId);";
        }

        protected static string FormatRoles(string[] requiredRoles)
        {
            return string.Join(",", requiredRoles.Select(a => '"' + a + '"'));
        }
    }
}