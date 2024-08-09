using EC_Experiments.Common;
using EC_Experiments.Core;
using Terraria;
using Terraria.ModLoader;

public class JumpingNPC : ModNPC {
	public enum ActionPhase {
		Idle,
		Jumping
	}

	public override string Texture => "Terraria/Images/NPC_10";

	public override void SetDefaults() {
		NPC.Size = new Microsoft.Xna.Framework.Vector2(32, 32);
		NPC.aiStyle = -1;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.lifeMax = 10;
	}

	public ActionPhase State { get; set; } = ActionPhase.Idle;
	public const int IdleToActionDelay = 5 * 60;
	
	public float Timer {
		get => NPC.ai[0];
		set => NPC.ai[0] = value;
	}

	public readonly JumpNPCComponent.ComponentData Phase1Jump = new JumpNPCComponent.ComponentData() {
		TimeBeforeExecution = 3,
		Height = 10,
		Distance = 5,
		TimeAfterExecution = 5,
		StopAfterJump = true,
		OnDisabled = new System.Action<NPC>(npc => {
			(npc.ModNPC as JumpingNPC).State = ActionPhase.Idle;
			Main.NewText($"switched to {(npc.ModNPC as JumpingNPC).State}");
		})
	};

	public override void AI() {

		if (State == ActionPhase.Idle) {
			Timer++;
		}

		if (Timer == IdleToActionDelay) {
			Timer = 0;
			State = ActionPhase.Jumping;
			NPC.direction = Main.rand.NextFromList([-1, 1]);
			NPC.EnableComponent<JumpNPCComponent>().Data = Phase1Jump;
		}
	}
}