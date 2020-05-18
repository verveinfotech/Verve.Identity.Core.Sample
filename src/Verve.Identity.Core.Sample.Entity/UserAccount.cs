using Verve.Identity.Core.Model;

namespace Verve.Identity.Core.Sample.Entity
{
    public class UserAccount : VerveUserAccount
    {
        public int Status { get; set; }

        public string Name { get; set; }

    }
}