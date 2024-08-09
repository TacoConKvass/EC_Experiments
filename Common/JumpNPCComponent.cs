using EC_Experiments.Core;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using EC_Experiments.Core.DataStructures;

namespace EC_Experiments.Common;

public class JumpNPCComponent : NPCComponent {
	public class ComponentData {
		/// <summary>
		/// Determines whether the NPC should stop before rnning the BeforeExecution timer
		/// </summary>
		public bool StopBeforeJump { get; set; }
		/// <summary>
		/// Time in seconds before executing the jump
		/// </summary>
		public int TimeBeforeExecution { get; set; }
		/// <summary>
		/// Wheteher the jumps velocity should be added to the current velocity
		/// </summary>
		public bool RetainVelocity { get; set; }
		/// <summary>
		/// Default jump distance in tiles
		/// </summary>
		public float Distance { get; set; }
		/// <summary>
		/// How much variation should be introduced to the jump distance
		/// </summary>
		public VariationRange DistanceVariation { get; set; }
		/// <summary>
		/// Default jump height in tiles
		/// </summary>
		public float Height { get; set; }
		/// <summary>
		/// How much variation should be introduced to the jump height
		/// </summary>
		public VariationRange HeightVariation { get; set; }
		/// <summary>
		/// Time in seconds before disabling the component after executing the jump
		/// </summary>
		public int TimeAfterExecution { get; set; }
		/// <summary>
		/// Determines whether the velocity should return to the state before the jump
		/// </summary>
		public bool ResetVelocity { get; set; }
		public Action<NPC>? OnEnabled { get; set; }
		public Action<NPC>? OnDisabled { get; set; }
	}

	public ActionPhases State { get; set; } = ActionPhases.Preparation;

	public ComponentData Data { get; set; } = new();

	private bool cachedNoTileCollide;
	private bool cachedNoGravity;
	private int timer;
	private float distance;

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
		distance = (Data.Distance / 4 * npc.direction) * Data.DistanceVariation.Random;
		npc.velocity = new Vector2(
			distance,
			-Data.Height * Data.HeightVariation.Random
		);
		State = ActionPhases.FollowUp;
	}

	public void FollowUp(NPC npc) {
		if (!npc.collideY) {
			return;
		}

		if (Data.ResetVelocity) {
			npc.velocity.X = Math.Clamp(npc.velocity.X - distance, 0, 0);
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