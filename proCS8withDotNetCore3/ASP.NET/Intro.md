# Introducing ASP.NET Core

ASP.NET Core, the latest version of the web development framework 
that uses C# and .NET Core. 

- Microsoft released ASP.NET MVC in 2007 to great success

<br>

## Introducing the MVC Pattern

### The Model

- The Model is the **data** of your application.
- One way to think about models and view models is to relate them to **database tables and database views**.
- Academically, models should be extremely clean and not contain validation or any other business rules.

### The View

- **user interface** of the application
- **accept commands**, **render their results** to the user
- should hand off all work **to the controller**

### The Controller

- the **brains of the application**
- take commands/requests from the user **via the View**

<br>

## ASP.NET Core and .NET Core

- rewrite of the popular ASP.NET framework.
- This opened the door for ASP.NET Core applications to use a cross-platform, lightweight, fast, and open 
source web server called **KesTreL**. Kestrel presents a uniform development experience across all platforms.

### One Framework, Many Uses

With ASP.NET Core, you can build applications that use **Razor Pages**, the **Model-View-Controller**
pattern, **RESTful services**, and SPA applications using **JavaScript frameworks** like Angular and React. While 
the UI rendering varies with choices between the **MVC pattern** and JavaScript frameworks, the underlying 
development framework is the same across all choices. Two prior choices that have not been carried forward 
into ASP.NET Core are WebForms and WCF.

<br>

## ASP.NET Core Features from MVC/WebAPI

design goals and features

- Convention over Configuration
- Controllers and Actions
- Model Binding
- Model Validation
- Routing
- Filters
- Layouts and Razor Views


### **Naming Conventions**

- controllers are typically named with the “Controller” suffix (e.g. HomeController)
- Editor and Display templates are named after 
the class that they render in the view.

### **Directory Structure**

#### The Controllers Folder

- By convention, the Controllers folder is where the ASP.NET Core MVC and API implementations (and the 
routing engine) expect that **the controllers for your application are placed**.

#### The Views Folder

- The Views folder is where the **views for the application are stored**. Each controller gets its own folder under 
the main Views folder named after the controller name (minus the Controller suffix). The action methods 
will render views in their controller’s folder by default. For example, the Views/Home folder holds all the 
views for the HomeController controller class.

#### **The Shared Folder**

- A **special folder** under Views is named Shared. This folder is accessible to all controllers and their action 
methods. After searching the folder named for the controller, **if the view can’t be found**, then the Shared 
folder is searched for the view.

#### The wwwroot Folder (New in ASP. NET Core)

- **client-side is all contained** under the wwwroot
folder. This significantly cleans up the project structure when working with ASP.NET Core.

### Controllers and Actions

#### **The Controller Class**

The Controller class provides a host of **helper methods** for web applications. The most commonly used 
methods:

| Helper Method | Meaning in Life
| ------------- | ----------------
| `ViewDataTempDataViewBag` | Provide data to the view through the ViewDataDictionary, TempDataDictionary, and dynamic ViewBag transport.
| `View` | Returns a ViewResult (derived from ActionResult) as the HTTP response. Defaults to view of the same name is the action method, with the option of specifying a specific view. All options allow specifying a ViewModel that is strongly typed and sent to the View. (?????)
| `PartialView` | Returns a PartialViewResult to the response pipeline. 
| `ViewComponenet` | Returns a ViewComponentResult to the response pipeline.
| `Json` | Returns a JsonResult containing an object serialized as JSON as the response.
| `OnActionExecuting` | Executes before an action method executes.
| `OnActionExecutionAsync` | Async version of OnActionExecuting.
| `OnActionExecuted` | Executes after an action method executes.

#### **The ControllerBase Class**

The ControllerBase class provides the core functionality for both ASP.NET Core web applications and 
services, in addition to **helper methods** for returning HTTP status codes.

| Helper Method | Meaning in Life
| ------------- | ----------------
| `HttpContext` | Returns the HttpContext for the currently executing action.
| `Request` | Returns the HttpRequest for the currently executing action.
| `Response` | Returns the HttpResponse for the currently executing action.
| `RouteData` | Returns the RouteData for the currently executing action
| `ModelState` | Returns the state of the model in regard to model binding and validation
| `Url` | Returns an instance of the IUrlHelper, providing access to building URLs for ASP.NET Core MVC applications and services
| `User` | Returns the ClaimsPrincipal User. |
| `Content` | Returns a ContentResult to the response. Overloads allow for adding content type and encoding definition.
| `File` | Returns a FileContentResult to the response.
| `Redirect` | A series of methods that redirect the user to another URL by returning a RedirectResult.
| `LocalRedirect` | A series of methods that redirect the user to another URL only if the URL is local. More secure than the generic Redirect methods.
| `RedirectToAction` <br> `RedirectToPage` <br> `RedirectToRoute` | A series of methods that redirect to another action method, Razor Page, or named route.
| `TryUpdateModel` | Explicit model binding
| `TryValidateModel` | Explicit model validation

<br>

*Some of the HTTP Status Code Helper Methods Provided by the ControllerBase Class*

| Helper Method | HTTP Status Code Action Result | Status Code
| ------------- | ------------------------------ | ------------
| `NoContent` | NoContentResult | 204
| `Ok` | OkResult | 200
| `NotFound` | ...Result | 404
| `BadRequest` | .. | 400
| `Created ` | .. | 201
| `CreatedAtAction` | .. | 201
| `CreatedAtRoute` | .. | 201
| `Accepted` | .. | 202
| `AcceptedAtAction` | .. | 202
| `AcceptedAtRoute` | .. | 202

### Actions

Actions are methods on a controller that return an IActionResult (or Task<IActionResult> for async 
operations) or a class that implements IActionResult, such as ActionResult or the ViewResult.

### Model Binding

Model binding is the process where ASP.NET Core takes the name/value pairs available and attempts to 
reconstitute the action parameter types using reflection and recursion. 

[ in process.. ]

<br>
<br>
<br>
<br>
<br>
