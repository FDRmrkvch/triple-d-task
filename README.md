# Triple D Test Task

A modular and scalable Unity UI system built to fulfill the requirements of a technical art assignment.
This repository demonstrates a clean approach to UI structure, animation, localization and extensibility.

---

## Features

### ✅ Task 1: Home Screen & Bottom Bar

- **Adaptive Layout**
  - Responsive UI that supports various screen sizes and safe areas
  - Background uses high-resolution images and camera-based blur post-processing

- **Top Bar**
  - Displays currency icons and a settings button

- **Bottom Tab Bar**
  - Tabs are dynamically generated using a `ScriptableObject`
  - Tab states: `Locked`, `Unlocked`, `Selected`
  - Smooth transition animations via `Animator`
  - Only one tab can be `Selected` at a time
  - Each tab supports:
    - Custom icon via sprite reference
    - Localized label via `LocalizeStringEvent`

- **TabBar Configurable via SO**
  - Specify tab count, icon, state, and localization key in `TabBarConfigSO`
  - Icon assets and localization references are externalized


### ✅ Task 2: Settings Popup

- **PopupManager System**
  - Dynamically loads popup prefabs via `ScriptableObject` config
  - Popups implement `IPopup` interface with `Show()` / `Close()` lifecycle
  - Centralized instantiation under `Canvas_Popup`

- **Settings Popup**
  - Opens via top bar
  - Animator-driven `In` and `Out` transitions
  - Button to close popup cleanly

- **Background Tint Control**
  - Custom `BackgroundTintController` to animate:
    - Volume weight (post-processing)
    - Background overlay opacity
  - Easing transition support for dimming and reset

- **Localization System**
  - Uses Unity’s `Localization` package
  - `LocalizeStringEvent` used for all translatable UI labels
  - `LanguageSwitcher`:
    - Supports cycling between locales
    - Displays flag via `LocaleFlagConfigSO`
    - Saves selected locale in `PlayerPrefs`


### ✅ Task 3: Completed Screen

- **Completed Screen Popup**
  - Triggered dynamically
  - Built on the same popup system

- **Animated Grid Background**
  - Spawns `N x M` grid of prefab tiles
  - Each tile (`GridItem`) plays random legacy animation
  - Entire grid rotates over time (configurable speed/direction)

- **Particle System Integration**
  - Displays celebratory particles
  - Proper canvas layering ensures particles are above all UI

---


## Architecture Overview

### 1. **Tab Bar System**

Dynamically builds a tab bar from a ScriptableObject config.

#### Components:
- `TabBarConfigSO` — defines:
  - number of tabs,
  - available icon set (name + sprite),
  - tab state and localization key per tab.
- `BarItemManager` — instantiates and configures tabs at runtime.
- `BarItemController` — handles click logic, animations, and localized label.
- Animator includes states:
  - `Locked`, `Unlocked`, `Selected`,
  - triggers: `Select`, `Unselect`, `Pressed`.

#### Features:
- Only one tab can be in `Selected` state.
- All animator triggers and state names are editable via Inspector.
- Localized tab labels are supported via `LocalizeStringEvent`.

### 2. **Popup Manager**

Manages UI popups via a clean interface and SO configuration.

#### Components:
- `PopupManager` — dynamically instantiates popups by name.
- `PopupConfigSO` — defines popup names and corresponding prefabs.
- `IPopup` — interface for popups (`Show()`, `Close()`).
- `BasePopup` — optional base class that handles open/close animation with `AnimationEvent`.
- `PopupAutoUnregister` — unregisters popup when destroyed.

#### Example popups:
- `SettingsPopup`
- `CompletedScreen`

### 3. **Completed Screen**

Displays level completed animation with grid of animated tiles.

#### Components:
- `GridAnimationController` — handles:
  - spawning NxN prefab tiles based on config,
  - continuous rotation of the grid container,
  - random playback of legacy animations on tiles.
- `GridItem` — prefab with multiple `Legacy Animation Clips`.


### 4. **Localization & Language Switcher**

Uses Unity’s Localization Package with a custom UI switcher.

#### Components:
- `LanguageSwitcher` — cycles available locales:
  - saves preference in `PlayerPrefs`,
  - updates `LocalizeStringEvent` elements,
  - updates language flag icon.
- `LocaleFlagConfigSO` — configures available locales and flag icons.

---

## ⚙️ Tech Stack

- Unity 2022.3.36f1
- Unity UI (UGUI)
- Unity Animator & Animation Events
- Unity Localization
- ScriptableObjects

---

Clone the repository:
   ```bash
   git clone https://github.com/FDRmrkvch/triple-d-task.git
