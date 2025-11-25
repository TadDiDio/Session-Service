# Session Service

The **Session Service** provides a simple, provider-agnostic abstraction for connecting, disconnecting, and managing session state in Unity projects.  
It is designed to be lightweight, testable, and easily extended with custom session providers (client-host, dedicated, Steam, local server, etc.).

---

## Features

- **Unified Session Abstraction**  
  One API for connecting, disconnecting, and managing session lifecycles — regardless of backend.

- **Provider-Agnostic**  
  Swap in different `ISessionProvider` implementations without changing game code.

- **Simple State Machine**  
  Built-in connection states:  
  `Disconnected → Connecting → Connected → Disconnecting`

- **Events for UI/Systems**  
  Consumers can subscribe to `OnStateChanged` for clean flow control.

---

## Installation (Unity UPM)

In package manager use install by git url and paste the following:
```bash
https://github.com/TadDiDio/Session-Service.git?path=Packages/com.radtad2.sessionservice#1.0.0
```

## API

To interact with the system, use the `Session` API where you have access to various methods. Call `Session.SetProvider(ISessionProvider)`
to (re)-initialize the system. Any calls to the API before a provider is set are safely dropped.