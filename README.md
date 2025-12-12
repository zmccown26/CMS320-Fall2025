# CMS320-Fall2025

## Company Name: WZW
## Game Name: Heli Hero

### Zach McCown: Team Lead & Programmer
### Will Bruzzese: Artist & Programmer
### Weston Hollmann: Story Writer & Programmer

## Description
A 2D helicopter landing game built with Unity 6. Navigate through challenging levels, avoid enemy turrets, collect coins and fuel, and land safely on landing pads to progress!

## 🎮 Game Overview

Heli Hero is a physics-based 2D helicopter landing game where players must carefully pilot their helicopter through levels filled with obstacles and enemies. The goal is to safely land on designated landing pads while managing fuel, avoiding hazards, and collecting bonuses along the way.

## 🎯 Objective

Successfully land your helicopter on the landing pads in each level. Landing quality determines your score - the more precise your landing angle and speed, the higher your score!

## 🕹️ Controls

- **↑ Up Arrow**: Apply upward thrust (consumes fuel)
- **← Left Arrow**: Rotate helicopter counterclockwise (consumes fuel)
- **→ Right Arrow**: Rotate helicopter clockwise (consumes fuel)
- **Menu Button** (ESC/Input System): Pause/Unpause the game

### How to Play

1. Press any arrow key to start the level
2. Use arrow keys to control your helicopter
3. Navigate through the level, avoiding obstacles and enemy turrets
4. Collect coins (500 points) and fuel pickups to extend your flight time
5. Land safely on the landing pad to complete the level
6. Progress through multiple levels to complete the game

## 🏆 Scoring System

Your score is calculated based on several factors:

### Landing Score
- **Landing Angle**: Maximum 100 points based on how upright your helicopter is (must be ≥90% upright to land successfully)
- **Landing Speed**: Maximum 100 points based on landing softly (speed ≤ 4 units/second)
- **Score Multiplier**: Landing pads have different multipliers that multiply your landing score

### Bonus Points
- **Coins**: Collect coins scattered throughout levels for 500 points each

### Landing Requirements

To successfully land, you must:
- ✅ Land on a designated landing pad (not on terrain)
- ✅ Be nearly upright (angle ≥ 90% vertical)
- ✅ Land softly (speed ≤ 4 units/second)

Failure to meet any requirement results in a crash and level restart.

## ⚙️ Game Mechanics

### Fuel System
- Helicopter starts with 10 units of fuel
- Fuel is consumed when using any control input
- Fuel consumption rate: 1 unit per second while using controls
- Collect fuel pickups to refill your tank (adds 10 units, capped at maximum)
- Running out of fuel prevents you from controlling the helicopter

### Enemy Turrets
- Turrets fire homing missiles when you enter their trigger zones
- Missiles track your helicopter and explode on impact
- Being hit by a missile results in a crash
- Turrets can be triggered by proximity and movement

### Physics
- Gravity: 0.7 (normalized)
- Thrust force: 700 units
- Rotation speed: 100 units/second
- Soft landing velocity threshold: 4 units/second

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Lander.cs              # Main player helicopter controller
│   ├── GameManager.cs         # Core game state and level management
│   ├── GameInput.cs           # Input system handler
│   ├── GameLevel.cs           # Level configuration and spawn points
│   ├── LandingPad.cs         # Landing pad with score multipliers
│   ├── TurretController.cs   # Enemy turret firing system
│   ├── TurretAmbushTrigger.cs # Turret activation zones
│   ├── HomingMissile.cs      # Homing missile behavior
│   ├── CoinPickup.cs          # Coin collectible
│   ├── FuelPickup.cs          # Fuel collectible
│   ├── StatsUI.cs             # In-game HUD display
│   ├── LandedUI.cs            # Landing results screen
│   ├── PausedUI.cs            # Pause menu
│   ├── MainMenuUI.cs          # Main menu interface
│   ├── SceneLoader.cs         # Scene management utility
│   ├── CinemachineCameraZoom2D.cs # Camera zoom system
│   └── BackgroundMusic.cs     # Background music controller
├── Scenes/
│   ├── Main_menu.unity        # Main menu scene
│   └── Level_01.unity         # Gameplay scene (loads level prefabs dynamically)
├── Prefabs/
│   ├── Level_01.prefab        # Level 1 configuration
│   ├── Level_02.prefab        # Level 2 configuration
│   └── Level_03.prefab        # Level 3 configuration
└── InputSystem_Actions.inputactions # Input system configuration
```

## 🛠️ Technical Details

### Unity Version
- **Unity Editor**: 6000.2.11f1 (Unity 6)

### Key Technologies
- **Unity Input System**: Modern input handling
- **Cinemachine**: Camera tracking and zoom
- **Unity Physics 2D**: Rigidbody2D for helicopter physics
- **TextMesh Pro**: UI text rendering
- **Unity Events**: Event-driven architecture for game state

### Game States
The game uses a state machine with three main states:
- **WaitingToStart**: Initial state before player input
- **Normal**: Active gameplay state
- **GameOver**: Game ended (success or crash)

### Level System
- Levels are managed through the `GameManager` singleton
- Each level is a prefab with a `GameLevel` component
- Levels define spawn positions, camera targets, and zoom levels
- Currently supports 3 levels (returns to main menu after level 3)

## 🚀 Setup Instructions

### Prerequisites
- Unity 6 (6000.2.11f1 or compatible version)
- Unity Input System package
- Cinemachine package
- TextMesh Pro package

### Installation
1. Clone or download this repository
2. Open the project in Unity Hub
3. Unity will automatically import required packages
4. Open the `Main_menu` scene to start

### Building
1. Go to **File > Build Settings**
2. Add scenes in order: `Main_menu`, `Level_01`
3. Select your target platform
4. Click **Build**

## 🎨 Features

- ✅ Physics-based helicopter controls
- ✅ Fuel management system
- ✅ Multiple levels with progression
- ✅ Enemy turrets with homing missiles
- ✅ Collectible coins and fuel pickups
- ✅ Dynamic camera system with zoom
- ✅ Scoring system based on landing quality
- ✅ Pause functionality
- ✅ Main menu and level selection
- ✅ Real-time stats display (level, score, time, speed, fuel)
- ✅ Landing results screen with detailed stats
- ✅ Background music support

## 🐛 Known Issues / Notes

- The game uses Unity's new Input System for menu controls
- Homing missiles can optionally explode on ground contact (requires "Ground" tag)
- Score resets between levels
- Timer only runs during active gameplay (not during waiting or game over states)

## 📝 Development Notes

### Adding New Levels
1. Create a new level prefab with a `GameLevel` component
2. Set the level number in the component
3. Configure spawn positions and camera settings
4. Add the prefab to the `GameManager`'s level list in the Inspector

### Customizing Landing Pads
- Each landing pad has a configurable score multiplier
- Set the multiplier in the `LandingPad` component Inspector

### Adjusting Difficulty
Key parameters to modify:
- `Lander.cs`: Fuel consumption, thrust force, rotation speed
- `HomingMissile.cs`: Missile speed and rotation speed
- `TurretAmbushTrigger.cs`: Activation delays and conditions
- `GameLevel.cs`: Camera zoom levels for different perspectives

## 📄 License

This project is part of CMS320 Fall 2025 coursework.

## 👥 Credits

Developed for CMS320 - Fall 2025

---

**Enjoy flying!** 🚁
