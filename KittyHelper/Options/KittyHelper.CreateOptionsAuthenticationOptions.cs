namespace KittyHelper.Options
{
    
        public class CreateOptionsAuthenticationOptions
        {
            public bool Authenticate { get; set; } = false;
            public string[] AllowedRoles { get; set; }
            public string UserIdField { get; set; } = "Id";
            public string UserIdVariable { get; set; } = "user";
            public string UserType { get; set; } = "AppUser";
            public string SessionType { get; set; } = "CustomSession";

            public bool CheckUserOwnerShip { get; set; }
        }
     
}