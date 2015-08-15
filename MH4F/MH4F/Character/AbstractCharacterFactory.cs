using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MH4F
{
    abstract class AbstractCharacterFactory
    {
        public abstract Player createCharacter(ContentManager content,int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager);
        protected void loadCharacterData(String character, Player player, ContentManager content)
        {
            // Load Sprite Info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Sprites.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                bool readingAnimations = false;
                Dictionary<String, Texture2D> spriteTextures = new Dictionary<String, Texture2D>();
                while (sreader.Peek() >= 0)
                {
                    String spriteLine = sreader.ReadLine();
                    Console.WriteLine(spriteLine);
                    if (spriteLine.Contains("-SPRITEANIMATIONS-"))
                    {
                        readingAnimations = true;
                    }
                    else if (spriteLine.Contains("-SPRITESHEET-"))
                    {
                        readingAnimations = false;
                    }
                    else if (!readingAnimations)
                    {
                        Texture2D test = content.Load<Texture2D>(spriteLine);
                        spriteTextures[spriteLine] = test;
                    }
                    else
                    {
                        String[] sHb = spriteLine.Split(';');
                        Console.WriteLine("Using " + sHb[0]);
                        //player2.Sprite.AddAnimation(standing, "standing", 0, 0, 144, 288, 8, 0.1f, CharacterState.STANDING);                       
                        CharacterState moveState;
                        Enum.TryParse(sHb[8], true, out moveState);
                        if (sHb.Length >= 10)
                        {
                            // Hacky way to add moves
                            //
                            if (sHb[9] == "true")
                            {
                                player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                                                          int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState, true);
                            }
                            else
                            {
                                player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                                                          int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState, sHb[9]);
                            }

                        }
                        else
                        {
                            player.Sprite.AddAnimation(spriteTextures[sHb[0]], sHb[1], int.Parse(sHb[2]), int.Parse(sHb[3]), int.Parse(sHb[4]),
                            int.Parse(sHb[5]), int.Parse(sHb[6]), float.Parse(sHb[7]), moveState);
                        }

                    }



                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load hitbox info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Hitbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hitboxInfo = sreader.ReadLine();
                    Console.WriteLine(hitboxInfo);
                    String[] sHb = hitboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);

                    player.Sprite.AddHitbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Hurtbox info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Hurtbox.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String hurtboxInfo = sreader.ReadLine();
                    Console.WriteLine(hurtboxInfo);
                    String[] sHb = hurtboxInfo.Split(';');
                    Console.WriteLine(sHb[0]);
                    player.Sprite.AddHurtbox(sHb[0], Convert.ToInt32(sHb[1]), new Hitbox(sHb[2], sHb[3], sHb[4], sHb[5]));
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Move info
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Moves.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();
                    if (movesInfo.Contains("- MoveName"))
                    {

                    }
                    else
                    {
                        Console.WriteLine(movesInfo);
                        String[] sHb = movesInfo.Split(';');
                        Console.WriteLine(sHb[0]);
                        Hitzone hitZone;
                        Enum.TryParse(sHb[3], true, out hitZone);
                        Boolean isHardKnockdown = false;
                        if (sHb[5] == "true")
                        {
                            isHardKnockdown = true;
                        }
                        HitInfo hitInfo = new HitInfo(int.Parse(sHb[1]), int.Parse(sHb[2]), hitZone);
                        hitInfo.IsHardKnockDown = isHardKnockdown;
                        hitInfo.Damage = int.Parse(sHb[4]);
                        hitInfo.AirUntechTime = int.Parse(sHb[6]);
                        hitInfo.AirXVelocity = int.Parse(sHb[7]);
                        hitInfo.AirYVelocity = -int.Parse(sHb[8]);
                        if (sHb.Length > 9)
                        {
                            if (sHb[9] == "true")
                            {
                                hitInfo.ForceAirborne = true;
                            }
                        }
                        if (sHb.Length > 10)
                        {
                            if (sHb[10] == "true")
                            {
                                hitInfo.FreezeOpponent = true;
                            }
                        }
                        if (sHb.Length > 11)
                        {
                            if (sHb[11] == "true")
                            {
                                hitInfo.Unblockable = true;
                            }
                        }
                        player.SetAttackMoveProperties(sHb[0], hitInfo);
                    }
                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Generic Move Input data
            //
            Dictionary<String, MoveInput> moveInputList = new Dictionary<String, MoveInput>(); ;
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("UniversalInputs.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data           
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();

                    Console.WriteLine(movesInfo);
                    String[] moves = movesInfo.Split(' ');
                    moveInputList.Add(moves[0], new MoveInput(moves[0], new List<String> { moves[1] }));
                    //player.SetAttackMoveProperties(sHb[0], hitInfo);

                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load character specific input move data
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "MoveInputs.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data           
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();

                    Console.WriteLine(movesInfo);
                    String[] moves = movesInfo.Split(' ');
                    String[] inputs = moves[1].Split(';');
                    List<String> inputList = new List<String>(inputs);
                    moveInputList.Add(moves[0], new MoveInput(moves[0], inputList));
                    //player.SetAttackMoveProperties(sHb[0], hitInfo);
                   // player1.RegisterGroundMove("fireball", new List<string> { "2", "3", "6", "A" });
                    player.RegisterGroundMove(moves[0], inputList);

                }
                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            // Load Gatling
            //
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(character + "Gatling.txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data
                int lineNumber = 0;
                List<String> moves = new List<String>(); ;
                while (sreader.Peek() >= 0)
                {
                    String movesInfo = sreader.ReadLine();
                    if (lineNumber == 0)
                    {
                        Console.WriteLine(movesInfo);
                        moves = new List<String>(movesInfo.Split('|'));
                    }
                    else
                    {
                        Console.WriteLine(movesInfo);
                        String[] sHb = movesInfo.Split('|');
                        List<MoveInput> moveInputs = new List<MoveInput>();
                        for (int i = 0; i < moves.Count; i++)
                        {
                            if (sHb[i].Trim().Equals("x"))
                            {
                                moveInputs.Add(moveInputList[moves[i].Trim()]);
                            }
                            Console.WriteLine("Use " + moves[i].Trim() + " is " + sHb[i].Trim());
                        }
                        player.SpecialInputManager.registerGatling(sHb[0].Trim(), moveInputs);


                    }
                    lineNumber++;
                }

                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }

            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }

            player.Sprite.CurrentAnimation = "standing";
        }
    }
}
