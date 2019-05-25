# Getting started

Steps for creating a Test project that uses Specflow tests for testing a WebApi.

This tutorial uses Visual Studio 2019 Community with the extension SpecFlow for Visual Studio 2019.

To install SpecFlow for Visual Studio 2019 extension select Extensions->Manage Extension menu, search the extension by the name and complete the steps to install it.

![Extensions](specflow-extension.png)


1.	Create a new test project in Visual Studio. This tutorial uses a MSTest Test Project  (.NET Core) but it is possible to use other types of test projects.

![Create New Project](create-new-project.png)


2.	Enter the name MyApi.Tests for the project.

![Configure New Project](configure-new-project.png)


3.	After the project is created remove the test that was created with the template. UnitTest1.cs.


4.	Add NuGet packages SpecFlow.Tools.MsBuild.Generation, Helpers.Specflow.Steps.WebApi, and SpecFlow.MsTest to the project.

![SpecFlow.Tools.MsBuild.Generation](SpecFlow.Tools.MsBuild.Generation.png)

![Helpers.Specflow.Steps.WebApi](Helpers.Specflow.Steps.WebApi.png)

![SpecFlow.MsTest](SpecFlow.MsTest.png)

Specflow has different packages for different test frameworks, e.g. SpecFlow.xUnit and SpecFlow.NUnit.


5.	Add a new class called MyApiSteps.cs

![Steps File](add-steps-file.png)

Replace the text

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.WebApi;
using TechTalk.SpecFlow;

namespace MyApi.Tests
{
    [Binding]
    [Scope(Feature = "MyApiSteps")]
    public class MyApiSteps : WebApiSpecs
    {
        private static readonly WebApiSpecsConfig _config = new WebApiSpecsConfig { BaseUrl = "https://reqres.in" };
        public MyApiSteps(TestContext testContext) : base(testContext, _config) { }
    }
}
```
