using Microsoft.EntityFrameworkCore;

namespace Upload_Files_With_Fake_Extensions.Model
{
    public class ApplicationDbcontext:DbContext
    {

        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {

        }


        public DbSet<UploadFile> uploadFiles { get; set; }
    }
}
