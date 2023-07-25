using ChallengeAluraBackEnd_7.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAluraBackEnd_7.Data;

public class DepoimentosContext : DbContext
{
    public DepoimentosContext(DbContextOptions<DepoimentosContext> opts) : base(opts)
    {

    }

    public DbSet<Depoimento> Depoimentos { get; set; }
}
