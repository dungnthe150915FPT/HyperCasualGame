using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Core.Library
{
    public class CONST
    {
        // Instanced Prefab Animation
        public static readonly Vector3 INSTANCED_PREFAB_ANIMATION_POSITION = new Vector3(1, -0.7f, 1);

        // Path to Assets Folder Resources
        public static readonly string
            ANIMATOR_CONTROLLER_PATH = "AnimatorControllers/Gun2New"
            , PREFAB_ANIMATED_PATH = "Prefabs/Sakura"
            , PREFAB_GAMEPLAY_UI = "Prefabs/UIGamePlay/CanvasGamePlay"
            , PREFAB_CIRCLE_BULLET_PATH = "Prefabs/Bullets/CircleBullet"
            , PREFAB_ROUND_BULLET_PATH = "Prefabs/Bullets/RoundBullet"
            , PREFAB_SHARP_BULLET_PATH = "Prefabs/Bullets/SharpBullet"
            , PREFAB_WEAPON_PICKUP_PATH = "Prefabs/WeaponPickup/WeaponPickup";

        // Object name
        public static readonly string
            OBJECT_NAME_PLAYER = "Player"
            , OBJECT_NAME_BULLET = "Bullet"
            , OBJECT_MUZZLE_EXTRACTOR = "MuzzleExtractor"
            , OBJECT_SHELL_EXTRACTOR = "ShellExtractor";

        // Events
        public static readonly string
            PATH_EVENT_FIRE = "Events/FireEvent"
            , PATH_EVENT_STOP_FIRE = "Events/StopFireEvent"
            , PATH_EVENT_SWITCH_WEAPON = "Events/SwitchWeaponEvent"
            , PATH_EVENT_JUMP = "Events/JumpEvent"
            , PATH_EVENT_RUN = "Events/RunEvent"
            , PATH_EVENT_RUN_STOP = "Events/RunStopEvent"
            , PATH_EVENT_MOVE = "Events/MoveEvent"
            , PATH_EVENT_RELOAD = "Events/ReloadEvent"
            , PATH_DEBUG_EVENT = "Events/DebugUIEvent"
            , PATH_EVENT_PICKUP_OBJECT = "Events/PickupObjectEvent";


        // SFX
        public static readonly string
            SOUND_RIFLE_FIRE_PATH = "SFX/gun-shot"
            , SOUND_BIG_SHOT_PATH = "SFX/182273__martian__gun-for-ghose"
            , SOUND_AUTO_SHOT_PATH = "SFX/274559__mrjohnweez__auto-machine-gun"
            , SOUND_RELOAD_PATH = "SFX/363167__samsterbirdies__mag-reload"
            , SOUND_COOKING_PATH = "SFX/pistol-gun-cocking"
            , SOUNG_SNIPER_RELOAD_PATH = "SFX/276956__gfl7__awp-reload-sound"
            , SOUND_NORMAL_RELOAD_PATH = "SFX/363167__samsterbirdies__mag-reload"
            , SOUND_HEAL_PATH = "SFX/heal";

        // Config File JSON
        public static readonly string FILE_WEAPON_CONFIG = "WeaponConfigTest2.json";

        // Virtual Camera
        public static readonly string VIRTUAL_CAMERA_NAME = "CM vcam1";
        // Player Input Actionss
        public static readonly string
            PLAYER_INPUT_ACTIONS_PATH = "InputActionAssets/PlayerInputActions"
            , ACTIONMAP_PLAYER = "Player"
            , ACTIONMAP_UI = "UI"
            , ACTION_MOVE = "Move"
            , ACTION_JUMP = "Jump"
            , ACTION_RUN = "Run"
            , ACTION_FIRE = "Fire"
            , ACTION_SWITCH_WEAPON = "SwitchWeapon";


        // Animator Controller Parameter
        public static readonly string
            ANIMATOR_CONTROLLER_PARAMETER_IS_AIMING = "isAiming"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND = "isInGround"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_ACCELERATING = "isAccelerating"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_IDLE = "isIdle"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_WALKING = "isWalking"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_RUNNING = "isRunning"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_JUMPING = "isJumping"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_FALLING = "isFalling"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_ATTACKING = "isAttacking"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_DEAD = "isDead"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_TAKING_DAMAGE = "isTakingDamage"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_CASTING = "isCasting"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_STUNNED = "isStunned"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_KNOCKED_BACK = "isKnockedBack"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_KNOCKED_DOWN = "isKnockedDown"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_FROZEN = "isFrozen"
            , ANIMATOR_CONTROLLER_PARAMETER_IS_BURNING = "isBurning"
            , ANIMATOR_CONTROLLER_PARAMETER_RELOAD_MULTIPLIER = "ReloadMultiplier";

        // Animator UP Trigger Parameter
        public static readonly string
            ANIMATOR_TRIGGER_BLANK = "UP_blank"
            , ANIMATOR_TRIGGER_AIM = "UP_aim"
            , ANIMATOR_TRIGGER_RELOAD1 = "UP_reload1"
            , ANIMATOR_TRIGGER_RELOAD2 = "UP_reload2"
            , ANIMATOR_TRIGGER_FIRE_SINGLE = "UP_fire light"
            , ANIMATOR_TRIGGER_FIRE_AUTO = "UP_fire light loop"
            , ANIMATOR_TRIGGER_FIRE_CHARGE = "UP_fire heavy";

        // Animation Length Parameter
        public static readonly float
            ANIMATION_LENGTH_RELOAD2 = 0.583f
            , ANIMATOR_LENGTH_EXIT_RELOAD2 = 0.15f;

        // game object tag
        public static readonly string
            TAG_PLAYER = "Player"
            , TAG_ENEMY = "Enemy"
            , TAG_PROJECTILE = "Projectile"
            , TAG_FLOOR = "Floor"
            , TAG_TOXIN = "Toxin"
            , TAG_BULLET = "Bullet";

        // game object layer
        public static readonly string
            LAYER_PLAYER = "Player"
            , LAYER_ENEMY = "Enemy"
            , LAYER_PROJECTILE = "Projectile"
            , LAYER_FLOOR = "Floor"
            , LAYER_MAGIC = "Magic";

        // scene name
        public static readonly string
            SCENE_MAIN_MENU = "MainMenuScene"
            , SCENE_GAMEPLAY = "Gameplay"
            , SCENE_TEST = "TestGamePlay";
    }
}
