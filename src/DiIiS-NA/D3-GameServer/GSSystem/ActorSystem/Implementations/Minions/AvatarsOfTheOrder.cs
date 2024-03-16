﻿using DiIiS_NA.GameServer.MessageSystem;
using DiIiS_NA.GameServer.GSSystem.AISystem.Brains;
using DiIiS_NA.GameServer.GSSystem.PowerSystem;
using DiIiS_NA.GameServer.GSSystem.TickerSystem;
using DiIiS_NA.GameServer.GSSystem.MapSystem;
using DiIiS_NA.D3_GameServer.Core.Types.SNO;

namespace DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations.Minions
{
	class AvatarMelee : Minion
	{
		public AvatarMelee(World world, PowerContext context, int AvatarID, float damageMult, TickTimer lifeTime)
			: base(world, ActorSno._x1_crusader_phalanx, context.User, null)
		{
			Scale = 1.2f;
			WalkSpeed *= 5;
			DamageCoefficient = 4f;     //Useless otherwise
			SetBrain(new MinionBrain(this));

			Attributes[GameAttributes.Summoned_By_SNO] = context.PowerSNO;

			Attributes[GameAttributes.Hitpoints_Max] = context.User.Attributes[GameAttributes.Hitpoints_Max_Total];
			Attributes[GameAttributes.Hitpoints_Cur] = Attributes[GameAttributes.Hitpoints_Max];

			Attributes[GameAttributes.Damage_Weapon_Min, 0] = context.User.Attributes[GameAttributes.Damage_Weapon_Min_Total] * damageMult;
			Attributes[GameAttributes.Damage_Weapon_Delta, 0] = context.User.Attributes[GameAttributes.Damage_Weapon_Delta_Total] * damageMult;
			Attributes[GameAttributes.Attacks_Per_Second] = 1.0f;
			Attributes[GameAttributes.Pet_Type] = 0x8;

			LifeTime = lifeTime;
		}
	}

	class AvatarRanged : Minion
	{
		public AvatarRanged(World world, PowerContext context, int AvatarID, float damageMult, TickTimer lifeTime)
			: base(world, ActorSno._x1_crusader_phalanxarcher, context.User, null)
		{
			Scale = 1f;
			WalkSpeed *= 5;
			DamageCoefficient = 4f;         //Useless otherwise
			SetBrain(new MinionBrain(this));
			(Brain as MinionBrain).AddPresetPower(369807);

			Attributes[GameAttributes.Summoned_By_SNO] = context.PowerSNO;

			Attributes[GameAttributes.Hitpoints_Max] = context.User.Attributes[GameAttributes.Hitpoints_Max_Total];
			Attributes[GameAttributes.Hitpoints_Cur] = Attributes[GameAttributes.Hitpoints_Max];

			Attributes[GameAttributes.Damage_Weapon_Min, 0] = context.User.Attributes[GameAttributes.Damage_Weapon_Min_Total] * damageMult;
			Attributes[GameAttributes.Damage_Weapon_Delta, 0] = context.User.Attributes[GameAttributes.Damage_Weapon_Delta_Total] * damageMult;
			Attributes[GameAttributes.Attacks_Per_Second] = 1.0f;
			Attributes[GameAttributes.Pet_Type] = 0x8;

			LifeTime = lifeTime;
		}
	}
}
