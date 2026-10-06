# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.0] - 2026-10-06

Early beta. First public release. It passes every automated check in the build (Papyrus compile, plugin validation, round trip, FOMOD schema) but has not been playtested in-game yet. Please report problems on the Nexus page or on GitHub.

### Added
- Lesser power "Swear an Oath", granted after Helgen. It opens the oath menu: swear, renounce, and review your oaths.
- Five oaths, each a vow the game can actually detect without SKSE:
  - Oath of Steel: cast no spell from your hands and never ready a scroll (Health).
  - Oath of the Shield: keep a shield on your arm, with a 3-second grace for swapping shields (Block).
  - Oath of Iron: wear no light armor, light shields included (Armor).
  - Oath of Endurance: no potions, food or poisons while in combat (Stamina).
  - Oath of Close Quarters: never equip a bow (One-Handed and Two-Handed).
- Up to three oaths at once (`SOT_MaxOaths`).
- Blessings grow while an oath holds: Sworn, then Steadfast after 3 days, then Unbroken after 7 days.
- Breaking an oath: the blessing is lost, you become an Oathbreaker (Health and Stamina −30) for 24 hours, and the oath can't be sworn again for 72 hours. Renouncing has the cooldown but no penalty.
- Event-driven detection through the player alias (OnSpellCast, OnObjectEquipped, OnObjectUnequipped) and single scheduled game-time updates. No polling loops.
- FOMOD installer with optional Papyrus sources.
