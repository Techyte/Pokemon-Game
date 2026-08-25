using Riptide;

namespace PokemonGame.Networking
{
    using Battle;
    
    public class MessageHandlers
    {
        [MessageHandler((ushort)ClientToServerMessageId.ConnectionInfo)]
        private static void ServerConnectionInfo(ushort fromPlayerId, Message message)
        {
            BattleNetworkManager.Instance.ServerConnectionInfo(fromPlayerId, message.GetString(), message.GetInt());

        }

        [MessageHandler((ushort)ClientToServerMessageId.AllPartiesReceived)]
        private static void ServerAllPartiesReceived(ushort fromPlayerId, Message message)
        {
            BattleNetworkManager.Instance.ServerAllPartiesReceived();
        }

        [MessageHandler((ushort)ClientToServerMessageId.Action)]
        private static void ServerSwapSelected(ushort fromPlayerId, Message message)
        {
            BattleNetworkManager.Instance.ServerGetClientAction(fromPlayerId, message);
        }
        
        [MessageHandler((ushort)ServerToClientMessageId.UpdatePlayerInfo)]
        private static void ClientConnectionInfo(Message message)
        {
            BattleNetworkManager.Instance.ClientUpdatePlayerInfo(message);
        }
        
        [MessageHandler((ushort)ServerToClientMessageId.PartyInfo)]
        private static void ClientPartyInfo(Message message)
        {
            BattleNetworkManager.Instance.ClientPartyInfo(message.GetUShort(), message.GetParty());
        }
        
        [MessageHandler((ushort)ServerToClientMessageId.StartBattle)]
        private static void ClientStartBattle(Message message)
        {
            BattleNetworkManager.Instance.ClientStartBattle();
        }
        
        [MessageHandler((ushort)ServerToClientMessageId.VisibleActions)]
        private static void ClientTurnPlayerMove(Message message)
        {
            BattleNetworkManager.Instance.ClientGetVisibleBattleActions(message);
        }
    }
}