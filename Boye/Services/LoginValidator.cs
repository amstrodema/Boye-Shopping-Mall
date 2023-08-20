namespace Boye.Services
{
    public class LoginValidator
    {
        private readonly IHttpContextAccessor _context;
        public LoginValidator(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }


        public bool IsLoggedIn()
        {
            try
            {
                //HttpContext.Current.Session["userID"] = "beb01db7-a035-46fd-92a0-64dc49fe4a74";
                if (_context.HttpContext.Session.GetString("userID") != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsAdmin()
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return false;
                }
                if (_context.HttpContext.Session.GetString("isAdmin") != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsDev()
        {
            try
            {
                //HttpContext.Current.Session["isDev"] = true;
                if (!IsLoggedIn())
                {
                    return false;
                }
                if (_context.HttpContext.Session.GetString("isDev") != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public string GetUserID()
        {
            return _context.HttpContext.Session.GetString("userID");
        }
    }
}
