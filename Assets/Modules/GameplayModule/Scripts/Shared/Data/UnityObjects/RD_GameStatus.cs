using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.GameplayModule.Shared.Data.UnityObjects
{
    /// <summary>
    /// Where the game stands during play, read by every module that takes an order. The game is played one action at
    /// a time: while one runs - a unit walking out of its door, walking to a cell, or walking up to strike - the game
    /// is locked, and every module leaves the player's orders unanswered until it ends. Only the gameplay module
    /// locks and unlocks it.
    ///
    /// Filed in the Shared Scriptables of GameplaySystemRoot's adapter; a reader takes it with
    /// ISharedDataModel.GetScriptable&lt;RD_GameStatus&gt;(). Nothing of a session is written into the asset file.
    /// </summary>
    [CreateAssetMenu(fileName = "RD_GameStatus", menuName = "Game/Data/RD_GameStatus")]
    public class RD_GameStatus : ScriptableObject
    {
        /// <summary>Whether an action is running: no order is taken until it ends.</summary>
        [ShowInInspector, ReadOnly]
        [InfoBox("Filled in Play mode.")]
        public bool IsLocked { get; internal set; }
    }
}
