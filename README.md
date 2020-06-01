# x-PATeSCO - Cross-Platform App Test Script Recorder

x-PATeSCO is a software tool that proposes mechanisms to produce automated tests for cross-platform mobile apps. 

If you use this tool, we gently ask you to cite our publication:
```
@article{iet-sen/MenegassiE20,
  author    = {Andre Augusto Menegassi and Andr{\'{e}} Takeshi Endo},
  title     = {Automated tests for cross-platform mobile apps in multiple configurations},
  journal   = {{IET} Softw.},
  volume    = {14},
  number    = {1},
  pages     = {27--38},
  year      = {2020}
}
```

<h1>Folders</h1>

Each folder is described as follows. 

<b>x-PATeSCO (source) </b>- it contains the source files for the prototype tool. The tool has been developed in C# and its project can be opened using Microsoft Visual Studio 2017.

<b>x-PATeSCO (bin) </b> - binary files and dependencies to run the tool; we tested it with Windows 10. It requires an Appium server (http://appium.io/) installed and running; the mobile device must be accessible through the Appium server. The server's address and port are set up within the tool. 

<b>Experimental Data/AppProject</b> - for each app, we provide the project source folder (used to build the .apk for Android and .app for iOS). We make available seven out of nine apps tested in the paper since two apps are from IT partners and are not open source. 
 
<b>Experimental Data/TestProject</b> - for each app the test project (for Microsoft Visual Studio 2017) that runs and collects the data about the eight locating strategies discussed in the paper. 

<b>Experimental Data/data.csv</b> - it contains the raw data for the experimental study conducted in the paper. This file summarizes pieces of information for all executions, initially recorded in JSON files.

We believe all files needed to replicate the experiments are herein available. 

Best regards,

André Augusto Menegassi - andremenegassi@hotmail.com 

Andre Takeshi Endo - andreendo@gmail.com

UTFPR - Campus Cornelio Procopio - Brazil
