﻿using DiIiS_NA.D3_GameServer.Core.Types.SNO;
using DiIiS_NA.GameServer.Core.Types.TagMap;
using DiIiS_NA.GameServer.GSSystem.PlayerSystem;
using DiIiS_NA.GameServer.MessageSystem;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.Animation;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.Base;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.World;
using DiIiS_NA.GameServer.MessageSystem.Message.Fields;
using System;
using System.Threading.Tasks;

namespace DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations
{
    [HandledSNO(
		ActorSno._trdun_cath_wallcollapse_01// trDun_cath_Chandilier_Trap.acr 
	)]
	class CathedralWall : Gizmo
	{
		public CathedralWall(MapSystem.World world, ActorSno sno, TagMap tags)
			: base(world, sno, tags)
		{

		}

		public void ReceiveDamage(Actor source, float damage)
		{
			if (SNO == ActorSno._trout_highlands_goatman_totem_gharbad && World.Game.CurrentSideQuest != 225253) return;

			World.BroadcastIfRevealed(plr => new FloatingNumberMessage
			{
				Number = damage,
				ActorID = DynamicID(plr),
				Type = FloatingNumberMessage.FloatType.White
			}, this);

			Attributes[GameAttributes.Hitpoints_Cur] = Math.Max(Attributes[GameAttributes.Hitpoints_Cur] - damage, 0);
			Attributes.BroadcastChangedIfRevealed();

			if (Attributes[GameAttributes.Hitpoints_Cur] == 0 && !SNO.IsUndestroyable())
				Die(source);
		}

		public void Die(Actor source = null)
		{
			base.OnTargeted(null, null);

			Logger.Trace("Breaked barricade, id: {0}", SNO);

			if (AnimationSet.TagMapAnimDefault.ContainsKey(AnimationSetKeys.DeathDefault))
				World.BroadcastIfRevealed(plr => new PlayAnimationMessage
				{
					ActorID = DynamicID(plr),
					AnimReason = 11,
					UnitAniimStartTime = 0,
					tAnim = new PlayAnimationMessageSpec[]
					{
						new PlayAnimationMessageSpec()
						{
							Duration = 10,
							AnimationSNO = AnimationSet.TagMapAnimDefault[AnimationSetKeys.DeathDefault],
							PermutationIndex = 0,
							AnimationTag = 0,
							Speed = 1
						}
					}

				}, this);

			Attributes[GameAttributes.Deleted_On_Server] = true;
			Attributes[GameAttributes.Could_Have_Ragdolled] = true;
			Attributes[GameAttributes.Attacks_Per_Second] = 1.0f;
			Attributes[GameAttributes.Damage_Weapon_Min, 0] = 5f;
			Attributes[GameAttributes.Damage_Weapon_Delta, 0] = 5f;
			Attributes.BroadcastChangedIfRevealed();

			Task.Delay(1400).ContinueWith(delegate
			{
				World.PowerManager.RunPower(this, 186216);
				Destroy();
			});
		}


		public override void OnTargeted(Player player, TargetMessage message)
		{
			base.OnTargeted(player, message);
			ReceiveDamage(player, 100);
		}
	}
}
