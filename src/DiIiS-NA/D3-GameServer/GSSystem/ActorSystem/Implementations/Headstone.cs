﻿
using System.Linq;
using DiIiS_NA.D3_GameServer.Core.Types.SNO;
using DiIiS_NA.GameServer.Core.Types.TagMap;
using DiIiS_NA.GameServer.GSSystem.MapSystem;
using DiIiS_NA.GameServer.MessageSystem;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.World;

namespace DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations
{
	[HandledSNO(ActorSno._playerheadstone /* PlayerHeadstone.acr */)]
	class Headstone : Gizmo
	{
		public int playerIndex { get; set; }

		public Headstone(World world, ActorSno sno, TagMap tags, int playerIndex = -1)
			: base(world, sno, tags)
		{
			this.playerIndex = playerIndex;
			Attributes[GameAttributes.MinimapActive] = true;
			Attributes[GameAttributes.Headstone_Player_ANN] = 1;
			Attributes[GameAttributes.TeamID] = 1;
			if (World.Game.PvP) Attributes[GameAttributes.Disabled] = true;
		}


		public override bool Reveal(PlayerSystem.Player player)
		{
			if (!base.Reveal(player))
				return false;
			return true;
		}

		public override void OnTargeted(PlayerSystem.Player player, TargetMessage message)
		{
			base.OnTargeted(player, message);
			if (playerIndex > -1)
				GetPlayersInRange(100f).Where(p => p.PlayerIndex == playerIndex).First().Resurrect();
			//this.Destroy();
		}
	}
}
