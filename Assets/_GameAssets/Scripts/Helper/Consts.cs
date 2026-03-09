using System;
using UnityEngine;

public class Consts 
{

    public struct SceneNames
    {
        public const string MainMenu_Scene = "MainMenuScene";
        public const string Game_Scene = "GameScene";

    }
    public struct Layers
    {
        public const string GROUND_LAYER = "Ground";
        public const string FLOOR_LAYER = "Floor";
    }
   public struct PlayerAnimations
    {
        public const string IS_MOVING = "isMoving";
        public const string IS_RUNNING = "isRunning";
        public const string IS_JUMPING = "isJumping";
        public const string IS_FALLING = "isFalling";
        public const string IS_LANDING = "isLanding";
      // public const string IS_SLIDING_ACTIVE = "IsSlidingActive";
    }
    public struct CatAnimations
    {
        public const string IS_IDLING = "IsIdling";
        public const string IS_WALkING = "IsWalking";
        public const string IS_RUNNING = "IsRunning";
        public const string IS_ATTACKING = "IsAttacking";
    }
    public struct OtherAnimtion
    {
        public const string IS_SPATULA_JUMPING = "IsSpatulaJumping";
    }
    
    public struct WheatTypes
    {
        public const string GOLD_WHEAT = "GoldWheat";
         public const string HOLY_WHEAT = "HolyWheat";
          public const string ROTTEN_WHEAT = "RottenWheat";
    }
}
