﻿using DiIiS_NA.Core.Logging;
using DiIiS_NA.GameServer.GSSystem.ActorSystem;
using DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations.Hirelings;
using DiIiS_NA.GameServer.GSSystem.GameSystem;
using DiIiS_NA.GameServer.GSSystem.PlayerSystem;
using DiIiS_NA.GameServer.MessageSystem;
using System.Linq;
using System;
using System.Collections.Generic;
using DiIiS_NA.Core.Extensions;
using DiIiS_NA.LoginServer.AccountsSystem;
using DiIiS_NA.GameServer.GSSystem.QuestSystem.QuestEvents;
using DiIiS_NA.GameServer.Core.Types.Math;
using DiIiS_NA.Core.Helpers.Math;
using DiIiS_NA.D3_GameServer.Core.Types.SNO;

namespace DiIiS_NA.GameServer.GSSystem.QuestSystem.QuestEvents.Implementations
{
	class SpawnSouls : QuestEvent
	{

		private static readonly Logger Logger = LogManager.CreateLogger();

		public SpawnSouls()
			: base(151125)
		{
		}

		List<Vector3D> ActorsVector3D = new List<Vector3D> { }; //We fill this with the vectors of the actors that transform, so we spwan zombies in the same location.
		List<uint> monstersAlive = new List<uint> { }; //We use this for the killeventlistener.

		public override void Execute(MapSystem.World world)
		{
			//if (world.Game.Empty) return;
			//Logger.Debug("SpawnSouls event started");
			var spot1 = world.GetActorBySNO(ActorSno._trdun_skeleton_d_3);
			while (spot1 != null)
			{
				ActorsVector3D.Add(spot1.Position);
				spot1.Destroy();
				spot1 = world.GetActorBySNO(ActorSno._trdun_skeleton_d_3);
			}
			var spot2 = world.GetActorBySNO(ActorSno._trdun_skeleton_b_2);
			while (spot2 != null)
			{
				ActorsVector3D.Add(spot2.Position);
				spot2.Destroy();
				spot2 = world.GetActorBySNO(ActorSno._trdun_skeleton_b_2);
			}
			var spot3 = world.GetActorBySNO(ActorSno._trdun_skeleton_c_4);
			while (spot3 != null)
			{
				ActorsVector3D.Add(spot3.Position);
				spot3.Destroy();
				spot3 = world.GetActorBySNO(ActorSno._trdun_skeleton_c_4);
			}

			for (int i = 0; i < 6; i++)
			{
				var randPos = ActorsVector3D.PickRandom();
				world.SpawnMonster(ActorSno._ghost_jail_prisoner, randPos);
				ActorsVector3D.Remove(randPos);
			}
		}

	}
}
