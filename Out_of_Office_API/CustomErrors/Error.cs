using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Serialization;

namespace Out_of_Office_API.CustomErrors
{
    [Serializable]
    public class Error
    {
        public Error(string error)
        {
            this.error = error;
        }

        public Error(List<IdentityError> errors)
        {
            error = errors.Select(t => t.Description).ToList();
        }

        public object error { get; set; }

        public object getError()
        {
            return new {error = error };
        }
    }
}
