/*************************************************************************
 *
 *   file		: PeriodicDamage.cs
 *   copyright		: (C) The WCell Team
 *   email		: info@wcell.org
 *   last changed	: $LastChangedDate: 2010-01-10 13:00:10 +0100 (s? 10 jan 2010) $
 *   last author	: $LastChangedBy: dominikseifert $
 *   revision		: $Rev: 1185 $
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *************************************************************************/

using System;
using WCell.Constants.Spells;
using WCell.RealmServer.Entities;

namespace WCell.RealmServer.Spells.Auras.Handlers
{
	/// <summary>
	/// Periodically damages the holder
	/// </summary>
	public class PeriodicDamageHandler : AuraEffectHandler
	{
		protected override void Apply()
		{
			var holder = Owner;
			if (holder.IsAlive)
			{
				var value = EffectValue;
				if (m_aura.Spell.Mechanic == SpellMechanic.Bleeding)
				{
					var bonus = m_aura.Auras.GetBleedBonusPercent();
					value += ((value * bonus) + 50) / 100;
					m_aura.Owner.AuraState |= AuraStateMask.Bleeding;
				}

				holder.DealSpellDamage(m_aura.CasterUnit, m_spellEffect, value);
			}
		}
		protected override void Remove(bool cancelled)
		{
			var auras = m_aura.Owner.Auras;
			var reset = false;
			foreach (var aura in auras)
			{
				if (!aura.IsBeneficial && aura.Spell.Mechanic == SpellMechanic.Bleeding)
				{
					if (aura != m_aura)
					{
						reset = false;	//this is another bleed aura, so we don't need to reset the aurastate
						break;			//no need to iterate further
					}
					else
						reset = true;
				}
			}
			if (reset)
				m_aura.Owner.AuraState ^= AuraStateMask.Bleeding;
		}
	}

	public class ParameterizedPeriodicDamageHandler : PeriodicDamageHandler
	{
		public int TotalDamage { get; set; }

		public ParameterizedPeriodicDamageHandler() : this(0)
		{
		}

		public ParameterizedPeriodicDamageHandler(int totalDmg)
		{
			TotalDamage = totalDmg;
		}

		protected override void Apply()
		{
			BaseEffectValue = TotalDamage / (m_aura.TicksLeft + 1);
			TotalDamage -= BaseEffectValue;
		}
	}
}