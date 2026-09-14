# Model Configuration Switcher

## Introduction
This class encapsulates model selection logic for the Antigravity integration, providing safe defaults and transitions to `gemini-3.7-flash`.

## Usage
```csharp
ModelConfig config = new ModelConfig();
config.SwitchToGemini37Flash();
```

## Detailed Explanation
The `ModelConfig` class maintains the active AI model name with input validation. It defaults to `gemini-3.7-flash` and provides explicit utility methods for switching configuration state.

## Complexity Analysis
- Time Complexity: O(1) for getter and setter operations.
- Space Complexity: O(1) auxiliary memory space usage.