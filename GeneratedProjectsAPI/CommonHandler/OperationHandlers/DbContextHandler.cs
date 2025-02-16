using GeneratedProjectsAPI.CommonHandler.Models;

using System.Text;

namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class DbContextHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating DbContext and setting up domain models started");

            CreateDbContext(context.ProjectPath, context.Tables, context.ProjectName);

            base.Handle(context);
        }
        private void CreateDbContext(string projectPath, IEnumerable<Table> tables, string projectName)
        {
            var dbContextCode = $@"
using Microsoft.EntityFrameworkCore;
using {projectName}.Model;


namespace {projectName}.Repository
{{
    public class AppDbContext : DbContext
    {{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {{ }}

        {GenerateDbSets(tables)}

        {GenerateModelCreating(tables)}
    
    }}
}}";

            var repositoryFolderPath = Path.Combine(projectPath, $"{projectName}.Repository");

            // Klasör yoksa oluştur
            if (!Directory.Exists(repositoryFolderPath))
            {
                Directory.CreateDirectory(repositoryFolderPath);
            }

            var dbContextFilePath = Path.Combine(repositoryFolderPath, "AppDbContext.cs");

            if (File.Exists(dbContextFilePath))
            {
                File.Delete(dbContextFilePath); // Eski dosyayı sil
            }

            File.WriteAllText(dbContextFilePath, dbContextCode);
        }

        private string GenerateModelCreating(IEnumerable<Table> tables)
        {
            var dbSets = new StringBuilder();

            dbSets.AppendLine(@$" protected override void OnModelCreating(ModelBuilder modelBuilder)
        {{");
            

            foreach (var table in tables)
            {
                foreach (var item in table.Columns) { 
                    if(item.IsForeignKey && !string.IsNullOrWhiteSpace(item.ForeignKeyTableName))
                    {
                        switch (item.Releation)
                        {
                            case ((int)ReleationType.OneToOne):
                                dbSets.Append($@"modelBuilder.Entity<{item.ForeignKeyTableName}>()
         .HasOne(u => u.{table.TableName})
         .WithOne(a => a.{item.ForeignKeyTableName})
         .HasForeignKey<{table.TableName}>(a => a.{item.Name}) // Doğru FK
         .IsRequired();");
                                dbSets.AppendLine();
                                dbSets.AppendLine();

                                break;

                            case ((int)ReleationType.OneToMany):
                                dbSets.Append($@"modelBuilder.Entity<{table.TableName}>()
                .HasOne(a => a.{item.ForeignKeyTableName})
                .WithMany(u => u.{item.ForeignKeyTableName}{table.TableName}s)
                .HasForeignKey(a => a.{item.Name})
                .OnDelete(DeleteBehavior.Restrict);");

                                dbSets.AppendLine();
                                dbSets.AppendLine();


                                break;
                            case ((int)ReleationType.ManyToMany):
                                dbSets.Append($@"modelBuilder.Entity<{item.ForeignKeyTableName}>()
           .HasMany(s => s.{item.ForeignKeyTableName}{table.TableName}s)
           .WithMany(c => c.{item.ForeignKeyTableName}s)
           .UsingEntity(j => j.ToTable(""{item.ForeignKeyTableName}{table.TableName}""));");

                                dbSets.AppendLine();
                                dbSets.AppendLine();

                                break;
                            default:
                                break;
                        }

                        
                    }
                }
            }
            dbSets.AppendLine();

            dbSets.Append($@" base.OnModelCreating(modelBuilder);
        }}");

            return dbSets.ToString();
        }

        private string GenerateDbSets(IEnumerable<Table> tables)
        {
            var dbSets = new StringBuilder();
            foreach (var table in tables)
            {
                dbSets.AppendLine($"        public DbSet<{table.TableName}> {table.TableName} {{ get; set; }}");
            }
            return dbSets.ToString();
        }

    }
}
