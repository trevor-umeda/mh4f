﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace MH4F
{
    class LongSwordFactory : AbstractCharacterFactory
    {
        private String characterId = "LongSword";
        public override Player createCharacter(ContentManager content, int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager)
        {
            LongSwordPlayer longSwordPlayer = new LongSwordPlayer(playerNumber, xPosition, yHeight, comboManager, throwManager);
            base.loadCharacterDataConfigs(characterId, longSwordPlayer, content);
            return longSwordPlayer;
        }
    }
}
