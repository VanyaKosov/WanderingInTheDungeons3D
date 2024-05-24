namespace Assets.Scripts.Core
{
    public class SpeedBoots : Item
    {
        public const float speedChange = 2.0f;

        public SpeedBoots() : base("Speed Boots", "These boots increase your speed")
        {

        }

        public override void apply(IPlayer player)
        {
            player.MoveSpeed += speedChange;
            player.StepWait = 0.3f;
        }
    }
}
