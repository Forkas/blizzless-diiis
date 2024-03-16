﻿using DiIiS_NA.D3_GameServer.Core.Types.SNO;
using DiIiS_NA.GameServer.Core.Types.TagMap;
using DiIiS_NA.GameServer.GSSystem.MapSystem;
using DiIiS_NA.GameServer.GSSystem.PlayerSystem;
using DiIiS_NA.GameServer.MessageSystem;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.ACD;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.Animation;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.World;
using DiIiS_NA.GameServer.MessageSystem.Message.Fields;
using System.Threading.Tasks;

namespace DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations
{
	[HandledSNO(ActorSno._caout_stingingwinds_khamsin_gate)]
	public class Door : Gizmo
	{
		public bool isOpened = false;
		public Portal NearestPortal = null;
		public Door(World world, ActorSno sno, TagMap tags)
			: base(world, sno, tags)
		{
			var Portals = GetObjectsInRange<Portal>(10f);
			if (Portals.Count > 0)
				NearestPortal = Portals[0];
			if (NearestPortal != null)
			{
				NearestPortal.SetVisible(false);
				foreach (var plr in World.Players.Values)
					NearestPortal.Unreveal(plr);
			}
		}

		public override bool Reveal(Player player)
		{
			if (SNO == ActorSno._trout_cultists_summoning_portal_b) return false;
			if (SNO == ActorSno._a2dun_aqd_godhead_door_largepuzzle && World.SNO != WorldSno.a2dun_aqd_oasis_randomfacepuzzle_large) return false; //dakab door
			if (SNO == ActorSno._a2dun_aqd_godhead_door && World.SNO == WorldSno.a2dun_aqd_oasis_randomfacepuzzle_large) return false; //not dakab door

			if (SNO == ActorSno._a2dun_zolt_random_portal_timed) //Treasure Room door
				isOpened = true;

			if (SNO == ActorSno._caout_oasis_mine_entrance_a && (float)DiIiS_NA.Core.Helpers.Math.FastRandom.Instance.NextDouble() < 0.3f) //Mysterious Cave door
				isOpened = true;

			if (!base.Reveal(player))
				return false;

			if (isOpened == true)
			{
				player.InGameClient.SendMessage(new SetIdleAnimationMessage
				{
					ActorID = DynamicID(player),
					AnimationSNO = AnimationSetKeys.Open.ID
				});
			}

			if (NearestPortal == null)
			{
				var Portals = GetObjectsInRange<Portal>(10f);
				if (Portals.Count > 0)
					NearestPortal = Portals[0];
				if (NearestPortal != null)
				{
					NearestPortal.SetVisible(false);
					foreach (var plr in World.Players.Values)
						NearestPortal.Unreveal(plr);
				}
			}
			return true;
		}

		public void Open()
		{
			Logger.MethodTrace($"Opening door $[underline green]${SNO}$[/]$ in world $[underline green]${World.SNO}$[/]$");
			World.BroadcastIfRevealed(plr => new PlayAnimationMessage
			{
				ActorID = DynamicID(plr),
				AnimReason = 5,
				UnitAniimStartTime = 0,
				tAnim = new[]
				{
					new PlayAnimationMessageSpec()
					{
						Duration = 500,
						AnimationSNO = AnimationSet.TagMapAnimDefault[AnimationSetKeys.Opening],
						PermutationIndex = 0,
						AnimationTag = 0,
						Speed = 1
					}
				}

			}, this);

			World.BroadcastIfRevealed(plr => new ACDCollFlagsMessage
			{
				ActorID = DynamicID(plr),
				CollFlags = 0
			}, this);

			World.BroadcastIfRevealed(plr => new SetIdleAnimationMessage
			{
				ActorID = DynamicID(plr),
				AnimationSNO = AnimationSetKeys.Open.ID
			}, this);

			Attributes[GameAttributes.Gizmo_Has_Been_Operated] = true;
			//this.Attributes[GameAttribute.Gizmo_Operator_ACDID] = unchecked((int)player.DynamicID);
			Attributes[GameAttributes.Gizmo_State] = 1;
			CollFlags = 0;
			isOpened = true;

			TickerSystem.TickTimer Timeout = new TickerSystem.SecondsTickTimer(World.Game, 1.8f);
			if (NearestPortal != null)
			{
				var nearestPortalOpen = Task<bool>.Factory.StartNew(() => WaitToSpawn(Timeout));
				nearestPortalOpen.ContinueWith(delegate
				{
					NearestPortal.SetVisible(true);
					foreach (var plr in World.Players.Values)
						NearestPortal.Unreveal(plr);
				});
			}

			Attributes.BroadcastChangedIfRevealed();
		}

		public override void OnTargeted(Player player, TargetMessage message)
		{
			if (Attributes[GameAttributes.Disabled]) return;
			Open();
            
			base.OnTargeted(player, message);
			Attributes[GameAttributes.Disabled] = true;
		}

		private bool WaitToSpawn(TickerSystem.TickTimer timer)
		{
			while (timer.TimedOut != true)
			{

			}
			return true;
		}
	}

	//175810

}
