# PowerShell-AI

A hotkey-driven Windows desktop application that leverages AI to generate single-line PowerShell commands. Stop typing complex commands manually—describe what you need, and PowerShell-AI generates the exact command for you.

## Features

- **Hotkey Activation**: Press a customizable hotkey from anywhere in Windows to instantly access the command generator
- - **AI-Powered Generation**: Uses Claude API to understand natural language and generate precise PowerShell commands
  - - **Secure API Key Storage**: Integrates with Windows Credential Manager for safe, encrypted storage of your API credentials
    - - **System Tray Integration**: Runs quietly in your system tray with easy access and exit options
      - - **User-Friendly UI**: Recently improved interface for faster and easier command composition
       
        - ## Quick Start
       
        - ### Option 1: Download Executable (Easiest)
       
        - 1. Visit the [Releases](https://github.com/Paul-Corbin/Powershell-AI/releases) page
          2. 2. Download the latest standalone executable from **PowerShell AI Release 1.0**
             3. 3. Run the .exe file - no installation required!
                4. 4. Enter your Claude API key when prompted (it will be securely stored)
                   5. 5. Set your preferred hotkey and start using it
                     
                      6. ### Option 2: Build from Source
                     
                      7. Requires .NET SDK and Visual Studio.
                     
                      8. 1. Clone this repository:
                         2.    ```bash
                                  git clone https://github.com/Paul-Corbin/Powershell-AI.git
                                  cd Powershell-AI
                                  ```

                               2. Open the solution in Visual Studio:
                               3.    ```bash
                                        start PowershellAI.sln
                                        ```

                                     3. Build the project:
                                     4.    - Build → Build Solution (or Ctrl+Shift+B)
                                       
                                           - 4. Run the application:
                                             5.    - Press F5 to start debugging
                                               
                                                   - ## Requirements
                                               
                                                   - - **For Executable**: Windows OS only
                                                     - - **For Source**: Windows OS, .NET SDK, and Visual Studio
                                                       - - **API**: Active Claude API Key ([Get one here](https://console.anthropic.com/))
                                                        
                                                         - ## Configuration
                                                        
                                                         - ### Setting Up Your API Key
                                                        
                                                         - 1. Launch PowerShell-AI
                                                           2. 2. The application will prompt you to enter your Claude API key on first run
                                                              3. 3. Your key will be securely stored in Windows Credential Manager
                                                                 4. 4. You can update your key at any time through the application settings
                                                                   
                                                                    5. ### Customizing Your Hotkey
                                                                   
                                                                    6. 1. Access the application settings (via system tray menu)
                                                                       2. 2. Configure your preferred hotkey combination
                                                                          3. 3. Changes take effect immediately
                                                                            
                                                                             4. ## Usage
                                                                            
                                                                             5. 1. Press your configured hotkey from anywhere in Windows
                                                                                2. 2. Describe the PowerShell command you need in natural language
                                                                                   3.    - Example: "List all files modified in the last 7 days"
                                                                                         -    - Example: "Kill all processes using more than 50% CPU"
                                                                                              -    - Example: "Find all PowerShell scripts in my Documents folder"
                                                                                                   - 3. Review the generated command
                                                                                                     4. 4. Execute it directly from the application or copy it to your clipboard
                                                                                                        5. 5. Your command history is saved for quick access
                                                                                                          
                                                                                                           6. ## Project Structure
                                                                                                          
                                                                                                           7. ```
                                                                                                              PowerShell-AI/
                                                                                                              ├── PowershellAI/          # Main application code
                                                                                                              │   ├── Program.cs         # Application entry point
                                                                                                              │   └── [Other source files]
                                                                                                              ├── PowershellAI.csproj    # Project configuration
                                                                                                              ├── PowershellAI.sln       # Visual Studio solution
                                                                                                              └── README.md              # This file
                                                                                                              ```
                                                                                                              
                                                                                                              ## Security & Privacy
                                                                                                              
                                                                                                              - API keys are stored using Windows Credential Manager, not in configuration files or plain text
                                                                                                              - - All communications with Claude API use secure HTTPS connections
                                                                                                                - - No command history or personal data is sent beyond what's necessary for API calls
                                                                                                                  - - Your commands remain private and are only sent to Claude's API
                                                                                                                   
                                                                                                                    - ## Development
                                                                                                                   
                                                                                                                    - ### Tech Stack
                                                                                                                    - - **Language**: C#
                                                                                                                      - - **Framework**: .NET
                                                                                                                        - - **UI**: Windows Forms / WPF
                                                                                                                          - - **API**: Anthropic Claude API
                                                                                                                           
                                                                                                                            - ### Building from Source
                                                                                                                           
                                                                                                                            - 1. Install Visual Studio (Community edition is free)
                                                                                                                              2. 2. Ensure you have .NET SDK installed
                                                                                                                                 3. 3. Open `PowershellAI.sln`
                                                                                                                                    4. 4. Restore NuGet packages
                                                                                                                                       5. 5. Build and run
                                                                                                                                         
                                                                                                                                          6. ## Contributing
                                                                                                                                         
                                                                                                                                          7. Contributions are welcome! Please feel free to:
                                                                                                                                          8. - Report bugs via [Issues](https://github.com/Paul-Corbin/Powershell-AI/issues)
                                                                                                                                             - - Submit feature requests
                                                                                                                                               - - Create pull requests with improvements
                                                                                                                                                
                                                                                                                                                 - ## License
                                                                                                                                                
                                                                                                                                                 - Please specify your license (MIT, Apache 2.0, etc.)
                                                                                                                                                
                                                                                                                                                 - ## Support
                                                                                                                                                
                                                                                                                                                 - For issues, questions, or suggestions, please open an [issue](https://github.com/Paul-Corbin/Powershell-AI/issues) on GitHub.
                                                                                                                                                
                                                                                                                                                 - ## Acknowledgments
                                                                                                                                                
                                                                                                                                                 - - Built with the [Anthropic Claude API](https://www.anthropic.com/)
                                                                                                                                                   - - Windows integration powered by .NET
                                                                                                                                                    
                                                                                                                                                     - ---
                                                                                                                                                     
                                                                                                                                                     **Note**: This application requires an active Claude API key. Visit [Anthropic's console](https://console.anthropic.com/) to get started.
