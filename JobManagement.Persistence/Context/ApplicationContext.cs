﻿using JobManagement.Domain.JobManagers;
using JobManagement.Domain.JobManagers.Jobs.ValueObjects;
using JobManagement.Domain.Users;
using JobManagement.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using SmartEnum.EFCore;

namespace JobManagement.Persistence.Context;

internal class ApplicationContext : DbContext
{

    private readonly string _connectionString;

    public DbSet<JobManager> JobManagers { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")!; // required for application startup
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobManager>(managerBuilder =>
        {
            managerBuilder.ToTable("managers"); // not really plural

            managerBuilder.OwnsMany(p => p.Jobs, jobBuilder =>
            {
                jobBuilder.ToTable("jobs");
                jobBuilder.Property(p => p.Name)
                    .HasConversion(new ValueConverter<JobName, string>(
                        jobName => jobName.Value,
                        value => JobName.Create(value).Value));

                jobBuilder.OwnsOne(p => p.Log);
                jobBuilder.OwnsOne(p => p.ExecutionName);
            });
        });

        modelBuilder.Entity<User>(userBuilder =>
        {
            userBuilder.ToTable("users");

            userBuilder.HasKey(p => p.Name);
            userBuilder.Property(p => p.Name)
                .HasConversion(new ValueConverter<UserName, string>(
                    userName => userName.Value,
                    value => UserName.Create(value).Value));
            userBuilder.OwnsOne(p => p.Password);
        });

        modelBuilder.ConfigureSmartEnum();
    }

}
