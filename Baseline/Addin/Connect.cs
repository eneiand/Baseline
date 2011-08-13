using System;
using System.Collections.Generic;
using System.IO;
using Baseline.TypeAnalysis;
using EnvDTE90;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Addin
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
			if(connectMode == ext_ConnectMode.ext_cm_UISetup)
			{
				object []contextGUIDS = new object[] { };
				Commands2 commands = (Commands2)_applicationObject.Commands;

				//Place the command on the project context menu
				Microsoft.VisualStudio.CommandBars.CommandBar projectContextMenu = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["Project"];

                CommandBarPopup baseLinePopup = (CommandBarPopup)projectContextMenu.Controls.Add(MsoControlType.msoControlPopup, 
                    System.Reflection.Missing.Value, System.Reflection.Missing.Value, 1, true );

			    baseLinePopup.Caption = "Baseline";


                CommandBarControl generateTestsControl =
                baseLinePopup.Controls.Add(MsoControlType.msoControlButton,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, 1, true);
                // Set the caption of the submenuitem
                generateTestsControl.Caption = "Generate Unit Tests";


                m_generateTestsHandler = (CommandBarEvents)_applicationObject.Events.get_CommandBarEvents(generateTestsControl);
                m_generateTestsHandler.Click += GenerateTestsClick;

			}
		}


        private List<Project> GetSelectedProjects()
        {
            var selectedProjects = new List<Project>();
            Array projects = this._applicationObject.ActiveSolutionProjects as Array;
            foreach (var project in projects)
            {
                selectedProjects.Add(project as Project);
            }

            return selectedProjects;
        }

        private Project AddTestProject(Project projectToBeTested)
        {
            var s = this._applicationObject.Solution as Solution3;
            string templatePath = s.GetProjectTemplate("ClassLibrary.zip", "CSharp");
            String testProjectName = projectToBeTested.Name + ".Tests";
            s.AddFromTemplate(templatePath, projectToBeTested.FileName.Substring(0, projectToBeTested.FileName.LastIndexOf('\\') + 1) + testProjectName, testProjectName, false);

            VSLangProj.VSProject addedProject = null;

            foreach (var p in s.Projects)
            {
                if ((p as Project).Name == testProjectName)
                    addedProject = (p as Project).Object as VSLangProj.VSProject;
            }

            (addedProject).References.AddProject(projectToBeTested);
            return addedProject.Project;
            
        }

        private Assembly GetAssembly(Project project)
        {

            var outputFile = project.Properties.Item("OutputFileName").Value as String;
            var outputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value as String;
            var assemblyFile = Path.Combine(project.FullName.Substring(0, project.FullName.LastIndexOf("\\") + 1),
                                            outputPath, outputFile);
            return Assembly.LoadFrom(assemblyFile);
        }

        protected void GenerateTestsClick(object CommandaBarControl,
                           ref bool handled, ref bool cancelDefault)
        {

            foreach (var selectedProject in this.GetSelectedProjects())
            {
              this._applicationObject.Solution.SolutionBuild.BuildProject(_applicationObject.Solution.SolutionBuild.ActiveConfiguration.Name,
                selectedProject.UniqueName, true);

              if (this._applicationObject.Solution.SolutionBuild.LastBuildInfo == 0)
              {
                  var testProject = this.AddTestProject(selectedProject);
                  var testAssembly = this.GetAssembly(selectedProject);

                  var tester = new Tester();

                  foreach (var type in testAssembly.GetTypes())
                  {
                      var testSuite = tester.GenerateTests(type);

                  }
              }

            }

            //Array projects = this._applicationObject.ActiveSolutionProjects as Array;
            
            //foreach (Object item in projects)
            //{
            //    var project = item as Project;

            //   var prop= project.Properties.Item("OutputFileName");

            //   foreach (var p in project.Properties)
            //   {
            //       Console.WriteLine((p as Property).Value);
            //   }


            //    var configprops = project.ConfigurationManager.ActiveConfiguration.Properties;
            //    var outpath = configprops.Item("OutputPath");

            //    foreach (var configprop in configprops)
            //    {
            //        Console.WriteLine((configprop as Property).Value);
            //    }
            //}
            
        }

	    /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if(commandName == "Addin.Connect.Addin")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
					return;
				}
			}
		}

		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if(commandName == "Addin.Connect.Addin")
				{
					handled = true;
					return;
				}
			}
		}
		private DTE2 _applicationObject;
		private AddIn _addInInstance;
	    private CommandBarEvents m_generateTestsHandler;
	}
}