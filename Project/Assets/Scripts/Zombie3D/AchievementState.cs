using System;
using System.Collections.Generic;
using System.IO;

namespace Zombie3D
{
	public class AchievementState
	{
		public AchievementState()
		{
			for (int i = 0; i < 16; i++)
			{
				this.acheivements[i] = new AchievementInfo();
			}
			this.LoadAchievementConfig();
		}

		private void LoadAchievementConfig()
		{
		}

		public List<AchievementInfo> GetAchievementInfoList()
		{
			return this.achieveList;
		}

		public void SubmitScore(int score)
		{
			this.scoreInfo.score = score;
		}

		public void SubmitAllToGameCenter()
		{
		}

		public void GotNewWeapon()
		{
			this.newWeaponsGot++;
			this.CheckAchievemnet_NewBattleAbility();
			this.CheckAchievemnet_WeaponHouseware();
			this.CheckAchievemnet_WeaponCollector();
		}

		public void GotNewAvatar()
		{
			this.newAvatarGot++;
			this.CheckAchievemnet_Avatar();
			this.CheckAchievemnet_AvatarMaster();
		}

		public void UpgradeTenTimes()
		{
			this.upgradeTimes++;
			this.CheckAchievemnet_Upgrade();
			this.CheckAchievemnet_UpgradeMaster();
		}

		public void KillEnemy()
		{
			this.enemyKills++;
			if (GameApp.GetInstance().GetGameScene().GetPlayer().GetWeapon().GetWeaponType() == WeaponType.Saw)
			{
				this.sawKills++;
				this.CheckAchievemnet_SawKillers();
			}
			this.CheckAchievemnet_TookAShoot();
			this.CheckAchievemnet_Killer();
		}

		public void LoseGame()
		{
			this.loseTimes++;
		}

		public void AddScore(int scoreAdd)
		{
			this.score += scoreAdd;
		}

		public void Save(BinaryWriter bw)
		{
			bw.Write(this.score);
			bw.Write(this.newWeaponsGot);
			bw.Write(this.enemyKills);
			bw.Write(this.loseTimes);
			bw.Write(this.newAvatarGot);
			bw.Write(this.upgradeTimes);
			for (int i = 0; i < 16; i++)
			{
				bw.Write(this.achieveList[i].submitting);
				bw.Write(this.achieveList[i].complete);
			}
		}

		public void Load(BinaryReader br)
		{
			this.score = br.ReadInt32();
			this.newWeaponsGot = br.ReadInt32();
			this.enemyKills = br.ReadInt32();
			this.loseTimes = br.ReadInt32();
			this.newAvatarGot = br.ReadInt32();
			this.upgradeTimes = br.ReadInt32();
			for (int i = 0; i < 16; i++)
			{
				this.achieveList[i].submitting = br.ReadBoolean();
				this.achieveList[i].complete = br.ReadBoolean();
			}
		}

		public void CheckAchievemnet_NewBattleAbility()
		{
			this.CheckAchievemnet(AchieveType.NewBattleAbility, this.newWeaponsGot);
		}

		public void CheckAchievemnet_WeaponHouseware()
		{
			this.CheckAchievemnet(AchieveType.WeaponStore, this.newWeaponsGot);
		}

		public void CheckAchievemnet_SawKillers()
		{
			this.CheckAchievemnet(AchieveType.SawKillers, this.sawKills);
		}

		public void CheckAchievemnet_TookAShoot()
		{
			this.CheckAchievemnet(AchieveType.TookAShoot, this.enemyKills);
		}

		public void CheckAchievemnet_BraveHeart()
		{
			this.CheckAchievemnet(AchieveType.BraveHeart, 1);
		}

		public void CheckAchievemnet_Killer()
		{
			this.CheckAchievemnet(AchieveType.Killer, this.enemyKills);
		}

		public void CheckAchievemnet_RichMan(int cash)
		{
			this.CheckAchievemnet(AchieveType.RichMan, cash);
		}

		public void CheckAchievemnet_Survivior(int level)
		{
			this.CheckAchievemnet(AchieveType.Survivior, level);
		}

		public void CheckAchievemnet_LastSurvivior(int level)
		{
			this.CheckAchievemnet(AchieveType.LastSurvivior, level);
		}

		public void CheckAchievemnet_Avatar()
		{
			this.CheckAchievemnet(AchieveType.NewAvatarGot, this.newAvatarGot);
		}

		public void CheckAchievemnet_AvatarMaster()
		{
			this.CheckAchievemnet(AchieveType.AvatarMaster, this.newAvatarGot);
		}

		public void CheckAchievemnet_UpgradeMaster()
		{
			this.CheckAchievemnet(AchieveType.UpgradeMaster, this.upgradeTimes);
		}

		public void CheckAchievemnet_Upgrade()
		{
			this.CheckAchievemnet(AchieveType.Upgrade, 1);
		}

		public void CheckAchievemnet_WeaponMaster()
		{
			this.CheckAchievemnet(AchieveType.WeaponMaster, 1);
		}

		public void CheckAchievemnet_NeverGiveUp()
		{
			this.CheckAchievemnet(AchieveType.NeverGiveUp, this.loseTimes);
		}

		public void CheckAchievemnet_WeaponCollector()
		{
			this.CheckAchievemnet(AchieveType.WeaponCollector, this.newWeaponsGot);
		}

		private void CheckAchievemnet(AchieveType type, int num)
		{
			if (this.achieveList[(int)type].complete)
			{
				return;
			}
			if (num >= this.achieveList[(int)type].upperlimit)
			{
				this.achieveList[(int)type].complete = true;
				GameApp.GetInstance().Save();
			}
		}

		public int GetCurNumByAchieveType(AchieveType type)
		{
			int result = 0;
			switch (type)
			{
			case AchieveType.NewBattleAbility:
			case AchieveType.WeaponStore:
			case AchieveType.WeaponCollector:
				result = this.newWeaponsGot;
				break;
			case AchieveType.SawKillers:
				result = this.sawKills;
				break;
			case AchieveType.TookAShoot:
			case AchieveType.Killer:
				result = this.enemyKills;
				break;
			case AchieveType.BraveHeart:
			case AchieveType.WeaponMaster:
				result = ((!this.achieveList[(int)type].complete) ? 0 : 1);
				break;
			case AchieveType.NewAvatarGot:
			case AchieveType.AvatarMaster:
				result = this.newAvatarGot;
				break;
			case AchieveType.UpgradeMaster:
			case AchieveType.Upgrade:
				result = this.upgradeTimes;
				break;
			case AchieveType.NeverGiveUp:
				result = this.loseTimes;
				break;
			}
			return result;
		}

		protected int score;

		protected int newWeaponsGot;

		protected int newAvatarGot;

		protected int enemyKills;

		protected int sawKills;

		protected int loseTimes;

		protected int upgradeTimes;

		protected const int ACHIEVEMENT_COUNT = 16;

		protected AchievementInfo[] acheivements = new AchievementInfo[16];

		protected ScoreInfo scoreInfo = new ScoreInfo();

		protected List<AchievementInfo> achieveList;
	}
}
