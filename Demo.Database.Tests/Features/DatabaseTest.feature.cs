﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.5.0.0
//      SpecFlow Generator Version:3.5.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Demo.Database.Tests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.5.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class DatabaseTestFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "DatabaseTest.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "DatabaseTest", "\tDemonstrates populating and asserting expected data in a SQL Server database", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "DatabaseTest")))
            {
                global::Demo.Database.Tests.Features.DatabaseTestFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Populate and validate a table")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseTest")]
        public virtual void PopulateAndValidateATable()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Populate and validate a table", null, tagsOfScenario, argumentsOfScenario);
#line 4
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Code",
                            "Name",
                            "Description",
                            "Cost",
                            "ListPrice",
                            "CategoryId",
                            "SupplierId",
                            "IsActive",
                            "ReleaseDate",
                            "CreatedOn"});
                table1.AddRow(new string[] {
                            "1",
                            "code-1",
                            "product-1",
                            "desc-1",
                            "101.10",
                            "111.10",
                            "1",
                            "2",
                            "1",
                            "2000-01-01",
                            "2000-02-01"});
                table1.AddRow(new string[] {
                            "2",
                            "code-2",
                            "product-2",
                            "desc-2",
                            "102.10",
                            "112.10",
                            "1",
                            "2",
                            "1",
                            "2000-01-02",
                            "2000-02-02"});
                table1.AddRow(new string[] {
                            "3",
                            "code-3",
                            "product-3",
                            "desc-3",
                            "103.10",
                            "113.10",
                            "2",
                            "1",
                            "1",
                            "2000-01-03",
                            "2000-02-03"});
                table1.AddRow(new string[] {
                            "4",
                            "code-4",
                            "product-4",
                            "",
                            "104.10",
                            "114.10",
                            "2",
                            "1",
                            "0",
                            "2000-01-04",
                            "2000-02-04"});
                table1.AddRow(new string[] {
                            "5",
                            "code-5",
                            "product-5",
                            "[NULL]",
                            "105.10",
                            "115.10",
                            "2",
                            "1",
                            "0",
                            "2000-01-05",
                            "2000-02-05"});
#line 5
testRunner.Given("table with identity columns \'[dbo].[Product]\' contains the data", ((string)(null)), table1, "Given ");
#line hidden
#line 13
testRunner.When("I execute my operation", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id:Key",
                            "Code",
                            "Name",
                            "Description",
                            "Cost:Number",
                            "ListPrice:Number",
                            "CategoryId",
                            "SupplierId",
                            "IsActive:Boolean",
                            "ReleaseDate:DateTime",
                            "CreatedOn:DateTime"});
                table2.AddRow(new string[] {
                            "1",
                            "code-1",
                            "product-1",
                            "desc-1",
                            "101.10",
                            "111.10",
                            "1",
                            "2",
                            "true",
                            "2000-01-01",
                            "2000-02-01"});
                table2.AddRow(new string[] {
                            "2",
                            "code-2",
                            "product-2",
                            "desc-2",
                            "102.10",
                            "112.10",
                            "1",
                            "2",
                            "true",
                            "2000-01-02",
                            "2000-02-02"});
                table2.AddRow(new string[] {
                            "3",
                            "code-3",
                            "product-3",
                            "desc-3",
                            "103.10",
                            "113.10",
                            "2",
                            "1",
                            "true",
                            "2000-01-03",
                            "2000-02-03"});
                table2.AddRow(new string[] {
                            "4",
                            "code-4",
                            "product-4",
                            "",
                            "104.10",
                            "114.10",
                            "2",
                            "1",
                            "false",
                            "2000-01-04",
                            "2000-02-04"});
                table2.AddRow(new string[] {
                            "5",
                            "code-5",
                            "product-5",
                            "[NULL]",
                            "105.10",
                            "115.10",
                            "2",
                            "1",
                            "false",
                            "2000-01-05",
                            "2000-02-05"});
#line 15
testRunner.Then("table \'[dbo].[Product]\' should contain the data", ((string)(null)), table2, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Populate and validate a table 2")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseTest")]
        public virtual void PopulateAndValidateATable2()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Populate and validate a table 2", null, tagsOfScenario, argumentsOfScenario);
#line 23
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Code",
                            "Name",
                            "Description",
                            "Cost",
                            "ListPrice",
                            "CategoryId",
                            "SupplierId",
                            "IsActive",
                            "ReleaseDate",
                            "CreatedOn"});
                table3.AddRow(new string[] {
                            "1",
                            "code-1",
                            "product-1",
                            "desc-1",
                            "101.10",
                            "111.10",
                            "1",
                            "2",
                            "1",
                            "2000-01-01",
                            "2000-02-01"});
                table3.AddRow(new string[] {
                            "2",
                            "code-2",
                            "product-2",
                            "desc-2",
                            "102.10",
                            "112.10",
                            "1",
                            "2",
                            "1",
                            "2000-01-02",
                            "2000-02-02"});
                table3.AddRow(new string[] {
                            "3",
                            "code-3",
                            "product-3",
                            "desc-3",
                            "103.10",
                            "113.10",
                            "2",
                            "1",
                            "1",
                            "2000-01-03",
                            "2000-02-03"});
                table3.AddRow(new string[] {
                            "4",
                            "code-4",
                            "product-4",
                            "",
                            "104.10",
                            "114.10",
                            "2",
                            "1",
                            "0",
                            "2000-01-04",
                            "2000-02-04"});
                table3.AddRow(new string[] {
                            "5",
                            "code-5",
                            "product-5",
                            "[NULL]",
                            "105.10",
                            "115.10",
                            "2",
                            "1",
                            "0",
                            "2000-01-05",
                            "2000-02-05"});
#line 24
testRunner.Given("table with identity columns \'[dbo].[Product]\' contains the data", ((string)(null)), table3, "Given ");
#line hidden
#line 32
testRunner.When("I execute my operation", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id:Key",
                            "Code",
                            "Name",
                            "Description",
                            "Cost:Number",
                            "ListPrice:Number",
                            "CategoryId",
                            "SupplierId",
                            "IsActive:Boolean",
                            "ReleaseDate:DateTime"});
                table4.AddRow(new string[] {
                            "1",
                            "code-1",
                            "product-1",
                            "desc-1",
                            "101.10",
                            "111.10",
                            "1",
                            "2",
                            "true",
                            "2000-01-01"});
                table4.AddRow(new string[] {
                            "2",
                            "code-2",
                            "product-2",
                            "desc-2",
                            "102.10",
                            "112.10",
                            "1",
                            "2",
                            "true",
                            "2000-01-02"});
                table4.AddRow(new string[] {
                            "3",
                            "code-3",
                            "product-3",
                            "desc-3",
                            "103.10",
                            "113.10",
                            "2",
                            "1",
                            "true",
                            "2000-01-03"});
                table4.AddRow(new string[] {
                            "4",
                            "code-4",
                            "product-4",
                            "",
                            "104.10",
                            "114.10",
                            "2",
                            "1",
                            "false",
                            "2000-01-04"});
                table4.AddRow(new string[] {
                            "5",
                            "code-5",
                            "product-5",
                            "[NULL]",
                            "105.10",
                            "115.10",
                            "2",
                            "1",
                            "false",
                            "2000-01-05"});
#line 34
testRunner.Then("table \'[dbo].[Product]\' should contain the data", ((string)(null)), table4, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
