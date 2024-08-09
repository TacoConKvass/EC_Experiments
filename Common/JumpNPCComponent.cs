using EC_Experiments.Core;
using Microsoft.Xna.Framework;
using Terraria;
using System;

namespace EC_Experiments.Common;

public class JumpNPCComponent : NPCComponent {

	public record struct ComponentData {
		public int TimeBeforeExecution { get; set; }
		public float Distance { get; set; }
		public float Height { get; set; }
		public int TimeAfterExecution { get; set; }
		public bool StopBeforeJump { get; set; }
		public bool StopAfterJump { get; set; }
		public Action<NPC>? OnEnabled { get; set; }
		public Action<NPC>? OnDisabled { get; set; }
	}

	public ActionPhases State { get; set; } = ActionPhases.Preparation;

	public ComponentData Data { get; set; } = new ComponentData();

	private bool cachedNoTileCollide;
	private bool cachedNoGravity;
	private int timer;

	public override void AI(NPC npc) {
		if (!Enabled) {
			return;
		}

		switch (State) {
			case ActionPhases.Preparation:
				PrepareJump(npc); 
				break;
			case ActionPhases.Execution:
				ExecuteJump(npc);
				break;
			case ActionPhases.FollowUp:
				FollowUp(npc);
				break;
		}
	}

	public void PrepareJump(NPC npc) {
		if (Data.StopBeforeJump) {
			npc.velocity.X = 0;
		}
		if (timer++ < Data.TimeBeforeExecution * 60) {
			return;
		}

		timer = 0;
		State = ActionPhases.Execution;
	}

	public void ExecuteJump(NPC npc) {
		npc.velocity += new Vector2((Data.Distance / 4 * npc.direction) - npc.velocity.X, -Data.Height - npc.velocity.Y );
		State = ActionPhases.FollowUp;
	}

	public void FollowUp(NPC npc) {
		if (!npc.collideY) {
			return;
		}

		if (Data.StopAfterJump) {
			npc.velocity.X = 0;
		}

		if (timer++ < Data.TimeAfterExecution * 60) {
			return;
		}
		
		SetEnabled(npc, false);
	}

	public override void OnEnabled(NPC npc) {
		Data.OnEnabled?.Invoke(npc);
		cachedNoTileCollide = npc.noTileCollide;
		cachedNoGravity = npc.noGravity;
		timer = 0;
		State = ActionPhases.Preparation;
	}

	public override void OnDisabled(NPC npc) {
		Data.OnDisabled?.Invoke(npc);
		npc.noTileCollide = cachedNoTileCollide;
		npc.noGravity = cachedNoGravity;
	}
}