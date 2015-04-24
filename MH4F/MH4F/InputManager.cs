using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;
namespace MH4F
{
    class InputManager
    {

        InputQueue<KeyboardState> inputs;
        KeyboardState lastKeyboardState;
        int maxInputQueueSize = 7;

        List<MoveInput> moveList;
        
        public KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }
        public InputManager()
        {
            inputs = new InputQueue<KeyboardState>();
            lastKeyboardState = new KeyboardState();
            moveList = new List<MoveInput>();
        }

        public String checkMoves(KeyboardState newKeyboardState)
        {
            inputs.Enqueue(newKeyboardState);
            if (DetermineButtonPress(newKeyboardState, lastKeyboardState))
              {
                System.Diagnostics.Debug.WriteLine("A button pressed");
            
                foreach(MoveInput moveInput in moveList)
                {
                    moveInput.resetCurrentInputCommandIndex();
                }
            
                lastKeyboardState = newKeyboardState;
           
                foreach (KeyboardState keyboardState in inputs)
                {
                    foreach (MoveInput moveInput in moveList)
                    {
                         if (MoveInput.checkStringInputToKeyInput(moveInput.InputCommand[moveInput.CurrentInputCommandIndex], keyboardState))
                        {
                            moveInput.moveCurrentInputCommandIndex();
                            if (moveInput.CurrentInputCommandIndex >= moveInput.InputCommand.Count)
                            {
                                System.Diagnostics.Debug.WriteLine(moveInput.Name);
                                inputs.Reset();                       
                                return moveInput.Name;
                            }
                        }
                    }              
                }
            }
            lastKeyboardState = newKeyboardState;
            return null;
        }

        public bool DetermineButtonPress(KeyboardState presentState, KeyboardState pastState)
        {
            if (MoveInput.KeyboardPressed(presentState, pastState, Keys.A))
            {
                return true;
            }
            return false;
        }

        public void registerMove(String name, List<String> input)
        {
           // input.Reverse();
            moveList.Add(new MoveInput(name, input));
        }


    }
}
