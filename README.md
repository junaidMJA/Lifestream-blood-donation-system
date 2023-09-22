[![Lifestream Banner](https://raw.githubusercontent.com/MaizNadeem/Lifestream-WPF/master/Frontend%20Screenshots/Banner.png)](https://drive.google.com/file/d/1FtdHXHE4VaEmcc1aHR-ecpwvatUnHzYx/view?usp=sharing)

# Lifestream - Blood Bank System

## About
Lifestream is a blood donation management system designed to streamline blood donation activities and facilitate efficient management of donor information, appointments, and requests. It provides a user-friendly interface for staff members to perform various tasks related to blood donations and ensures smooth coordination between donors, appointments, and blood requests.

## Documentation
Detailed documentation for the Lifestream - Blood Bank System can be found [here](https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Documentation.pdf).

## Installation Instructions

1. Download the installation file by clicking on the following link: [Download Lifestream Installer](https://drive.google.com/file/d/1FtdHXHE4VaEmcc1aHR-ecpwvatUnHzYx/view?usp=sharing)
2. Once the download is complete, locate the downloaded file named `Install.rar`.
3. Extract the contents of the `Install.rar` file to a desired location on your computer.
4. Open the extracted folder and navigate to the "Install" folder.
5. Inside the "Install" folder, you will find a file named `setup.exe`. Double-click on it to run the setup.
6. Follow the on-screen instructions provided by the setup wizard to proceed with the installation. You may need to accept the terms and conditions, choose the installation directory, and select any additional settings as required.
7. Once the installation process is complete, the Lifestream application will be downloaded and installed on your computer.

To uninstall Lifestream:

1. Go to the "Apps & features" settings on your computer. You can access this by searching for "Apps & features" in the Windows search bar or by going to the Control Panel and selecting "Uninstall a program" (Windows 7).
2. In the list of installed applications, locate "Lifestream" and click on it.
3. Click on the "Uninstall" button and follow any prompts or instructions provided to complete the uninstallation process.
By following these instructions, you should be able to install and uninstall Lifestream on your computer.

## Screenshots
Here are some screenshots of the Lifestream - Blood Bank System:

### Login Screen
<p align="center">
  <img src="https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Frontend%20Screenshots/Login.png?raw=true" alt="Login Screen" width="70%">
</p>

### Main View
<p align="center">
  <img src="https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Frontend%20Screenshots/Dashboard.png?raw=true" alt="Main View">
</p>

### Other Views
<p align="center">
  <img src="https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Frontend%20Screenshots/View%20Donors.png?raw=true" alt="Donor View">
  <img src="https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Frontend%20Screenshots/View%20Staff.png?raw=true" alt="Staff View">
  <img src="https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Frontend%20Screenshots/Staff's%20Info.png?raw=true" alt="Staff Info View">
</p>

You can find all the application screenshots [here](https://github.com/MaizNadeem/Lifestream-WPF/tree/master/Frontend%20Screenshots).

## Entity Relationship Diagram (ERD)
![ERD](https://github.com/MaizNadeem/Lifestream-WPF/blob/master/Frontend%20Screenshots/ERD.png?raw=true)

The ERD illustrates the logical structure and relationships between the entities (tables) in the Lifestream - Blood Bank System's database.

## Technologies Used
The Lifestream - Blood Bank System is built using the following technologies:

- **.NET Framework 4.8:** The application is developed using the .NET Framework 4.8, which provides a robust and stable development platform for Windows applications.
- **WPF (Windows Presentation Foundation):** The user interface is built using WPF, a powerful framework for creating desktop applications with rich UI and interactive user experiences.
- **MSSQL Server / Azure Database:** The application uses either Microsoft SQL Server or Azure Database as the backend database to store and manage donor information, appointments, and requests.
- **NuGet Packages:**
  - **Lepoco:** Used for UI design and controls, providing a modern and visually appealing user interface.
  - **LiveCharts.Wpf:** Used for creating interactive and dynamic charts to visualize blood donation data.
  - **FontAwesome.Sharp:** Used to incorporate a wide range of icons and fonts into the application for enhanced visual elements.

## Deployment Instructions
To deploy the Lifestream - Blood Bank System on your own Windows machine, follow these steps:

1. Download the source code from the [GitHub repository](https://github.com/MaizNadeem/Lifestream-WPF).
2. Ensure that you have the following dependencies installed:
   - Visual Studio (2019 or later) with .NET Framework 4.8 development tools.
   - .NET Framework 4.8 runtime.
   - Azure Cloud account with Azure Data Studio (or SQL Server Management Studio - SSMS) installed.
3. Open the project in Visual Studio.
4. Update the connection string in the `RepositoryBase.cs` file located at `Lifestream-WPF/Repositories/RepositoryBase.cs`. Replace `"AZURE_CONNECTION_STRING"` with your own connection string.
5. Build the solution to restore NuGet packages and compile the project.
6. Publish the project using Visual Studio to create the executable files.
7. Copy the published files to the target machine.
8. Install .NET Framework 4.8 runtime on the target machine if it's not already installed.
9. Run the executable file to start the Lifestream - Blood Bank System.

## Database Setup
To set up the database for the Lifestream - Blood Bank System, follow these steps:

1. Download the [BloodBank.bacpac](https://github.com/MaizNadeem/Lifestream-WPF/blob/master/BloodBank.bacpac) file from the GitHub repository.
2. Open Azure Data Studio or SQL Server Management Studio (SSMS).
3. Connect to your Azure Database or local MSSQL Server.
4. Right-click on the Databases folder and choose "Import Data-tier Application."
5. Select the downloaded BloodBank.bacpac file and follow the import wizard to restore the database.
6. Once the database is restored, update the connection string in the `RepositoryBase.cs` file of the application to point to the newly restored database.

## Additional Notes
- Make sure to have Visual Studio (2019 or later) and .NET Framework 4.8 installed on your machine before proceeding with the deployment.
- The `BloodBank.bacpac` file contains the pre-configured database schema for the Lifestream - Blood Bank System. You can use it to quickly set up the database.
- For any issues or questions, please refer to the project's [GitHub repository](https://github.com/MaizNadeem/Lifestream-WPF) or contact the project owner.

<hr>

<h4 align="center">© M. Maiz Nadeem</h4>
