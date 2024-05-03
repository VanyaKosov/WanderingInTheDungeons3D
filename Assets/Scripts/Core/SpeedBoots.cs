using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    public class SpeedBoots : Item
    {
        public const float speedChange = 3.0f;

        public SpeedBoots(string name, string description) : base(name, description)
        {
            
        }

        public override void apply(PlayerController playerController)
        {
            playerController.playerSpeed += speedChange;
        }
    }
}
