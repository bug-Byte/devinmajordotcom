# devinmajordotcom

The entire source code repository for http://devinmajor.com, generic-ized so you can set yourself up with your own boilerplate copy!

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live server.

### Prerequisites

What things you need to install the software and how to install them. If you're a developer you probably have all these things already... just make sure!

```
A decent PC / server with a Windows OS installed
Visual Studio (2012 or Higher)
An SQL Server / Express instance, as well as [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017)
[OpenHardwareMonitor](http://openhardwaremonitor.org/) (A third-party program, required to make hardware stats/charts function!)
```

### Installing

A step by step series of examples that tell you how to get a development environment running, so you can fiddle with the code to your liking before publishing to your server! Make sure to have the solution open in Visual Studio, as well as having a running instance of SQL Server / Express. It will also be helpful to have SSMS (SQL Server Management Studio) open as well, in case of any data issues.

First, you'll need to include a reference to the DLL in your installation of the OpenHardwareMonitor program. Remove any broken/missing references to this library in the solution, then follow the steps outlined in the images below...

![capture](https://user-images.githubusercontent.com/10644309/41570013-18f1ec2a-733b-11e8-8379-24bb324751d6.PNG)

![capture1](https://user-images.githubusercontent.com/10644309/41570044-2983fcf4-733b-11e8-978b-2c6268d17118.PNG)

Next, you'll need to create and publish a new database for the application to store and retrieve data in. You can do this by right clicking the MyDatabase project, and following the steps outlined in the images below...

![capture3](https://user-images.githubusercontent.com/10644309/41570079-336de70c-733b-11e8-882c-2e6cd832041f.PNG)

![capture4](https://user-images.githubusercontent.com/10644309/41570087-3b1c1064-733b-11e8-8081-d50eecb88894.PNG)

![capture5](https://user-images.githubusercontent.com/10644309/41570091-415543b0-733b-11e8-855f-861f926d4221.PNG)

You'll notice there's a field for "UserRole". This is used to give read/write/edit/special permissions to the Windows entity managing the database connection.

This is the user role that is typically used:
```
NT AUTHORITY\SYSTEM
```

Once this is done, feel free to Run/Start the project in the IDE! You should have a localhost instance starting up, and when it loads, you'll be presented with the following FirstRun page:

![capture6](https://user-images.githubusercontent.com/10644309/41570124-57e84b5e-733b-11e8-9673-41558ba0ece3.PNG)

All you have to do is fill out the form, and you'll get an email confirming your account details. Once you get the email, you can log in! 

When you log in as an administrator, you'll notice you have a Settings page. There are lots of options, and you have near total control over the entire website from this page! 
If you have any questions or are confused about one of the settings here or how they work, please don't hesitate to get in touch with me. I'd love to explain!

Here's some screenshots of the main pages in action, immediately after deploying! Impressive, is it not? ;)

![capture7](https://user-images.githubusercontent.com/10644309/41570135-78a0bae8-733b-11e8-9931-81dcb89680e9.PNG)

![capture8](https://user-images.githubusercontent.com/10644309/41570143-8082b734-733b-11e8-8882-8b0ecbe06b42.PNG)

![capture9](https://user-images.githubusercontent.com/10644309/41570149-86adfbd2-733b-11e8-9187-7adb183a2565.PNG)

## Running the tests

Unfortunately I haven't had time to write tests for this application as of yet (Yes I already know how important they are and how I should write them *first*)

## Deployment

Just follow the normal deployment process for an ASP.NET application on a Windows Server. The MSDN has better instructions than I can possibly include here, so I'll direct you to the article I used! 
[Here's How to Deploy the Application](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis)

## Built With

* [Visual Studio 2017](https://www.visualstudio.com/downloads/) - The IDE that was used
* [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017) - The SQL database manager that was used
* [OpenHardwareMonitor](http://openhardwaremonitor.org) - Used to monitor your hardware, in an open source fashion ;)
* [JQuery](https://jquery.com/) - JavaScript framework used
* [Bootstrap](https://getbootstrap.com/) - CSS framework used
* [SignalR](https://www.asp.net/signalr) - Real-time API web framework used

## Contributing

Please feel free to contribute to this project. It's completely open to anyone, though if you *do* want to contribute, I ask that you contact me either through GitHub, or through the email portal of [My Website](https://devinmajor.com/).
Also, please use the Development branch to branch/fork from, and make pull requests for. I am adhering to GitFlow (for the most part) and refraining from using the master branch.

## Versioning

We use [Git](https://git-scm.com/) as well as [SourceTree](https://www.sourcetreeapp.com/) for versioning. For the available versions of this application, see the list of branches near the top of this very page!

## Authors

* **Devin Major** (me) - *Everything* - [My Website](https://devinmajor.com)
## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details

## Acknowledgments

* Hat tip to anyone whose code was used in part
* Inspiration and occasional help from my friends and colleages
* Special thanks to Patrick Manseau for being so patient through this long tedious process
