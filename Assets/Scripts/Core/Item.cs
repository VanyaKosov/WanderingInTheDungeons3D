
namespace Assets.Scripts.Core
{
    public abstract class Item
    {
        public readonly string name;
        public readonly string description;

        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public abstract void apply(IPlayer player);
    }
}
