using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    public abstract class Entity
    {
        [ReadOnlyProperty] 
        public virtual int Id { get; set; }

        protected Entity()
        {
        }
    }
}