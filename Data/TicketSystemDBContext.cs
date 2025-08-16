using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TicketSystem.Models;
using TicketSystem.Views.Home;

namespace TicketSystem.Data
{
    public class TicketSystemDBContext : DbContext
    {

        public TicketSystemDBContext(DbContextOptions<TicketSystemDBContext> options)
        : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        //{
        //    var configBuilder = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //    var configSection = configBuilder.GetSection("ConnectionStrings");
        //    var connectionString = configSection["SQLServerConnection"] ?? null;
        //    optionsBuilder.UseSqlServer(connectionString);
        //    //optionsBuilder.UseSqlServer(@"Server=.;Database=TicketSystem;Trusted_Connection=false;user id=sa;password=Msc@@2025;TrustServerCertificate=True;");
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Watcher> watcher { get; set; }

        public DbSet<Models.IncidentModel> Incident { get; set; }

        public DbSet<TicketWatcher> TicketWatchers { get; set; }
        public DbSet<PreviousComments> PreviousComments { get; set; }
        public DbSet<Models.LoginModel> Users { get; set; }
        public DbSet<Caller> callers { get; set; }
        public DbSet<Models.DashboardModel> Dashboards { get; set; }

        public DbSet<Models.SignupModel> Signup { get; set; }

        public DbSet<Admin> Admins { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TicketWatcher>()
                .HasKey(tw => new { tw.TicketId, tw.WatcherId });

            modelBuilder.Entity<TicketWatcher>()
                .HasOne(tw => tw.Ticket)
                .WithMany(t => t.TicketWatchers)
                .HasForeignKey(tw => tw.TicketId);

            modelBuilder.Entity<TicketWatcher>()
                .HasOne(tw => tw.Watcher)
                .WithMany(w => w.TicketWatchers)
                .HasForeignKey(tw => tw.WatcherId);

            modelBuilder.Entity<Models.IncidentModel>()
                .Property(i => i.State)
                .HasConversion<int>();

            modelBuilder.Entity<Admin>()
            .HasOne(a => a.Login)
            .WithMany()
            .HasForeignKey(a => a.LoginId);


        }

    }
}
