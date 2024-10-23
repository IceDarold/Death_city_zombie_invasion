using System;

namespace Zombie3D
{
	public class Constant
	{
		public const float CAMERA_ZOOM_SPEED = 10f;

		public const float DEFLECTION_SPEED = 2f;

		public const float FLOORHEIGHT = 0f;

		public const float POWERBUFFTIME = 30f;

		public const float ITEM_HIGHPOINT = 1.2f;

		public const float ITEM_LOWPOINT = 1f;

		public const float PLAYING_WALKINGSPEED_DISCOUNT_WHEN_SHOOTING = 0.8f;

		public const float ANIMATION_ENDS_PERCENT = 1f;

		public const float SPARK_MIN_DISTANCE = 2f;

		public const float MAX_WAVE_TIME = 1800f;

		public const float DOCTOR_HP_RECOVERY = 1f;

		public const float MARINE_POWER_UP = 1.2f;

		public const float SWAT_HP = 2f;

		public const float NERD_MORE_LOOT_RATE = 1.3f;

		public const float COWBOY_SPEED_UP = 1.3f;

		public const float RICHMAN_MONEY_MORE = 1.2f;

		public const float ENEGY_ARMOR__DAMAGE_BOOST = 2f;

		public const float ENEGY_ARMOR_HP_BOOST = 3f;

		public const float ENEGY_ARMOR_SPEED_UP = 1.3f;

		public const int MAX_CASH = 99999999;

		public const int SELECTION_NUM = 3;

		public const int LEVEL_NUM = 20;

		public const float R = 0.5f;

		public const int MAX_LEVEL_NUM = 30;

		public const int REWARD_LEVEL_NUM = 10;

		public const int ENDLESS_LEVEL_NUM = 2;

		public const int DEFEND_LEVEL_NUM = 5;

		public const float RADAR_RADIUS = 8f;

		public const float SHOOT_ANI_LENGTH = 0.167f;

		public const int INT_BYTE_NUM = 32;

		public const string SCENE_PREFAB_PATH = "Assets/Resources/";

		public const float WEAPON_RAYCAST_LENGTH = 30f;

		public const float CONTINUOUS_KILL_TIME_LMT = 1.5f;

		public const float BULLET_TIME_INTERVAL = 10f;

		public const string ENEMY_PREFAB_PATH = "Prefabs/zombies/";

		public const string ENEMY_DEADBODY_PATH = "Prefabs/DeadBody/";

		public const string ENEMY_DEADBODY_SUFFIX = "_RAGDOLL";

		public const int RAMDOM_LEVEL_DATA_DELAY = 2;

		public const int GOLD_LEVEL_DATA_DELAY = 1;

		public const float NORMAL_FOV = 60f;

		public static float[] SNIPESCOPE_FOV = new float[]
		{
			60f,
			50f,
			40f,
			30f,
			27f,
			23f,
			19f,
			15f,
			13f,
			11f,
			9f,
			7f,
			6f
		};
	}
}
