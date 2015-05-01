using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace MH4F
{
    class SpecialInputManager
    {
        private InputManager inputManager;
        public SpecialInputManager()
        {
            inputManager = new InputManager();
        }

        public String checkMoves(CharacterState characterState, Direction direction, KeyboardState newKeyboardState)
        {
            String returnMove = null;

            if (characterState == CharacterState.DASHING && newKeyboardState.IsKeyDown(Keys.Right))
            {
                if (direction != Direction.Left)
                {
                    returnMove = "rightdash";
                }
            }
            if (characterState == CharacterState.DASHING && newKeyboardState.IsKeyDown(Keys.Left))
            {
                if (direction == Direction.Left)
                {
                    returnMove = "leftdash";
                }    
            }
            if (characterState == CharacterState.STANDING)
            {
                returnMove = inputManager.checkGroundMoves(direction, newKeyboardState);
            }

            return returnMove; 
        }
        public void registerGroundMove(String name, List<String> input)
        {
            // input.Reverse();
            inputManager.registerGroundMove(name, input);
        }
    }
}
