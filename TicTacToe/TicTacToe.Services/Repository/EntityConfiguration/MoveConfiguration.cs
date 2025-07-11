using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Core.Entities;

namespace TicTacToe.Services.Repository.EntityConfiguration;

public class MoveConfiguration: IEntityTypeConfiguration<Move>
{
    public void Configure(EntityTypeBuilder<Move> builder)
    {
        builder.HasKey(m => m.Uuid);
        
        builder.Property(m => m.Uuid)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(m => m.Game)
            .WithMany(g => g.Moves)
            .HasForeignKey(m => m.GameUuid);
        
        builder
            .HasOne(m => m.Player)
            .WithMany()
            .HasForeignKey(m => m.PlayerUuid);
    }
}