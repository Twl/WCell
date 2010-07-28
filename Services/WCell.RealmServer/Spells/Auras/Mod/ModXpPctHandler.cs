﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WCell.RealmServer.Entities;

namespace WCell.RealmServer.Spells.Auras.Handlers
{
    public class ModXpPctHandler : AuraEffectHandler
    {
        protected override void Apply()
        {
            Character chr = m_aura.Auras.Owner as Character;
            if (chr != null)
                chr.ExperienceGainModifierPercent += EffectValue;
        }

        protected override void Remove(bool cancelled)
        {
            Character chr = m_aura.Auras.Owner as Character;
            if (chr != null)
                chr.ExperienceGainModifierPercent -= EffectValue;
        }

    }
};