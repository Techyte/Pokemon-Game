namespace PokemonGame.Global
{
    using ScriptableObjects;
    using UnityEngine;
    
    public static class Registry
    {
        /// <summary>
        /// Gets an item from the items folder using the name you supply
        /// </summary>
        /// <param name="itemName">The name of the item you want to get</param>
        /// <param name="foundItem">The found item</param>
        /// <returns>Whether the item was found</returns>
        public static bool GetItem(string itemName, out Item foundItem)
        {
            Item[] items = Resources.LoadAll<Item>("Pokemon Game/Items");
            foreach (var item in items)
            {
                if (item.name == itemName)
                {
                    foundItem = item;
                    return true;
                }
            }
    
            Debug.LogWarning($"Could not find item {itemName}, returning null");
            foundItem = null;
            return false;
        }
        
        /// <summary>
        /// Gets an ai from the ais folder using the name you supply
        /// </summary>
        /// <param name="aiName">The name of the ai you want to get</param>
        /// <param name="foundAi">The found ai</param>
        /// <returns>Whether the ai was found</returns>
        public static bool GetAI(string aiName, out EnemyAI foundAi)
        {
            EnemyAI[] ais = Resources.LoadAll<EnemyAI>("Pokemon Game/Ais");
            foreach (var ai in ais)
            {
                if (ai.name == aiName)
                {
                    foundAi = ai;
                    return true;
                }
            }
    
            Debug.LogWarning($"Could not find ai {aiName}, returning null");
            foundAi = null;
            return foundAi;
        }
        
        /// <summary>
        /// Gets a move from the moves folder using the name you supply
        /// </summary>
        /// <param name="moveName">The name of the move you want to get</param>
        /// <param name="foundMove">The found move</param>
        /// <returns>Whether the move was found</returns>
        public static bool GetMove(string moveName, out Move foundMove)
        {
            Move[] moves = Resources.LoadAll<Move>("Pokemon Game/Moves");
            foreach (var move in moves)
            {
                if (move.name == moveName)
                {
                    foundMove = move;
                    return true;
                }
            }
    
            Debug.LogWarning($"Could not find move {moveName}, returning null");
            foundMove = null;
            return false;
        }
        
        /// <summary>
        /// Gets a status effect from the status effects folder using the name you supply
        /// </summary>
        /// <param name="effectName">The name of the status effect you want to get</param>
        /// <param name="foundEffect">The found status effect</param>
        /// <returns>Whether the status effect was found</returns>
        public static bool GetStatusEffect(string effectName, out StatusEffect foundEffect)
        {
            StatusEffect[] effects = Resources.LoadAll<StatusEffect>("Pokemon Game/StatusEffects");
            foreach (var effect in effects)
            {
                if (effect.name == effectName)
                {
                    foundEffect = effect;
                    return true;
                }
            }
    
            Debug.LogWarning($"Could not find status effect {effectName}, returning null");
            foundEffect = null;
            return false;
        }
        
        /// <summary>
        /// Gets a type from the type folder using the name you supply
        /// </summary>
        /// <param name="typeName">The name of the type you want to get</param>
        /// <param name="foundType">The found type</param>
        /// <returns>Whether the type was found</returns>
        public static bool GetType(string typeName, out Type foundType)
        {
            Type[] types = Resources.LoadAll<Type>("Pokemon Game/Types");
            foreach (var type in types)
            {
                if (type.name == typeName)
                {
                    foundType = type;
                    return true;
                }
            }
    
            Debug.LogWarning($"Could not find type {typeName}, returning null");
            foundType = null;
            return false;
        }
    }
}