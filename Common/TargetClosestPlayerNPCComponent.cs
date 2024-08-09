using EC_Experiments.Core;
using Terraria;

namespace EC_Experiments.Common;

public class TargetClosestPlayerNPCComponent : NPCComponent {
	public class ComponentData {
		public float DetectionRange { get; set; }
	}

	public ComponentData Data { get; set; } = new();

	public override void AI(NPC npc) {
		if (!Enabled) {
			return;
		}

		float currentDistance = Data.DetectionRange;
		foreach (Player player in Main.ActivePlayers) {
			if (player.npcTypeNoAggro[npc.type]) {
				continue;
			}
			if (npc.Center.Distance(player.Center) + player.aggro < currentDistance) {
				npc.target = player.whoAmI;
			}
		}
	}
}