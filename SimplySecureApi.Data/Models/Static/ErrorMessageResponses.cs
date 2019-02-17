namespace SimplySecureApi.Data.Models.Static
{
    public static class ErrorMessageResponses
    {
        public static string UnableToFindModule => "Unable to find module.";

        public static string UnAuthorizedModifyLocation => "You are unauthorized to modify users/modules or permissions at this location. Only admins have this ability";

        public static string InvalidModuleId => "Invalid module id";

        public static string UnableToFindLocation => "Unable to find location";

        public static string UserNotAuthorizedForLocation => "User is not authorized for current location";

        public static string UnableToFindUser => "Unable to find user. Please have your user register";

        public static string UnableToDeleteAdmin => "You can not delete the location admin";

        public static string UnableToLogIn => "Unable to log in";

        public static string UnableToRegister => "Unable to register";

        public static string IncompleteDataReceived => "Incomplete data received";

        public static string AccountDeactivated => "Account deactivated";
    }
}