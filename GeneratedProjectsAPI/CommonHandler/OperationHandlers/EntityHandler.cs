using GeneratedProjectsAPI.CommonHandler.Models;

using System.Text;

namespace GeneratedProjectsAPI.CommonHandler.OperationHandlers
{
    public class EntityHandler : BaseHandler
    {
        public override void Handle(RequestContext context)
        {
            Console.WriteLine("Generating entities models started");

            CreateEntityClasses(context.ProjectPath, context.Tables, context.ProjectName);

            base.Handle(context);
        }

        private void CreateEntityClasses(string projectPath, IEnumerable<Table> tables, string projectName)
        {
            var modelsFolderPath = Path.Combine(projectPath, $"{projectName}.Model");

            if (!Directory.Exists(modelsFolderPath))
            {
                Directory.CreateDirectory(modelsFolderPath);
            }

            foreach (var table in tables)
            {
                var entityCode = $@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace {projectName}.Model
{{
    public class {table.TableName}
    {{
        {GenerateEntityProperties(table.Columns)}
    }}
}}";

                var entityFilePath = Path.Combine(modelsFolderPath, $"{table.TableName}.cs");
                System.IO.File.WriteAllText(entityFilePath, entityCode);
            }
        }
        private string GenerateEntityProperties(IEnumerable<Column> columns)
        {
            var properties = new StringBuilder();
            foreach (var column in columns)
            {
                var keyAttribute = column.IsPrimaryKey ? "[Key]\n        " : "";
                properties.AppendLine($"{keyAttribute}public {column.Type} {column.Name} {{ get; set; }}");

                if (column.IsForeignKey)
                {
                    if (string.IsNullOrEmpty(column.ForeignKeyTableName))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("IsForeignKey true but ForeignKeyTableName null. Releation cannot not connect!!!");
                        Console.BackgroundColor = ConsoleColor.White;

                    }
                    if (column.Releation != ((int)ReleationType.ManyToMany))
                    {
                        var attribute = $"[ForeignKey(\"" + column.Name + "\")] \n";
                        properties.AppendLine($"{attribute}public {column.ForeignKeyTableName} {column.ForeignKeyTableName} {{ get; set; }}");
                    }
                }
            }
            return properties.ToString();
        }
    }
}
