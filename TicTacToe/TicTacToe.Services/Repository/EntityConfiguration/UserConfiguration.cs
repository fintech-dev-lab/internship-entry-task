using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Core.Entities;

namespace TicTacToe.Services.Repository.EntityConfiguration;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Uuid);
        
        builder.Property(u => u.Uuid)
            .ValueGeneratedOnAdd();
    }
}