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

        List<MoveInput> groundMoveList;
        List<MoveInput> dashList;
        public KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }
        public InputManager()
        {
            inputs = new InputQueue<KeyboardState>();
            lastKeyboardState = new KeyboardState();
            groundMoveList = new List<MoveInput>();
            dashList = new List<MoveInput>();
            // TODO: this is a terrible place to put these. find a better spot
            //
            dashList.Add(new MoveInput("backstep", new List<string> { "4", "5", "4" }));
            dashList.Add(new MoveInput("dash", new List<string> { "6", "5", "6" }));
           
        }

        public String checkGroundMoves(Direction direction, KeyboardState newKeyboardState, Dictionary<string, Keys> controls)
        {
            inputs.Enqueue(newKeyboardState);
            // on a button press determine if a special move was inputted.
            //
            if (DetermineButtonPress(newKeyboardState, lastKeyboardState))
            {
                System.Diagnostics.Debug.WriteLine("A button pressed");

                foreach (MoveInput moveInput in groundMoveList)
                {
                    moveInput.resetCurrentInputCommandIndex();
                }
                lastKeyboardState = newKeyboardState;
                foreach (KeyboardState keyboardState in inputs)
                {
                    foreach (MoveInput moveInput in groundMoveList)
                    {
                        if (MoveInput.checkStringInputToKeyInput(moveInput.InputCommand[moveInput.CurrentInputCommandIndex], keyboardState, direction, controls))
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
            // Otherwise this is a movement special input
            //
            else
            {
                // Atm we only really care about reading a dash
                //
                foreach (MoveInput dashInput in dashList)
                {
                    dashInput.resetCurrentInputCommandIndex();
                }
                
                foreach (KeyboardState keyboardState in inputs.GetReverseEnumerator)
                {
                    foreach (MoveInput dash in dashList)
                    {

                        if (MoveInput.checkStringInputToKeyInput(dash.InputCommand[dash.CurrentInputCommandIndex], keyboardState, direction, controls))
                        {
                            dash.moveCurrentInputCommandIndex();
                            if (dash.CurrentInputCommandIndex >= dash.InputCommand.Count)
                            {
                                System.Diagnostics.Debug.WriteLine(dash.Name);
                                inputs.Reset();
                                return dash.Name;
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

        public void registerGroundMove(String name, List<String> input)
        {
           // input.Reverse();
            groundMoveList.Add(new MoveInput(name, input));
        }


    }
}
