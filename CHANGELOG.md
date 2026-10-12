# Changelog

All notable changes to this project will be documented in this file.

The format is inspired by [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project may adopt [Semantic Versioning](https://semver.org/) going forward.

## [Unreleased]

### Added
- Initial Windows WinForms client structure for ESP32 UDP communication.
- Bidirectional UDP communication flow (send commands / receive responses).
- Receive thread and UI-safe updates using `Control.Invoke`.
- Input controls for IPv4 and port configuration.
- Basic command send workflow and response console behavior.
- Project documentation with architecture, network schematic, data flow, and usage guidance.

### Notes
- Repository currently has no tagged releases.
- This changelog was bootstrapped from commit history on **2026-10-12**.

## [0.1.0] - 2026-10-12

### Added
- First documented baseline of the `esp32-Comm` project.
- Core solution and WinForms app under `CommEspe32/`.
- Initial README with requirements, build instructions, and integration notes.

[Unreleased]: https://github.com/paulocfmarques-collab/esp32-Comm/compare/0.1.0...HEAD
[0.1.0]: https://github.com/paulocfmarques-collab/esp32-Comm/releases/tag/0.1.0
