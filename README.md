# PlantsIdentifier

[![Build Status](https://tiagodenoronha.visualstudio.com/PlantsIdentifier/_apis/build/status/PlantsIdentifier-CI)](https://tiagodenoronha.visualstudio.com/PlantsIdentifier/_build/latest?definitionId=4)
[![CodeFactor](https://www.codefactor.io/repository/github/tiagodenoronha/plantsidentifier/badge)](https://www.codefactor.io/repository/github/tiagodenoronha/plantsidentifier)
[![codecov](https://codecov.io/gh/tiagodenoronha/PlantsIdentifier/branch/master/graph/badge.svg)](https://codecov.io/gh/tiagodenoronha/PlantsIdentifier)

I recently adopted a cat and found out there are some plants that are dangerous for him! So i've set out to build a mobile App which leverages Azure Custom Vision. 

It is a Xamarin.Forms App, with a .NET Core support Backend. 
All connections from the App pass through the backend to be able to monitor it a bit better.

[![Bugs](https://img.shields.io/github/issues/tiagodenoronha/PlantsIdentifier.svg)](https://github.com/tiagodenoronha/PlantsIdentifier/issues?utf8=✓&q=is%3Aissue+is%3Aopen+label%3Abug)
[![Twitter](https://img.shields.io/twitter/url/https/github.com/tiagodenoronha/PlantsIdentifier/.svg?style=social)](https://twitter.com/intent/tweet?text=This%20is%20awesome!&url=https%3A%2F%2Fgithub.com%2Ftiagodenoronha%2FPlantsIdentifier%2F)

## Getting Started

You can build and run the project from command line using:

1. Clone this repository to your local machine.
```
$ git clone https://github.com/tiagodenoronha/PlantsIdentifier.git
```
2. Move to the project directory.
```
$ cd PlantsIdentifier/src/PlantsIdentifierAPI
```
3. Restore packages.
```
$ dotnet restore
```
4. Initialize the database.
```
$ dotnet ef database update
```
5. Run the project.
```
$ dotnet run
```

## Contributing

First of all, thank you to everyone who contributes!

[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/0)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/0)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/1)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/1)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/2)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/2)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/3)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/3)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/4)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/4)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/5)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/5)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/6)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/6)[![](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/images/7)](https://sourcerer.io/fame/tiagodenoronha/tiagodenoronha/PlantsIdentifier/links/7)

If you are interested in fixing issues and contributing directly to the code base, be my guest! Just fork the repo and do your magic! :)
See the list of [contributors](https://github.com/tiagodenoronha/PlantsIdentifier/contributors) who participated in this project.

Please see also the [Code of Conduct](CODE_OF_CONDUCT.md).

## Feedback

* Vote for [popular feature requests](https://github.com/tiagodenoronha/PlantsIdentifier/issues?q=is%3Aopen+is%3Aissue+label%3Afeature-request+sort%3Areactions-%2B1-desc).
* File a bug in [GitHub Issues](https://github.com/tiagodenoronha/PlantsIdentifier/issues).
* [Tweet](https://twitter.com/noronhat) me with other feedback!

### References
This ReadMe was based on the [VSCode](https://github.com/Microsoft/vscode/)'s ReadMe.

## License

Copyright (c) Tiago Noronha. All rights reserved.

Licensed under the [MIT](LICENSE) License.
