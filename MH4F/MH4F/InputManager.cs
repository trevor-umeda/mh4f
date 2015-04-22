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
            foreach(MoveInput moveInput in moveList)
            {
                moveInput.resetCurrentInputCommandIndex();
            }
            List<String> fireball = moveList[0].InputCommand;
            int x = moveList[0].InputCommand.Count - 1;
            foreach (KeyboardState n in inputs)
            {
                foreach (MoveInput moveInput in moveList)
                {
                    if (MoveInput.checkStringInputToKeyInput(moveInput.InputCommand[moveInput.CurrentInputCommandIndex], n))
                    {
                        moveInput.decrementCurrentInputCommandIndex();
                        if (moveInput.CurrentInputCommandIndex < 0)
                        {
                            System.Diagnostics.Debug.WriteLine(moveInput.Name);
                            inputs.Reset();
                            return moveInput.Name;
                        }
                        
                    }
                }
               
                      
            }
            return null;
        }

        public void registerMove(String name, List<String> input)
        {
            input.Reverse();
            moveList.Add(new MoveInput(name, input));
        }

        public bool KeyboardPressed(KeyboardState keyboardState, Keys key)
        {
            return (keyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        public bool KeyboardReleased(KeyboardState keyboardState, Keys key)
        {
            return (keyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key));
        }

        public bool KeyboardDown(KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
