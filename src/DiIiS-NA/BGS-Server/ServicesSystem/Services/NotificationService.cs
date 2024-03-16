﻿using System;
using bgs.protocol;
using bgs.protocol.notification.v1;
using DiIiS_NA.Core.Logging;
using DiIiS_NA.GameServer.CommandManager;
using DiIiS_NA.LoginServer.AccountsSystem;
using DiIiS_NA.LoginServer.Base;
using DiIiS_NA.LoginServer.Helpers;
using Google.ProtocolBuffers;

namespace DiIiS_NA.LoginServer.ServicesSystem.Services
{
    [Service(serviceID: 0xa, serviceName: "bnet.protocol.notification.NotificationService")]
    public class NotificationService : bgs.protocol.notification.v1.NotificationService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public override void Publish(IRpcController controller, PublishRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SendNotification(IRpcController controller, Notification request, Action<NoData> done)
        {

            switch (request.GetNotificationType())
            {
                case NotificationTypeHelper.NotificationType.Whisper:

                    var targetAccount = GameAccountManager.GetAccountByPersistentID(request.TargetId.Low);
                    Logger.Trace(string.Format("NotificationRequest.Whisper by {0} to {1}", ((HandlerController) controller).Client.Account.GameAccount, targetAccount));

                    if (targetAccount.LoggedInClient == null) return;

                    if (targetAccount == ((HandlerController) controller).Client.Account.GameAccount)
                        CommandManager.TryParse(request.AttributeList[0].Value.StringValue, ((HandlerController) controller).Client); // try parsing it as a command and respond it if so.
                    else
                    {
                        var notification = Notification.CreateBuilder(request)
                            .SetSenderId(((HandlerController) controller).Client.Account.GameAccount.BnetEntityId)
                            .SetSenderAccountId(((HandlerController) controller).Client.Account.BnetEntityId)
                            .Build();

                        targetAccount.LoggedInClient.MakeRpc((lid) =>
                            NotificationListener.CreateStub(targetAccount.LoggedInClient).OnNotificationReceived(controller, notification, callback => { }));
                    }
                    break;
                default:
                    Logger.Warn("Unhandled notification type: {0}", request.Type);
                    break;
            }
            //*/
            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Subscribe(IRpcController controller, SubscribeRequest request, Action<NoData> done)
        {
            //throw new NotImplementedException();
            var builder = NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Unsubscribe(IRpcController controller, UnsubscribeRequest request, Action<NoData> done)
        {
            var builder = NoData.CreateBuilder();
            done(builder.Build());
            //throw new NotImplementedException();
        }
    }
}
