using System;

namespace CourseProject.Attributes;

public class ForeignKeyToMany : Attribute
{
    public ForeignKeyToMany(string relatedTableName)
    {
        relatedTableName = RelatedTableName;
    }

    public string RelatedTableName { get; set; }
}

public class ReadOnlyPropertyAttribute : Attribute
{
}

public class TableNameAttribute : Attribute
{
    public TableNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
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