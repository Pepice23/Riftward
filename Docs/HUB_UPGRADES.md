# Hub Upgrade System - Design Document

## Overview

The hub features 3 trainers (one per class) that offer permanent upgrades purchased with gold earned during runs. Upgrades persist across runs and make the player progressively stronger over the 2-3 hour playtime.

## Philosophy

- **"Easy Mode Unlocked"** - Upgrades are powerful (20-30% boosts), not minor tweaks
- **Gradual power curve** - Each upgrade feels meaningful
- **Thematic but accessible** - Trainers represent class fantasy but all upgrades are available to all players
- **Respects player time** - Failed runs still give gold, always making progress

## Hub Structure

### 3 Trainers (One Per Class)
- **Mage Trainer** - Arcane power and spellcasting
- **Paladin Trainer** - Survivability and holy magic
- **Hunter Trainer** - Speed and precision

### Upgrade Types

**Generic Upgrades (Shared):**
- Appear at ALL three trainers
- Purchasing a rank anywhere unlocks it everywhere
- Applied to ALL classes during runs
- Example: Buy "HP Rank 1" from Mage trainer → Shows as purchased at all trainers

**Class-Specific Upgrades (Unique):**
- Only appear at their respective trainer
- Only applied when playing that class
- Example: Projectile upgrades only at Mage trainer, only work when playing Mage

### When Starting a Run
- All generic upgrades are applied
- Class-specific upgrades for chosen class are applied
- Other class upgrades are ignored

---

## Generic Upgrades (4 total)

These appear at ALL trainers and benefit ALL classes.

### 1. Max Health
- **Effect:** +20% Max HP per rank
- **Max Rank:** 4
- **Total at Max:** +80% Max HP
- **Cost per Rank:** 100g, 250g, 500g, 1000g

### 2. Base Damage
- **Effect:** +15% Damage per rank
- **Max Rank:** 4
- **Total at Max:** +60% Base Damage
- **Cost per Rank:** 100g, 250g, 500g, 1000g

### 3. Movement Speed
- **Effect:** +10% Speed per rank
- **Max Rank:** 4
- **Total at Max:** +40% Movement Speed
- **Cost per Rank:** 100g, 250g, 500g, 1000g

### 4. Health Regeneration
- **Effect:** +50% Regen per rank
- **Max Rank:** 3
- **Total at Max:** +150% Health Regen
- **Cost per Rank:** 150g, 350g, 750g

---

## Class-Specific Upgrades

### Mage Trainer (3 upgrades)

#### 1. Projectile Speed
- **Effect:** +20% Projectile Speed per rank
- **Max Rank:** 3
- **Total at Max:** +60% Projectile Speed
- **Cost per Rank:** 200g, 400g, 800g

#### 2. Starting Projectile Count
- **Effect:** +1 Projectile per rank
- **Max Rank:** 3
- **Total at Max:** +3 Starting Projectiles
- **Cost per Rank:** 300g, 600g, 1200g
- **Note:** This is powerful - starts compound with run upgrades

#### 3. Attack Speed
- **Effect:** -10% Attack Cooldown per rank
- **Max Rank:** 3
- **Total at Max:** -30% Attack Cooldown
- **Cost per Rank:** 200g, 400g, 800g

---

### Paladin Trainer (3 upgrades)

#### 1. Aura Radius
- **Effect:** +15% Aura Radius per rank
- **Max Rank:** 3
- **Total at Max:** +45% Aura Radius
- **Cost per Rank:** 200g, 400g, 800g

#### 2. Aura Damage
- **Effect:** +20% Aura Damage per rank
- **Max Rank:** 3
- **Total at Max:** +60% Aura Damage
- **Cost per Rank:** 200g, 400g, 800g

#### 3. Life Leech
- **Effect:** +5% Life Leech per rank
- **Max Rank:** 3
- **Total at Max:** +15% Life Leech
- **Cost per Rank:** 250g, 500g, 1000g
- **Note:** Synergizes with Aura Damage

---

### Hunter Trainer (3 upgrades)

#### 1. Arrow Speed
- **Effect:** +20% Arrow Speed per rank
- **Max Rank:** 3
- **Total at Max:** +60% Arrow Speed
- **Cost per Rank:** 200g, 400g, 800g

#### 2. Pierce
- **Effect:** +1 Enemy Pierced per rank
- **Max Rank:** 3
- **Total at Max:** +3 Pierce
- **Cost per Rank:** 250g, 500g, 1000g
- **Note:** Huge value against grouped enemies

#### 3. Attack Speed
- **Effect:** -10% Attack Cooldown per rank
- **Max Rank:** 3
- **Total at Max:** -30% Attack Cooldown
- **Cost per Rank:** 200g, 400g, 800g

---

## Cost Summary

**Total cost to max all generic upgrades:** 6,300g
**Total cost to max one class's upgrades:** ~6,600g per class
**Total cost to max everything:** ~26,100g

For a 2-3 hour game, players should earn roughly:
- 500-1000g per failed run
- 1500-2500g per successful run
- Aiming for ~15-20 runs total to max out their main class + generics

---

## Balance Notes

### Power Curve
- **First 2000g spent:** Noticeable but still challenging
- **5000g spent:** Comfortable, experimenting with builds
- **10000g+ spent:** Powerful, crushing early game, focusing on late game/boss

### Stacking Interactions
Hub upgrades stack multiplicatively with run upgrades:
- Hub: +60% Projectile Speed, Run: +100% Projectile Speed = 2.6x total
- Hub: +3 Projectiles, Run: +3 Projectiles = 6 extra projectiles total

This creates the "easy mode unlocked" feel without trivializing runs entirely.

### Balancing Levers
If upgrades feel too weak/strong:
- Adjust percentages per rank
- Adjust max ranks
- Adjust costs (make expensive upgrades rarer/cheaper)

---

## Implementation Notes

### Currency System
- Gold drops from enemies during runs
- Gold persists between runs (saved to player data)
- One currency type: Gold

### Data Structure Needed
```
HubUpgrade {
    string Name
    string Description
    UpgradeType Type (Generic or ClassSpecific)
    PlayerClass TargetClass (if ClassSpecific)
    int CurrentRank
    int MaxRank
    int[] CostPerRank
    Action<Player> ApplyEffect
}
```

### Application Flow
1. Player enters hub with accumulated gold
2. Interacts with trainer → Shows upgrade UI
3. Purchase upgrade with gold → Deduct cost, increment rank
4. Start run → Apply all relevant upgrades to player stats
5. Upgrades persist in save data

---

## Future Considerations

### If More Content Needed
- Add 4th "Master Trainer" with advanced/unique upgrades
- Add cosmetic unlocks (skins, effects) alongside stat upgrades
- Add "prestige" system to reset and re-earn with bonus modifiers

### If Balance Issues Arise
- Consider diminishing returns on later ranks
- Add soft caps to prevent runaway scaling
- Adjust enemy scaling to match upgraded player power

---

## Design Philosophy Reminder

This system exists to:
- Make players feel progressively stronger
- Respect player time (always progressing)
- Encourage multiple runs and experimentation
- Create a satisfying power fantasy over 2-3 hours

Keep it simple, make it fun, make it feel good!
