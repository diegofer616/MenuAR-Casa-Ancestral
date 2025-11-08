using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "SceneLoadRequestGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "LoadScene",
	    order = 120)]
	public sealed class SceneLoadRequestGameEvent : GameEventBase<SceneLoadRequest>
	{
	}
}