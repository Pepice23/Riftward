# Riftward - Project Plan

**Title:** Riftward
**Created:** November 15, 2025
**Updated:** November 17, 2025
**Status:** Active Development - Core Loop Implementation

---

## Core Identity

**Genre:** Vampire Survivors-style auto-battler with high fantasy theme

**Visual Style:**
- Stylized, cartoony aesthetic (WoW-inspired)
- Bright and colorful (NOT dark/grimdark)
- AI-generated assets using Flux model in SwarmUI
- 2D illustrated style OR low-poly 3D (NOT pixel art)

**Gameplay Philosophy:**
- Accessible progression (not reflex-heavy)
- Focus on build choices over twitch reactions
- "One more run" loop with satisfying progression
- Run-based structure with meta progression

---

## Current Progress

### âœ… Completed Systems

**Foundation (Phase 1-2):**
- âœ… Project setup with Godot 4.5 + C#
- âœ… Player movement (WASD controls, sprite direction)
- âœ… Enemy spawning system with basic AI
- âœ… Auto-attack combat (projectile-based)
- âœ… Collision detection and damage

**Core Loop (Phase 3):**
- âœ… XP system with scaling requirements
- âœ… Level up triggers and UI pause
- âœ… Health system with damage flash feedback
- âœ… Class selection system (Paladin, Mage, Hunter)
- âœ… Signal-based UI architecture (HUD + LevelUpUI)

**Code Quality:**
- âœ… Player.cs refactored and organized with regions
- âœ… Separated UI into dedicated components (Hud.cs, LevelUpUi.cs)
- âœ… Signal connections with proper cleanup (_ExitTree)
- âœ… ~100 lines removed from Player.cs while maintaining functionality

### đŸ"„ In Progress

- Level-up upgrade selection (UI shows, needs upgrade data structure)
- Upgrade application system

### 📋 Next Up

**Immediate (Phase 4):**
- Define upgrade data structure
- Implement 3-4 starter upgrades per class
- Apply upgrades to player stats/abilities
- Test full level-up loop with actual effects

**Soon (Phase 4-5):**
- Refined enemy spawning (waves, difficulty scaling)
- Different enemy types
- Run timer and win condition (15 min survival)
- Class-specific starting weapons/abilities

---

## What Makes This Mine

This is the fantasy VS-like I would want to play:
- Uses the high fantasy aesthetic I already love
- Features distinct classes with unique abilities
- Bright, fun tone instead of horror/gothic
- Scratches my personal itch for this genre

**Personal motivation:** Building what I'd enjoy playing and creating, not trying to prove something or chase trends.

---

## What We're NOT Doing

**Explicit scope boundaries:**
- âŒ Pixel art (it's exhausting for me)
- âŒ Dark/horror themes
- âŒ Overly complex economy systems
- âŒ Trying to be revolutionary - just making something fun and solid
- âŒ Starting without a plan

---

## Technical Approach

**Engine:** Godot 4.5 with C# (.NET)

**Why C#:**
- Language familiarity and structure help me understand what I'm building
- Better tooling (VS Code, Rider)
- Strong typing keeps me from getting lost as complexity grows
- Access to .NET ecosystem
- I can reach 90%+ code confidence instead of 70%

**Development Philosophy:**
- Build to understand, not just to finish
- Prioritize visible progress (health bars > bugfixes for motivation)
- Small daily wins
- Ask for help when stuck, but understand the solutions

---

## Core Game Loop

**Basic Run Structure:** *(Mostly Implemented)*
- âœ… Player controls a character (class-based)
- âœ… Enemies spawn continuously
- âœ… Auto-attacking with projectile weapons
- âœ… Level up triggers and pauses game
- ⏳ Choose upgrades (UI ready, needs upgrade data)
- ⏳ Run ends after X minutes or death (death works, timer pending)
- 📋 Meta progression unlocks between runs (future scope)

**Classes:** *(3 Implemented)*
- âœ… **Paladin** - Tank archetype (sprites + switching implemented)
- âœ… **Mage** - Spellcaster archetype (sprites + switching implemented)
- âœ… **Hunter** - Ranged archetype (sprites + switching implemented)
- 📋 **Rogue** - Melee/stealth archetype (potential 4th class)

*Note: All classes currently share base stats/attacks. Class-specific abilities pending.*

**Upgrade System:** *(UI Complete, Data Pending)*
- âœ… UI shows on level up with 3 upgrade buttons
- ⏳ Define upgrade data structure
- ⏳ Create upgrade pools (start class-agnostic, then specialize)
- ⏳ Apply selected upgrades to player stats
- Examples to implement:
  - Damage boost (+10% projectile damage)
  - Fire rate (+attack speed)
  - Movement speed (+15% speed)
  - Health boost (+20 max HP)
  - Multi-shot (fire multiple projectiles)

**Meta Progression:** *(Future Scope)*
- 📋 Unlock new classes
- 📋 Permanent stat boosts
- 📋 New starting abilities
- *(To define after core loop is solid)*

---

## Development Milestones

**Week 1: Foundation** âœ… COMPLETE
- âœ… Define core identity
- âœ… Establish scope boundaries
- âœ… Install Godot 4.5 with .NET support
- âœ… Create project and player movement
- âœ… Basic enemy spawning and combat

**Week 2: Core Loop** âœ… COMPLETE
- âœ… XP and level-up system
- âœ… Health and damage
- âœ… UI foundation (HUD, LevelUpUI)
- âœ… Class selection system
- âœ… Code refactoring and organization

**Week 3: Upgrades** ⏳ IN PROGRESS
- ⏳ Upgrade data structure
- ⏳ Implement starter upgrades
- ⏳ Apply upgrades to player
- 📋 Test full progression loop

**Week 4+: Polish & Expand**
- 📋 Enemy variety and waves
- 📋 Run timer and win condition
- 📋 Class-specific abilities
- 📋 Visual polish and effects

---

## Development Principles

1. **Understanding over speed** - Build slower but comprehend every piece
2. **Visible progress** - Structure work to see results frequently
3. **No AI dependency** - Use AI for lookup/translation, not to outsource thinking
4. **Sustainable pace** - Rest when tired, don't push through burnout
5. **Personal connection** - Keep the "why I want this" visible

---

## Next Steps

**Immediate Priority:**
1. Define upgrade data structure (class/struct to hold upgrade info)
2. Create 4-5 simple upgrades (damage, speed, health, fire rate)
3. Wire upgrades to LevelUpUI buttons
4. Apply upgrade effects when selected
5. Test: Kill enemies â†' gain XP â†' level up â†' choose upgrade â†' see effect

**After Upgrades Work:**
1. Add run timer (15-minute survival goal)
2. Improve enemy spawning (waves, difficulty scaling)
3. Create 2-3 enemy types with different behaviors
4. Add visual polish (damage numbers, particle effects)
5. Implement class-specific starting abilities

**Code Quality Goals:**
- Continue refactoring as systems grow
- Keep Player.cs manageable (consider weapon system extraction)
- Document complex systems
- Add signal cleanup wherever needed

---

## Notes to Self

- This is about making something I'd enjoy, not proving I'm "production ready"
- The Zafi game worked because it had personal meaning
- The Tower got boring when it became about checking boxes
- C#'s structure helps me understand better than GDScript's flexibility
- I have the skills - I need the plan and the personal connection
- Monday start is nice, not mandatory
- Rest is productive too

---

**Remember:** Small wins compound. Build something you understand. Make what you'd want to play.

---

## Recent Session Notes

**November 17, 2025 - Refactoring & Signals:**
- Refactored Player.cs, removed ~100 lines while keeping functionality
- Separated UI into Hud.cs and LevelUpUi.cs
- Implemented proper signal architecture (XPUpdated, GamePaused, class selection)
- Added signal cleanup with _ExitTree methods
- Fixed type safety with typed exports (LevelUpUi, Hud instead of CanvasLayer)
- Learned about signal memory leaks and proper cleanup patterns
- Player.cs now well-organized with regions (Movement, Combat, Health, XP, etc.)

**Key Learning:** Signal connections need cleanup! Always disconnect in _ExitTree what you connect in _Ready.
