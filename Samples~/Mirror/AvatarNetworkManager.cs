// --------------------------------------------------
// Author:      Takayoshi Hagiwara (NITech)
// Created:     2026/7/15
// Summary:     Manages network synchronization of avatar movements.
//              Use this instead of Mirror's NetworkManager.
// --------------------------------------------------

using Mirror;

namespace TH.Utils.Avatar
{
    public sealed class AvatarNetworkManager : NetworkManager
    {
        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<AvatarMotionMessage>(OnReceiveMotionFromClient, requireAuthentication: false);
        }

        private static void OnReceiveMotionFromClient(NetworkConnectionToClient sender, AvatarMotionMessage message)
        {
            // Forward the motion received from the client to all clients
            NetworkServer.SendToAll(message, Channels.Unreliable);
        }
    }
}