using Microsoft.EntityFrameworkCore;


namespace WebApi
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        { }
        public bool Connected { get; set; }
    }
}
