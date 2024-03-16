﻿using DiIiS_NA.D3_GameServer.Core.Types.SNO;
using DiIiS_NA.GameServer.GSSystem.ActorSystem.Movement;
using System;

namespace DiIiS_NA.GameServer.GSSystem.QuestSystem.QuestEvents.Implementations
{
	class LeahTransformation_Line7 : QuestEvent
	{
		public bool raised = false;

		public LeahTransformation_Line7()
			: base(0)
		{
		}

		public override void Execute(MapSystem.World world)
		{
			var NStone = world.GetActorBySNO(ActorSno._a2dun_zolt_black_soulstone);//156328
			var Adria = world.GetActorBySNO(ActorSno._adria_event47);

			StartConversation(world, 195743);
			//Камера переходит к камню
			foreach (var plr in world.Players.Values)
				plr.InGameClient.SendMessage(new MessageSystem.Message.Definitions.Camera.CameraFocusMessage() { ActorID = (int)NStone.DynamicID(plr), Duration = 1f, Snap = false });
			//Адрия идёт к камню
			float facingAngle = MovementHelpers.GetFacingAngle(Adria, NStone);
			Adria.Move(new Core.Types.Math.Vector3D(NStone.Position.X + 5f, NStone.Position.Y + 5f, NStone.Position.Z), facingAngle);
		}

		private bool StartConversation(MapSystem.World world, Int32 conversationId)
		{
			foreach (var player in world.Players)
			{
				player.Value.Conversations.StartConversation(conversationId);
			}
			return true;
		}

	}
}
