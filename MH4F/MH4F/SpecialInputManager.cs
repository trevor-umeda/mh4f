﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace MH4F
{
    public class SpecialInputManager
    {
        private InputManager inputManager;
        private ControlSetting controlSetting;
        
        public SpecialInputManager()
        {
            inputManager = new InputManager();
        }

        public ControlSetting ControlSetting
        {
            get { return controlSetting; }
            set 
            {
                this.controlSetting = value;
                this.inputManager.ControlSetting = value;
            }
        }

        public String checkMoves(CharacterState characterState, Direction direction, KeyboardState newKeyboardState)
        {
            Dictionary<String, Keys> controls = controlSetting.Controls;
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
                returnMove = inputManager.checkGroundMoves(direction, newKeyboardState);
            }
            else
            {
                returnMove = inputManager.checkAirMoves(direction, newKeyboardState);
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
