using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    public class HealthPotion : Item
    {
        private const int healthRestore = 25;

        public HealthPotion() : base("Health Potion", "This magic potion can restore some of your health") { }

        public override void apply(IPlayer player)
        {
            player.Health += healthRestore;
        }
    }
}
