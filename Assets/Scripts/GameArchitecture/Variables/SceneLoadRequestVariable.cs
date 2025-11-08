using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class SceneLoadRequestEvent : UnityEvent<SceneLoadRequest> { }

	[CreateAssetMenu(
	    fileName = "SceneLoadRequestVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "LoadScene",
	    order = 120)]
	public class SceneLoadRequestVariable : BaseVariable<SceneLoadRequest, SceneLoadRequestEvent>
	{
	}
}