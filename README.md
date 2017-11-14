# EFCoreTools

## Introduction

EFCoreTools is a small library designed to help developers trying to make the jump from EF6 to EFCore from having to recreate many of their data annotations to FluentAPI. The annotations are applied using "Conventions" that are performed by overriding `OnModelCreating` and adding a few lines of code or by inheriting from EFCoreToolsDbContext.

## Available Functionality

### Set Indexes using `IndexAttribute`

To continue to use existing `IndexAttribute` data annotations, add `using EFCoreTools.Attributes` to the list of usings in your data model file(s). 

## TODO

- Create EFCoreToolsDbContext