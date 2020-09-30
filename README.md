# nRETS

[![Actions Status](https://github.com/behm/nrets/workflows/build-and-test/badge.svg)](https://github.com/behm/nrets/actions)
[![NuGet](http://img.shields.io/nuget/v/nrets.svg)](https://www.nuget.org/packages/nRETS/)

## What is nRETS?
nRETS is a .NET Standard Library used to read real estate data from RETS servers.  The goal is to make a library that works better with .NET Framework and .NET Core code.  It abstracts away the HTTP communication and payload parsing of the data so you can use it for your application.

## Status of the project
This is just getting started and we are starting from the ground up so while it's not fully functional yet, bear with us a new features are added.  

If you are a brave soul and would like to help test, it would be much appreciated.  Working with more RETS servers will only make this library better.

## How do I get started
*** sample code coming ***

This project uses .NET secrets to configure login credentials for you development environment.  To use it, run the following commands while in your project folder and replace the values with yours:

- dotnet user-secrets set "LoginUrl" "your-rets-login-url"
- dotnet user-secrets set "Username" "your-username"
- dotnet user-secrets set "Password" "your-password"
- dotnet user-secrets set "UserAgent" "your-useragent"

You can also supply your values in the appsettings.json file.

## Where can I get it?
New releases will be published to NuGet as they are available

## Feature requests
Obviously the main focus at first is to work on the basics but if you have a feature request, please post an issue.

