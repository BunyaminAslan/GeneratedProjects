** Sample Req.

/*
        public enum ReleationType
    {
        OneToOne,
        OneToMany,
        ManyToMany
    };
*/
{
  "projectPath": "C:/Calismalar/GenericTableRelation",
  "solutionName" : "AdminPanel",
  "projectName" : "AdminPanelProject",
  "connectionString" : "Server=localhost\\\\SQLEXPRESS;Initial Catalog=AdminPanel2;Trusted_Connection=True;Encrypt=False",
  "tables": [
    {
       "tableName": "Address",
       "columns": [
        { "name": "AddressId", "type": "int", "isPrimaryKey": true },
        { "name": "Title", "type": "string"  },
        { "name": "City", "type": "string" },
        { "name": "CountryCode", "type": "string" },
        { "name": "ZipCode", "type": "string" },
        { "name": "AddressText", "type": "string" },
        { "name" : "InsertDate","type" : "DateTime" },
        { "name" : "UpdateDate","type" : "DateTime" },
        { "name" : "Status","type" : "char"},
        { "name" : "UserId","type" : "int", "isForeignKey" : true, "foreignKeyTableName":"User", "releation":"0"}
      ]
    },
    {
       "tableName": "Article",
       "columns": [
        { "name": "ArticleId", "type": "int", "isPrimaryKey": true },
        { "name": "Title", "type": "string"  },
        { "name": "Description", "type": "string" },
        { "name" : "InsertDate","type" : "DateTime" },
        { "name" : "UpdateDate","type" : "DateTime" },
        { "name" : "Status","type" : "char"},
        { "name" : "UserId","type" : "int", "isForeignKey" : true, "foreignKeyTableName":"User", "releation":"1"}
      ]
    },
     {
       "tableName": "Role",
       "columns": [
        { "name": "RoleId", "type": "int", "isPrimaryKey": true },
        { "name": "RoleName", "type": "string"  },
        { "name": "RoleDescription", "type": "string" },
        { "name" : "InsertDate","type" : "DateTime" },
        { "name" : "UpdateDate","type" : "DateTime" },
        { "name" : "Status","type" : "char"},
        { "name" : "Users","type" : "List<User>", "isForeignKey" : true, "foreignKeyTableName":"User", "releation":"2"}
      ]
    },
    {
      "tableName": "User",
      "columns": [
        { "name":  "UserId", "type": "int",  "isPrimaryKey": true        },
        { "name" : "Name","type" : "string"     },
        { "name" : "Surname","type" : "string"  },
        { "name" : "Email","type" : "string"    },
        { "name" : "Password","type" : "string" },
        { "name" : "AdressId","type" : "int"    },
        { "name" : "InsertDate","type" : "DateTime" },
        { "name" : "UpdateDate","type" : "DateTime" },
        { "name" : "Status","type" : "char" },
        { "name" : "Address","type" : "Address" },
        { "name" : "UserRoles","type" : "List<Role>" },
        { "name" : "UserArticles","type" : "List<Article>" }
      ]
    }
  ]
}
