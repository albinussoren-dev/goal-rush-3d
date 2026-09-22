# Goal Rush 3D

Unity 6 LTS Android portrait 3D football obstacle runner.

## Current upgrade track
This repository is being evolved as a long-term game project. The current source includes a modular future-ready foundation for progression, daily rewards, missions, shop inventory, level catalog, save migration/backup, object pooling, settings, and mobile performance controls.

## Core gameplay
- 3-lane auto runner
- Swipe/touch lane switching, jump and slide
- Coins, money, gems and health
- Reward/penalty gates and obstacles
- 5 worlds / 100 level catalog
- Level unlock progression and deterministic level metadata
- Pause, retry, next-level and game-over flow

## Progression foundation
- Character unlock/select data for 5 characters
- Speed, jump, shield and magnet upgrade levels
- Shop consumable charges
- Four mission types with claimable rewards
- Seven-day daily reward streak model
- Versioned JSON save with backup recovery and atomic write
- Settings for master volume, SFX/music values, vibration and battery-saver mode

## Mobile/performance foundation
- Android portrait configuration
- Object-pool service for reusable spawned objects
- Device memory-aware quality fallback
- 60 FPS target with optional battery-saver target
- Runtime-generated primitives, so no external art dependency is required for the prototype

## Open
Open the project in Unity 6 LTS using Assets/Scenes/Bootstrap.unity.

## Controls
Swipe left/right: lane. Swipe up: jump. Swipe down: slide.
Editor: A/D or arrows, Space jump, S/down slide.

## Build Android
Use Unity Build Profiles, Android platform, portrait orientation, package identifier com.goalrush3d.game, then build an APK/AAB.

## Testing status
Unit-test source has been expanded, but this repository has not been claimed as Android-device tested or Unity-editor compiled in this environment. A release build should be validated in Unity 6 LTS on Android before distribution.

## Source package
The initial local source package remains available from the ChatGPT conversation as GoalRush3D_Unity_Project.zip; it is not stored as a binary asset in this Git repository.

## Roadmap
1. Complete production UI/navigation for world, level, character, shop, mission and daily-reward screens.
2. Replace runtime primitives with optimized original 3D assets, animation controller and VFX.
3. Add real level data/segment libraries, checkpoints, pooling integration and difficulty curves.
4. Add Android build automation, crash-safe telemetry hooks and release validation.
5. Run Unity compile, edit-mode tests, device performance tests and APK/AAB smoke tests before calling a release build production-ready.

## Notes
The game uses an original fictional football character and avoids real-player likenesses or club branding.
