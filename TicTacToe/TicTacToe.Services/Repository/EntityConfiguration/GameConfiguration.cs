using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Core.Entities;

namespace TicTacToe.Services.Repository.EntityConfiguration;

public class GameConfiguration: IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Uuid);
        
        builder.Property(g => g.Uuid)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(g => g.FirstPlayer)
            .WithMany()
            .HasForeignKey(g => g.FirstPlayerUuid)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(g => g.SecondPlayer)
            .WithMany()
            .HasForeignKey(g => g.SecondPlayerUuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}