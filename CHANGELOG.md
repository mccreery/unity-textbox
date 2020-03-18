# [Unreleased]
## Added
- New textbox component: controls textbox typing speed and events
  - Prevents interleaved typing from multiple sources
  - Individual typing speed for whitespace and non-whitespace
  - Scaled time option
  - Event hooks for typing
  - Allows interruptions
  - Rich text support
  - No word wrap reflows
  - Instant typing in the same frame (with delay=0)
- New typing sounds component: plays a sound after each character
  - Play on whitespace option
  - Minimum delay between sounds
  - Random pitch shift
  - Pitch shift window: ramps up or down to a final pitch at the end of each word
- New prompt component: exposes coroutines for input-based textboxes
  - Configurable button
  - Turbo mode with speed multiplier option (double press)
- New dialogue component: exposes coroutines for stringing together messages
  - List of messages
  - Option to play immediately
  - Prompt mode or automatic mode with delay between each message
  - Scaled time option
- Lazy loading and creation of component dependencies: Textbox, TextboxPrompt, AudioSource: \
  Unless set in the inspector, other component fields will be populated on first access
