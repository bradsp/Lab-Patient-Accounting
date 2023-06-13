<img src="Lab Billing UI/Resources/logoicon2.png" alt="Logo" width="80" height="80">

# Lab-Patient-Accounting
A medical patient account application developed for an outreach laboratory.

*Note: This readme is incomplete. I am adding to it as I have time.*

## About the Project
Lab Patient Accounting is a medical patient accounting and billing application created specifically for outreach laboraties. Laboratory charges can be billed to the client via invoice, insurance company or health plan via X12 837i and 837p files to a billing clearing house, or to the patient via statements.

**Features**

* Patient and account entry & updates via HL7 interface (ADT)
* Charge entry via HL7 interface (DFT)
* Provider file maintenance via HL7 interface (MFN)
* X12 837 5010 for electronic claims
* 


### Built With

* Visual Studio 2022
* C# 7.3
* .NET Framework 4.8
* WinForms
* Microsoft SQL Server
* Microsoft SQL Server Reporting Services
* PetaPoco
* AutoFac
* EdiTools
* FluentValidation
* log4net
* MDIWindowManager
* MetroModernUI
* Newtonsoft.Json
* NLog
* PDFsharp
* Quartz
* Spectre.Console
* Topshelf

## Getting Started

* Create database on a Microsoft SQL Server instance using the LabPatientAccounting DB Build.sql script. Update the database name and server name in the script to match your installation.
* Initial database table load is needed for the application to run.
  * System table (application parameters).
  * Emp table (users). 
* Open the repository in Visual Studio. In the Lab Billing UI project, update the database name and server name in the Settings.Settings. Build the application. 
* Run the application.

### Prerequsites

* Microsoft SQL Server 2014 or greater
* Microsoft Windows PC for client application
* 

### Installation




## Usage


## Roadmap


## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the GNU Affero General Public License v3.0. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
## Contact

Bradley Powers - [@bradleyspowers](https://twitter.com/bradleyspowers) - [LinkedIn](https://www.linkedin.com/in/bradley-powers/) - bradspowers@outlook.com

Project Link: [https://github.com/bradsp/Lab-Patient-Accounting](https://github.com/bradsp/Lab-Patient-Accounting)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!

* [Choose an Open Source License](https://choosealicense.com)
* [GitHub Emoji Cheat Sheet](https://www.webpagefx.com/tools/emoji-cheat-sheet)
* [Malven's Flexbox Cheatsheet](https://flexbox.malven.co/)
* [Malven's Grid Cheatsheet](https://grid.malven.co/)
* [Img Shields](https://shields.io)
* [GitHub Pages](https://pages.github.com)
* [Font Awesome](https://fontawesome.com)
* [React Icons](https://react-icons.github.io/react-icons/search)
* [PetaPoco](https://github.com/CollaboratingPlatypus/PetaPoco)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

