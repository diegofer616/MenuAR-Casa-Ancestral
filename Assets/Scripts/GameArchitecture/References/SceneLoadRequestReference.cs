using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class SceneLoadRequestReference : BaseReference<SceneLoadRequest, SceneLoadRequestVariable>
	{
	    public SceneLoadRequestReference() : base() { }
	    public SceneLoadRequestReference(SceneLoadRequest value) : base(value) { }
	}
}