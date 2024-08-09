using EC_Experiments.Common;
using EC_Experiments.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EC_Experiments.ContentTests;

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
		NPC.EnableComponent<TargetClosestPlayerNPCComponent>().Data.DetectionRange = 320;
		NPC.EnableComponent<FacePlayerNPCComponent>();
		NPC.EnableComponent<StompNPCComponent>().Data.Damage = 10;
	}

	public ActionPhase State { get; set; } = ActionPhase.Idle;
	public const int IdleToActionDelay = 5 * 60;
	
	public float Timer {
		get => NPC.ai[0];
		set => NPC.ai[0] = value;
	}

	public readonly JumpNPCComponent.ComponentData Phase1Jump = new JumpNPCComponent.ComponentData() {
		Height = 5,
		Distance = 8,
		HeightVariation = new Core.DataStructures.VariationRange(.8f, 2f),
		DistanceVariation = new Core.DataStructures.VariationRange(1f, 1.5f),
		ResetVelocity = true,
		OnDisabled = new System.Action<NPC>(npc => (npc.ModNPC as JumpingNPC).State = ActionPhase.Idle)
	};

	public override void AI() {

		if (State == ActionPhase.Idle) {
			Timer++;
		}

		if (Timer == IdleToActionDelay) {
			Timer = 0;
			State = ActionPhase.Jumping;
			NPC.EnableComponent<JumpNPCComponent>().Data = Phase1Jump;
		}
	}
}