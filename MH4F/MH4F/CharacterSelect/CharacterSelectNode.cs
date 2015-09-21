using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    class CharacterSelectNode
    {
        private String characterId;

        public CharacterSelectNode(String character)
        {
            characterId = character;
        }

        public String CharacterId { get { return characterId; } }
    }
}
