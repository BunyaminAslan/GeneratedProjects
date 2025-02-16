namespace GeneratedProjectsAPI.CommonHandler.Models
{
    public class ProjectRequest
    {
        public string ProjectPath { get; set; }
        public string ProjectName { get; set; }
        public string SolutionName { get; set; }
    }

    public class RequestContext : ProjectRequest
    {
        public string ConnectionString { get; set; }
        public List<Table> Tables { get; set; }
    }

    public class Table
    {
        public string TableName { get; set; }
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsPrimaryKey { get; set; } = false;
        public bool IsForeignKey { get; set; } = false;
        public string ForeignKeyTableName { get; set; }
        public int? Releation { get; set; } = null;

    }
    public enum ReleationType
    {
        OneToOne,
        OneToMany,
        ManyToMany
    };
}
