# What is this?

It is an experiment to see if .NET Compact Framework WinForms apps can
use the XAML-like markup for constructing controls.

The main app uses a form that is dynamically instantiated from markup.
This makes it easy to preprocess forms (e.g. add more validations,
localization, etc.)  while retaining backwards compatibility with .NET
CF WinForms.

It should be noted that based on this, one could create an environment
for form design and compile-time checking (a LINQPad for WinForms of
sorts, free of VS2008 absurdities).

# Usage

On desktop, the application performs the following functions:

* user selects an XML file to view
* when the file is loaded, the result is displayed on screen, and user can

  * browse the XAML element tree (selecting subnodes)
  * view and edit the properties of selected node
  * modify the selected XML file (the view reloads automatically)

On mobile, the application simply loads the provided XML file and puts it
on screen.

# Screenshots

Same [form](TestView.xml), different devices:

[Windows XP](screenshots/TestXP.png)
[Windows Mobile 6](screenshots/TestWM.png)

# Credits

* controls from https://github.com/KarinoTaro/ListBoxEx
* XAML parser & converters from [MobileUI](http://mobileui.codeplex.com/)
