namespace Assets.Scripts.Core
{
    public class SpeedBoots : Item
    {
        public const float speedChange = 3.0f;

        public SpeedBoots(string name, string description) : base(name, description)
        {

        }

        public override void apply(IPlayer player)
        {
            player.MoveSpeed += speedChange;
        }
    }
}
