﻿using DiIiS_NA.GameServer.MessageSystem;
using DiIiS_NA.GameServer.GSSystem.AISystem.Brains;
using DiIiS_NA.GameServer.GSSystem.PlayerSystem;
using DiIiS_NA.GameServer.GSSystem.MapSystem;
using DiIiS_NA.D3_GameServer.Core.Types.SNO;

namespace DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations.Minions
{
	class ZombieDog : Minion
	{
		public new int SummonLimit = 4;

		public ZombieDog(World world, Actor master, int dogID, float mul = 1f)
			: base(world, ActorSno._wd_zombiedog, master, null)
		{
			Scale = 1.35f;
			//TODO: get a proper value for this.
			WalkSpeed *= 5;
			DamageCoefficient = mul * 2f;
			SetBrain(new MinionBrain(this));
			Attributes[GameAttributes.Summoned_By_SNO] = 102573;

			Attributes[GameAttributes.Damage_Weapon_Min, 0] = master.Attributes[GameAttributes.Damage_Weapon_Min_Total, 0];
			Attributes[GameAttributes.Damage_Weapon_Delta, 0] = master.Attributes[GameAttributes.Damage_Weapon_Delta_Total, 0];


			//TODO: These values should most likely scale, but we don't know how yet, so just temporary values.
			//Attributes[GameAttribute.Hitpoints_Max] = 20f;
			//Attributes[GameAttribute.Hitpoints_Cur] = 20f;
			if ((master as Player).SkillSet.HasPassive(208563)) //ZombieHandler (wd)
			{
				Attributes[GameAttributes.Hitpoints_Max] *= 1.2f;
				Attributes[GameAttributes.Hitpoints_Cur] = Attributes[GameAttributes.Hitpoints_Max];
			}
			Attributes[GameAttributes.Attacks_Per_Second] = 1.0f;

			Attributes[GameAttributes.Damage_Weapon_Min, 0] = master.Attributes[GameAttributes.Damage_Weapon_Min_Total, 0] * mul;
			Attributes[GameAttributes.Damage_Weapon_Delta, 0] = master.Attributes[GameAttributes.Damage_Weapon_Delta_Total, 0] * mul;

			if ((master as Player).SkillSet.HasPassive(340909)) //MidnightFeast (wd)
			{
				Attributes[GameAttributes.Damage_Weapon_Min, 0] *= 1.5f;
			}

			Attributes[GameAttributes.Pet_Type] = 0x8;
			//Pet_Owner and Pet_Creator seems to be 0
			master.Attributes[GameAttributes.Free_Cast, SkillsSystem.Skills.WitchDoctor.Support.Sacrifice] = 1;
			master.Attributes.BroadcastChangedIfRevealed();
		}
	}
}
