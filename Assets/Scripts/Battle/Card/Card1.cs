﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card1 : EnemyTargetCard
{
	public override bool UseCard()
	{
		if (!base.UseCard()) return false;
		BM.OnAttack((int)values[0], targetEnemy, BM.actCharacter,1);

		return true;
	}
}
