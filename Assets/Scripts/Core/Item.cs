
namespace Assets.Scripts.Core
{
    public class Item
    {
        public readonly string name;
        public readonly string description;

        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public virtual void apply(PlayerController playerController)
        {

        }
    }
}
