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

        public String checkMoves(CharacterState characterState, Direction direction, KeyboardState newKeyboardState, Dictionary<string, Keys> controls)
        {
            String returnMove = null;
            if(!inputManager.DetermineButtonPress(newKeyboardState))
            {
            
                if (characterState == CharacterState.DASHING && newKeyboardState.IsKeyDown(controls["right"]))
                {
                    if (direction != Direction.Left)
                    {
                        return "dash";
                    }
                }
                if (characterState == CharacterState.DASHING && newKeyboardState.IsKeyDown(controls["left"]))
                {
                    if (direction == Direction.Left)
                    {
                       return "dash";
                    }    
                }
            }
            if(characterState != CharacterState.AIRBORNE)
            {
                returnMove = inputManager.checkGroundMoves(direction, newKeyboardState, controls);
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
