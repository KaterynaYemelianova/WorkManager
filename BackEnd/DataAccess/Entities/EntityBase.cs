namespace DataAccess.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && (obj as EntityBase).Id == Id;
        }
    }
}
