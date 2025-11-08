using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "SceneLoadRequest")]
	public sealed class SceneLoadRequestGameEventListener : BaseGameEventListener<SceneLoadRequest, SceneLoadRequestGameEvent, SceneLoadRequestUnityEvent>
	{
	}
}