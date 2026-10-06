# In-game test checklist (v0.1.0, early beta)

v0.1.0 ships as an early beta. It passed every automated build check but hasn't been played in-game yet. This checklist is for players and testers who want to confirm everything works, and for the next round of fixes.

Open the console with `~`. Commands that take a quest or global accept the EditorID directly. `player.addspell` needs a full FormID, which depends on your load order, so look it up first:

```
help "Swear an Oath" 4         -> e.g. SPEL: (FE01581E) 'Swear an Oath'
```

ESL-flagged plugins show up as `FE` + a 3-digit slot (`xxx`) + the record's own 3 digits. The record IDs are:

| Record | Local ID | Full FormID |
|---|---|---|
| Swear an Oath (lesser power) | 81E | `FExxx81E` |
| Blessings, Sworn: Steel, Shield, Iron, Endurance, Close Quarters | 81F to 823 | `FExxx81F` … |
| Blessings, Steadfast | 824 to 828 | `FExxx824` … |
| Blessings, Unbroken | 829 to 82D | `FExxx829` … |
| Oathbreaker | 82E | `FExxx82E` |

Globals: `SOT_ActiveSteel`, `SOT_ActiveShield`, `SOT_ActiveIron`, `SOT_ActiveEndurance`, `SOT_ActiveClose`, `SOT_ActiveCount`, `SOT_OathsBroken`, `SOT_LongestOathDays`, plus the tunables `SOT_MaxOaths`, `SOT_Tier2Days`, `SOT_Tier3Days`, `SOT_PenaltyHours`, `SOT_CooldownHours`.

**Turn on Papyrus logging before testing.** Add this to `Skyrim.ini` (under Proton: `.../compatdata/489830/pfx/drive_c/users/steamuser/Documents/My Games/Skyrim Special Edition/Skyrim.ini`):

```ini
[Papyrus]
bEnableLogging=1
bEnableTrace=1
bLoadDebugInformation=1
```

The log is written to `.../My Games/Skyrim Special Edition/Logs/Script/Papyrus.0.log`. After each session, run `grep -n "SOT_" Papyrus.0.log` and report anything it finds.

Tip: to fast-forward game time, `set timescale to 2000` for a few seconds, then `set timescale to 20`. Or wait/sleep.

---

## 0. Plugin loads
- [ ] `SteelOath.esp` is enabled and the game reaches the main menu.
- [ ] Load a save past Helgen. Within about a minute, the "weight of old vows" message appears and **Swear an Oath** is in Magic > Powers. `sqv SOT_OathQuest` shows the quest running.
- [ ] New game: the power does not arrive during Helgen, and arrives soon after.
- [ ] With the other mods in the series installed, all plugins load.

## 1. Menus
- [ ] Cast the power. The menu shows active 0 / 3, broken 0, longest 0 days, and *Swear an oath / My oaths / Close*. *Renounce* is hidden.
- [ ] *Swear an oath* lists all five oaths and *Back*. Each opens a confirmation with the vow and blessing (days filled in as 3 and 7). *Back* returns.
- [ ] *My oaths* says "You hold no oaths."

## 2. Swearing
- [ ] Swear the **Oath of Steel**. "You swear the Oath of Steel." appears, *Oath of Steel (Sworn)* is in Active Effects (Health +20), and it's gone from the swear list.
- [ ] With no shield, choose **Oath of the Shield**: refused with a message. Equip a shield and it works.
- [ ] Wearing any light armor (or a light shield), **Oath of Iron** is refused. In heavy armor or clothes, it works.
- [ ] With a bow equipped, **Oath of Close Quarters** is refused.
- [ ] With three oaths active, *Swear an oath* is hidden in the main menu.

## 3. Breaking each oath
For each: confirm the "Oath broken: …" notification, the blessing disappears from Active Effects, **Oathbreaker** (Health −30, Stamina −30) appears, and `show SOT_OathsBroken` goes up by one.

- [ ] **Steel**: cast a spell from either hand → broken. Separately: shouting, using a lesser power (e.g. a racial power), using a staff, drinking a potion do **not** break it. Equipping a scroll breaks it.
- [ ] **Shield**: unequip the shield → broken about 3 seconds later. Swapping directly to another shield does **not** break it. Equipping a two-handed weapon (which removes the shield) breaks it.
- [ ] **Iron**: put on a light armor piece → broken. Heavy pieces and clothes don't.
- [ ] **Endurance**: drink a potion or eat food in combat → broken. Outside combat, nothing happens. Also try applying a poison in combat and report whether it broke the oath.
- [ ] **Close Quarters**: equip a bow → broken. Swords, axes, daggers and staves don't.

## 4. Growth, penalty, cooldown
- [ ] `set SOT_Tier2Days to 0.1` and `set SOT_Tier3Days to 0.2`, swear an oath, then wait 3 hours. "The blessing grows: Steadfast" appears and the Active Effects entry updates. Wait 2 more hours: Unbroken. (Set them back to 3 and 7 afterwards.)
- [ ] After breaking an oath, *My oaths* shows the cooldown and the oathbreaker time left. Wait 24 hours: "The oathbreaker's shame fades." and the debuff is gone. The oath shows up in the swear list again after 72 hours.
- [ ] Breaking a second oath while still an oathbreaker resets the penalty to a fresh 24 hours (it doesn't stack twice).
- [ ] *Renounce an oath*: lists only active oaths. Renouncing removes the blessing with no Oathbreaker effect, and the cooldown applies.
- [ ] `show SOT_LongestOathDays` reflects your longest-held oath in whole days.

## 5. Werewolf (if available)
- [ ] With the Shield oath, transform into a werewolf. The oath is not broken.

## 6. Persistence
- [ ] Save with active oaths, a cooldown and the penalty running. Quit to desktop, reload. Blessings stay, the next tier still arrives, and the penalty still ends on time.

## Stuck? Reset commands
```
player.removespell FExxx82E       (remove Oathbreaker)
set SOT_MaxOaths to 3
stopquest SOT_OathQuest           (then startquest SOT_OathQuest: resets all oath state;
                                   remove leftover blessings with player.removespell)
```
