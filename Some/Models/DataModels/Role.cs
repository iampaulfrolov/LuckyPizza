﻿using System;
using CourseProject.Attributes;
using CourseProject.Models.DataModels;

namespace CourseProject.Identity.Models;

[TableName("Role")]
public class Role : Entity
{
    public Role()
    {
        Id = Guid.NewGuid().GetHashCode();
        Id = 1;
    }

    public Role(string name) : this()
    {
        Name = name;
    }

    public Role(int id, string name)
    {
        Name = name;
        Id = id;
    }

    public string Name { get; set; }
}