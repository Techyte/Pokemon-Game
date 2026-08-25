using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Riptide;
using Riptide.Transports.Steam;
using Riptide.Utils;
using Steamworks;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using SteamClient = Riptide.Transports.Steam.SteamClient;

namespace PokemonGame.Networking
{
    using Battle;
    using Game.Party;
    using General;
    using Global;
    using ScriptableObjects;
    
    public enum ClientToServerMessageId : ushort
    {
        ConnectionInfo,
        AllPartiesReceived,
        Action,
    }
    
    public enum ServerToClientMessageId : ushort
    {
        UpdatePlayerInfo,
        PartyInfo,
        StartBattle,
        VisibleActions,
    }
    
    public class BattleNetworkManager : MonoBehaviour
    {
        public static BattleNetworkManager Instance;

        public Server Server;
        public Client Client;
        
        public Dictionary<ushort, NetworkPlayer> Players = new Dictionary<ushort, NetworkPlayer>();
        public int MaxPlayerCount;
        public bool IsHost;

        public bool UseSteam = false;

        // temporary variable, just tracks how many players needed on both sides to start the game
        public int teamSize = 1;

        // tracks if the client has received everyone's party information
        private int _acknowledgedPartyCount;
        
        public event EventHandler OnUpdatePlayerInfo;
        public event EventHandler OnStartedGeneratingParties;
        public event EventHandler OnFinishedGeneratingParties;
        public event EventHandler OnSentPartiesInfo;
        public event EventHandler OnReadyToStartGame;
        
        protected Callback<LobbyEnter_t> LobbyEnter;
        protected Callback<GameLobbyJoinRequested_t> LobbyJoinRequested;
        protected CSteamID LobbyID;
        
        private void Awake()
        {
            Instance = this;
#if UNITY_EDITOR
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
            RiptideLogger.Initialize(Debug.Log, true);
#endif
            
            LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
            LobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        }
        
        private void SubscribeToClientEvents()
        {
            Client.Connected += ClientOnConnected;
            Client.Disconnected += ClientOnDisconnected;
        }

        private void UnSubscribeToClientEvents()
        {
            if (Client == null)
            {
                return;
            }
            Client.Connected -= ClientOnConnected;
            Client.Disconnected -= ClientOnDisconnected;
        }
        
        private void SubscribeToServerEvents()
        {
            Server.ClientDisconnected += ServerOnClientDisconnected;
        }

        private void UnSubscribeToServerEvents()
        {
            if (Server == null)
            {
                return;
            }
            Server.ClientDisconnected -= ServerOnClientDisconnected;
        }
        
        private void ClientOnConnected(object sender, EventArgs e)
        {
            ClientSendBasicInfo();
        }

        private void ClientOnDisconnected(object sender, DisconnectedEventArgs e)
        {
            Players.Clear();
            SceneManager.LoadScene("Multiplayer");
        }

        private void ServerOnClientDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            Players.Remove(e.Client.Id);
            ServerSendPlayersInfo();
        }
        
        public void StartHosting()
        {
            if (UseSteam)
            {
                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
                IsHost = true;
            }
            else
            {
                Server = new Server();
                SubscribeToServerEvents();
                Server.Start(7777, 2);
                MaxPlayerCount = Server.MaxClientCount;
                IsHost = true;
            }
        }
        
        private void OnLobbyEnter(LobbyEnter_t callback)
        {
            SteamServer steamServer = new SteamServer();
            if (IsHost)
            {
                Server = new Server(steamServer);
                SubscribeToServerEvents();
                Server.Start(0, 2);
                MaxPlayerCount = Server.MaxClientCount;
            }

            LobbyID = (CSteamID)callback.m_ulSteamIDLobby;
            
            CSteamID hostId = SteamMatchmaking.GetLobbyOwner(LobbyID);

            SteamClient steamClient = new SteamClient(steamServer);
            Client = new Client(steamClient);
            
            JoinGame(hostId.ToString(), SteamFriends.GetPersonaName());
        }

        private void OnLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("On lobby join requested");
            if (!IsHost)
            {
                SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
            }
        }
        
        public void JoinGame(string address, string username)
        {
            if (Client == null)
            {
                Client = new Client();
            }
            
            SubscribeToClientEvents();

            if (Client.IsConnected)
            {
                return;
            }
            
            NetworkPlayer player = new NetworkPlayer();
            player.Id = 0;
            player.Username = username;
            player.Pfp = Random.Range(1, 810);
            
            Players.Add(player.Id, player);
            
            Client.Connect(address);
        }

        public void StartGame()
        {
            OnStartedGeneratingParties?.Invoke(this,  EventArgs.Empty);
            GenerateParty(GetPotentialBattlers());
        }

        private List<BattlerTemplate> GetPotentialBattlers()
        {
            List<BattlerTemplate> allTemplates = 
                Resources.LoadAll<BattlerTemplate>("Pokemon Game/Battler Template").ToList();

            List<BattlerTemplate> potentialBattlers = new List<BattlerTemplate>();

            foreach (var potentialBattler in allTemplates)
            {
                bool valid = potentialBattler.GetEvolutions().possibleEvolutions.Count == 0;
                
                // no more funny man :(
                if (potentialBattler.name == "Sans")
                {
                    valid = false;
                }
                
                if (valid)
                {
                    potentialBattlers.Add(potentialBattler);
                }
            }

            return potentialBattlers;
        }

        private void GenerateParty(List<BattlerTemplate> potentialBattlers)
        {
            int start = DateTime.UtcNow.Millisecond;
            
            foreach (var player in Players.Values)
            {
                player.Party = GenerateParty(50, potentialBattlers);
            }
            
            int end = DateTime.UtcNow.Millisecond;
            
            Debug.Log($"Doing party generation took {end-start}ms");
            OnFinishedGeneratingParties?.Invoke(this,  EventArgs.Empty);
            // we don't send it to the host so they have already "acknowledged" the party generation
            _acknowledgedPartyCount = 1;
            
            if (_acknowledgedPartyCount >= MaxPlayerCount)
            {
                StartCoroutine(StartBattle());
                OnReadyToStartGame?.Invoke(this, EventArgs.Empty);
                ServerSendStartBattle();
            }
            
            foreach (var player in Players.Values)
            {
                ServerSendPartyInfoFor(player);
            }
            
            OnSentPartiesInfo?.Invoke(this, EventArgs.Empty);
        }
        
        private IEnumerator StartBattle()
        {
            List<NetworkPlayer> players = Players.Values.ToList();
            int assigned = 0;
            int teamToAssign = 1;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Team = teamToAssign;
                assigned++;
                if (assigned >= teamSize)
                {
                    assigned = 0;
                    teamToAssign++;
                }
            }
            
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "online", true },
                { "trainerBattle", false},
                { "battlersEach", 1}
            };
            
            Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyClose"));
            
            yield return new WaitForSeconds(0.4f);
            
            SceneLoader.LoadScene("Battle", vars);
        }
        
        private Party GenerateParty(int level, List<BattlerTemplate> potential)
        {
            Party partyToReturn = new Party();
            
            for (int i = 0; i < 6; i++)
            {
                BattlerTemplate template = potential[Random.Range(0, potential.Count)];
                
                Battler battler = Battler.Init(template, level, template.name, new List<Move>(), true);
                List<Move> moves = battler.source.GetPossibleMoves().levelup.Keys.ToList();
                
                for (int j = 0; j < 4; j++)
                {
                    battler.LearnMove(moves[Random.Range(0, moves.Count)]);
                }
                
                partyToReturn.Add(battler);
            }

            return partyToReturn;
        }
        
        private void ServerSendPlayersInfo()
        {
            Debug.Log("Server broadcasting new basic info");
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.UpdatePlayerInfo);
            message.AddInt(MaxPlayerCount);
            message.AddInt(Players.Count);

            foreach (var playerInfoBeingSent in Players.Values)
            {
                message.AddUShort(playerInfoBeingSent.Id);
                message.AddString(playerInfoBeingSent.Username);
                message.AddInt(playerInfoBeingSent.Pfp);
            }
            
            Server.SendToAll(message, Client.Id);
            OnUpdatePlayerInfo?.Invoke(this, EventArgs.Empty);
        }

        private void ServerSendPartyInfoFor(NetworkPlayer player)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.PartyInfo);
            message.AddUShort(player.Id);
            message.AddParty(player.Party);
            
            Server.SendToAll(message, Client.Id);
        }

        private void ServerSendStartBattle()
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.StartBattle);
            
            Server.SendToAll(message, Client.Id);
        }
        
        private void ClientSendBasicInfo()
        {
            Debug.Log("Client sending basic info");
            if (Players.TryGetValue(0, out NetworkPlayer player))
            {
                Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.ConnectionInfo);
                message.AddString(player.Username);
                message.AddInt(player.Pfp);
                
                Client.Send(message);
            }
            
            Players.Clear();
        }

        private void ClientSendAllPartiesReceived()
        {
            _acknowledgedPartyCount = 0;
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.AllPartiesReceived);
            
            Client.Send(message);
        }
        
        private void FixedUpdate()
        {
            if (UseSteam && !SteamManager.Initialized)
            {
                return;
            }
            
            if (Server != null)
            {
                Server.Update();
            }

            if (Client != null)
            {
                Client.Update();
            }
        }
        
        private void OnDisable()
        {
            if (Server != null)
            {
                Server.Stop();
            }
            
            if (Client != null)
            {
                Client.Disconnect();
            }
            
            UnSubscribeToClientEvents();
            UnSubscribeToServerEvents();
        }
        
        public void ServerConnectionInfo(ushort fromPlayerId, string username, int pfp)
        {
            Debug.Log("Server received basic info");
            
            NetworkPlayer player = new NetworkPlayer();
            player.Id = fromPlayerId;
            player.Username = username;
            player.Pfp = pfp;
            
            Players.Add(player.Id, player);
            ServerSendPlayersInfo();
        }
        
        public void ServerAllPartiesReceived()
        {
            _acknowledgedPartyCount++;
            
            if (_acknowledgedPartyCount >= MaxPlayerCount)
            {
                StartCoroutine(StartBattle());
                OnReadyToStartGame?.Invoke(this, EventArgs.Empty);
                ServerSendStartBattle();
            }
        }

        public void ClientUpdatePlayerInfo(Message message)
        {
            MaxPlayerCount = message.GetInt();
            int length = message.GetInt();

            for (int i = 0; i < length; i++)
            {
                ushort id = message.GetUShort();
                string username = message.GetString();
                int pfp = message.GetInt();
                if (Players.TryGetValue(id, out NetworkPlayer player))
                {
                    player.Username = username;
                    player.Pfp = pfp;
                }
                else
                {
                    player = new NetworkPlayer();
                    player.Id = id;
                    player.Username = username;
                    player.Pfp = pfp;
                    
                    Players.Add(player.Id, player);
                }
            }

            OnUpdatePlayerInfo?.Invoke(this, EventArgs.Empty);
        }

        public void ClientPartyInfo(ushort player, Party party)
        {
            if (Players.TryGetValue(player, out NetworkPlayer playerInfo))
            {
                playerInfo.Party = party;
            }

            _acknowledgedPartyCount++;
            if (_acknowledgedPartyCount == MaxPlayerCount)
            {
                ClientSendAllPartiesReceived();
            }
        }

        public void ClientStartBattle()
        {
            StartCoroutine(StartBattle());
        }

        public void ServerSendVisibleBattleActions(List<VisibleBattleAction> visibleBattleActions)
        {
            Message message =  Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.VisibleActions);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(visibleBattleActions);
            message.AddString(json);
            
            Server.SendToAll(message, Client.Id);
        }

        public void ClientGetVisibleBattleActions(Message message)
        {
            List<VisibleBattleAction> actions =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<VisibleBattleAction>>(message.GetString());

            Battle.Singleton.ReceiveVisibleActions(actions);
        }

        public void ServerGetClientAction(ushort fromClientId, Message message)
        {
            int playerId = Battle.Singleton.GetPlayerIndexFromNetworkId(fromClientId);
            int actionIndex = message.GetInt();
            
            BattleAction action = Newtonsoft.Json.JsonConvert.DeserializeObject<BattleAction>(message.GetString());
            
            Battle.Singleton.PickPlayerAction(playerId, actionIndex, action);
        }
        
        public void ClientSendBattleAction(int actionIndex, BattleAction action)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.Action);
            message.AddInt(actionIndex);
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(action);
            message.AddString(json);
            
            Client.Send(message);
        }
    }
    
    public class NetworkPlayer
    {
        /// <summary>
        /// NETWORK ID, not player index, may go beyond bounds of max players
        /// </summary>
        public ushort Id;
        public string Username;
        public int Pfp;
        public Party Party;

        /// <summary>
        /// Only set just before the battle begins, generally based on join order
        /// </summary>
        public int Team;
    }
}