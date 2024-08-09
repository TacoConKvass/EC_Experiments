using EC_Experiments.Core;
using Terraria;

namespace EC_Experiments.Common;

public class StompNPCComponent : NPCComponent {
	public class ComponentData {
		public int Damage { get; set; }
	}

	public ComponentData Data { get; set; } = new();

	public override void AI(NPC npc) {
		if (!Enabled) {
			return;
		}

		if (npc.velocity.Y > 0) {
			npc.damage = Data.Damage;
		}
		else {
			npc.damage = 0;
		}
	}
}