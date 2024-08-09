using Terraria;

namespace EC_Experiments.Core;

public static class NPCExtensions {
	
	/// <summary>
	/// Enable component
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="npc"></param>
	/// <param name="initializer"></param>
	/// <returns></returns>
	public static T EnableComponent<T>(this NPC npc) where T : NPCComponent {
		T component = npc.GetGlobalNPC<T>();

		component.SetEnabled(npc, true);

		return component;
	}
}