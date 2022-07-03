using System;

namespace CourseProject.Attributes
{
    public class ForeignKeyToMany : Attribute
    {
        public string RelatedTableName { get; set; }
        public ForeignKeyToMany(string relatedTableName)
        {
            relatedTableName = RelatedTableName;
        }
    }

    public class ReadOnlyPropertyAttribute : Attribute
    {

    }

    public class TableNameAttribute : Attribute
    {
        public string Name { get; set; }

        public TableNameAttribute(string name)
        {
            Name = name;
        }
    }

    public class TransitionTableNameAttribute : TableNameAttribute
    {
        public TransitionTableNameAttribute(string name) : base(name)
        {
        }
    }

    public class RelatedTableNameAttribute : TableNameAttribute
    {
        public RelatedTableNameAttribute(string name) : base(name)
        {
        }
    }
    public class RelatedEntityTypeAttribute : TableNameAttribute
    {
        public RelatedEntityTypeAttribute(string name) : base(name)
        {
        }
    }

    public class MasterEntityNameAttribute : TableNameAttribute
    {
        public MasterEntityNameAttribute(string name) : base(name)
        {
        }
    }
    public class MasterTableNameAttribute : TableNameAttribute
    {
        public MasterTableNameAttribute(string name) : base(name)
        {
        }
    }
}