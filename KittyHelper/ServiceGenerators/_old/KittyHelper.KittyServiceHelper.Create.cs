using System;
using System.Text;
using KittyHelper.Options;
using KittyHelper.ServiceGenerators.CS;
using static KittyHelper.KittyHelper.KittyViewHelper;
using static KittyHelper.ServiceGenerators.KittyServiceHelper;

namespace KittyHelper.ServiceGenerators
{
   


    public static partial class KittyServiceHelper
    {
       

  
     
        /// <summary>
        ///     I keep the option types by the functions that use them. It really helps clarity.
        /// </summary>
        public class CreateCreateEndPointOptions<A> : CreateOptions<A>
        {
            public CreateCreateEndPointOptions(string baseNameSpace, string baseType = null,
                CreateOptionsAuthenticationOptions authenticate = null,
                string[] requiredRoles = null) : base( baseType ?? "Create" + typeof(A).Name, baseNameSpace,authenticate)
            {
                var t = typeof(A);
                RequestObjectNewObjectField = t.Name;
                VueRouterDirectory = t.Name;
            }


            public string RequestObjectNewObjectField { get; set; }
         
            
        }
        public class GenerateEndPointAuthHelper<T>
        {
            private CreateOptions<T> options;

            public GenerateEndPointAuthHelper(CreateOptions<T> options)
            {
                this.options = options;
            }

            public string GenerateAuthIfNeeded()
            {
                return options.Authenticate is not {CheckUserOwnerShip: true}
                    ? ""
                    : $"a.{options.Authenticate.UserIdField} == {options.Authenticate.UserIdVariable} && ";
            }

            public string GenerateAssignToUser()
            {
                return options.Authenticate is not {CheckUserOwnerShip: true}
                    ? ""
                    : $"{options.Authenticate}.{options.UserIdField} = {options.Authenticate.UserIdVariable}.Id";
            }

            public string GenerateUserLookUp()
            {
                return options.Authenticate is not {CheckUserOwnerShip: true}
                    ? ""
                    : @$"var customUserSession = SessionAs<{options.Authenticate.SessionType}>();
 
                var {options.Authenticate.UserIdVariable} = ({options.Authenticate.UserType}) AuthRepository.GetUserAuth(customUserSession.UserAuthId);";
            }
        }
    }
}