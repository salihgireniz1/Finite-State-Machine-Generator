# Finite State Machine Generator

FSM Generator is a Unity tool designed to simplify the creation and management of finite state machines in your Unity projects. By leveraging the R3 reactive extensions, the FSM Generator enhances the performance and responsiveness of state transitions, making it ideal for developers looking to optimize their game or application dynamics.

## Features

- **Dynamic State Machine Creation**: Easily create and manage state machines with a user-friendly editor window.
- **Customizable States**: Add or remove states dynamically within the Unity Editor.
- **Enhanced Performance**: Integrates with the R3 reactive extension to ensure state transitions are handled efficiently and performantly.
- **Reactive Extensions Powered**: Utilizes the power of reactive programming to manage state changes, providing a robust foundation for complex state behaviors.
- **Integration with Existing Projects**: Seamlessly fits into any Unity project to enhance state management without disrupting existing workflows.

## Dependencies

This project relies on the following packages:
- **R3** - A runtime library that enhances Unity's capabilities. [Learn more about R3](https://github.com/Cysharp/R3)
- **NuGetForUnity** - A NuGet client for Unity, which allows the integration of NuGet packages directly into your Unity projects. [Learn more about NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)

## Installation
https://github.com/salihgireniz1/Finite-State-Machine-Generator/assets/78977599/564d2f7b-5449-4208-b812-e919f91130e7
Before installing FSM Generator, ensure the following packages are installed in your Unity project:

### 1. NuGetForUnity
Enables the use of NuGet packages directly in Unity projects.

**Installation**:
- Download the latest `.unitypackage` from [NuGetForUnity Releases](https://github.com/GlitchEnzo/NuGetForUnity/releases).
- Import it into your Unity project via `Assets -> Import Package -> Custom Package...`.

### 2. R3
R3 enhances Unity's capabilities with advanced runtime features. The minimum supported Unity version for R3 is Unity 2021.3.

**Installation Steps**:
1. **Install R3 Using NuGetForUnity**:
   - Open `NuGet -> Manage NuGet Packages` in Unity.
   - Search for "R3" and click "Install".
   - If you encounter version conflicts, disable "Assembly Version Validation" in `Edit -> Project Settings -> Player`, under "Configuration" in the "Other Settings" section.

2. **Install the R3.Unity Package via Git URL**:
   - Use the Git URL: `https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity`
   - You can specify a version by appending a release tag, e.g., `#1.0.0`.

### Installing FSM Generator

Once the dependencies are installed:
1. Download the latest FSM Generator release from the [Releases page](https://github.com/salihgireniz1/Finite-State-Machine-Generator/releases).
2. Import the package into your Unity project via `Assets -> Import Package -> Custom Package...` and select the downloaded package.

### Installing FSM Generator

Once the dependencies are installed, you can add FSM Generator to your project by following these steps:
1. Download the latest release of FSM Generator from the [Releases](https://github.com/salihgireniz1/fsm-generator/releases) page.
2. Import the package into your Unity project via `Assets -> Import Package -> Custom Package...` and select the downloaded package.

## Usage

The FSM Generator can be used in two primary ways: through the Unity Editor window or by manually creating and configuring the Scriptable Object.
https://github.com/salihgireniz1/Finite-State-Machine-Generator/assets/78977599/5dbc82ec-6fec-47fa-b6a4-aa8386454ef7
### Using the Editor Window

To open the FSM Generator window, navigate to `Tools -> FSM Generator` in the Unity menu bar. This window provides a user-friendly interface to create and manage state machines directly within Unity. Here you can:

- **Create a new state machine**: Enter the state machine name, specify the path, and define the states directly in the editor.
- **Configure states**: Add or remove states as your project evolves.
- **Set the default state**: Choose which state should be the default on initialization.

### Manually Creating the Scriptable Object

You can also create a state machine by manually creating a Scriptable Object in Unity. This method gives you direct control over the FSM Generator's properties through the Unity Inspector.

1. **Create the Scriptable Object**:
   - Right-click in the Project panel and select `Create -> FSM -> FSMGenerator`. This will create a new FSM Generator Scriptable Object in your project.
   - Alternatively, you can duplicate an existing FSM Generator if you have one.

2. **Configure the FSM Generator**:
   - Select the newly created FSM Generator in the Project panel.
   - In the Inspector, fill out the details of your state machine:
     - **State Machine Name**: Set a unique name for your state machine.
     - **States**: Add the names of the states needed for your state machine.
     - **Path**: Specify the path where the state machine should be saved.
     - **Default State Index**: Set the index of the default state from the list.

3. **Generate the State Machine**:
   - Once configured, you can invoke the FSM generation by selecting the FSM Generator object and pressing a designated "Generate" button in the Inspector or using a custom menu option you define in your scripts.

### Note:
Both methods will generate the same underlying state machine structure, but using the editor window might be faster for users unfamiliar with Unity's Scriptable Object system or who prefer a more graphical interface.

## Contributing

Contributions to FSM Generator are welcome. Please feel free to fork the repository, make changes, and submit pull requests.

## Acknowledgements

- Thanks to Cysharp for the [R3](https://github.com/Cysharp/R3) package which enhances the functionality of Unity projects.
- Thanks to the developers of [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) for providing a bridge to integrate NuGet packages within Unity.
