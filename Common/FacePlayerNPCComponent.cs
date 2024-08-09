using EC_Experiments.Core;
using Terraria;

namespace EC_Experiments.Common;

public class FacePlayerNPCComponent : NPCComponent {
	public override void AI(NPC npc) {
		if (!Enabled) {
			return;
		}

		npc.direction = Main.player[npc.target].Center.X > npc.Center.X ? 1 : -1;
	}
}