using Terraria;
using Terraria.ModLoader;

namespace EC_Experiments.Core;

public class NPCComponent : GlobalNPC {
	public override bool InstancePerEntity { get; } = true;

	public bool Enabled { get; protected set; }

	public virtual void OnEnabled(NPC npc) { }

	public virtual void OnDisabled(NPC npc) { }

	public void SetEnabled(NPC npc, bool value) {
		if (Enabled == value) {
			return;
		}

		Enabled = value;

		if (!Enabled) {
			OnDisabled(npc);
			return;
		}
		
		OnEnabled(npc);
	}
}