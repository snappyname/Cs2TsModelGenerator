# Advanced .cs to .ts DTO models generator using Roslyn

## Convert c# dto to ts frontend models

### How to use:
1) Download latest realize
2) Exstract zip to folder
3) Create config.json file for settings
4) Run ModelGenerator with path to config.json

`ModelGenerator.exe config.json`

or create .bat app (for windows)

```bat  
@echo off  
set PARAM=config.json  
ModelGenerator2.exe "%PARAM%"  
```  

or, for linux

```bash  
#!/usr/bin/env bash  
PARAM="config.json"  
./ModelGenerator "$PARAM"  
```  

### Requirements / Limits
* All DTO's must be in one project
* Not supported records and interfaces
* Generic classes
* Classes that inherited from interfaces with fields (from classes/abstract classes - OK)

### Supported data types

| cs                 | ts                                        |
| ------------------ | ----------------------------------------- |
| `int`, `long`      | `number`                                  |
| `string`           | `string`                                  |
| `bool`             | `boolean`                                 |
| `T`                | `T` *(your data types)                    |
| `List<T>`          | `T[]`                                     |
| `List<List<T>>`    | `T[][]`                                   |
| `T[]`              | `T[]`                                     |
| `Dictionary<T, V>` | `Record<T, V>`                            |
| `enums`            | `any` (but can be changed on config.json) |
Some abstract types like `IList`/`IDictionary`/`IEnumerable` and others are not supported and will not be

### Config.json settings

| Value                           | Meaning                            | Example                                                                          |
| ------------------------------- | ---------------------------------- | -------------------------------------------------------------------------------- |
| `solutionPath` (string)         | path to solution (.sln file)       | `C:\\DEV\\TemplateWebApi\\TemplateWebApi.sln`                                    |
| `outputPath` (string)           | path to output models              | `C:\\DEV\\TemplateWebAuth\\models\\`                                             |
| `targetProjectName` (string)    | name of project that include dto's | `DTO`                                                                            |
| `fieldLowerCase` (bool)         | name of result fields              | `SomeValue` -> `someValue` (cs -> ts)                                            |
| `classNameLowerCase` (bool)     | name of result class               | `SomeValueModel` -> `someValueModel` (cs -> ts)                                  |
| `classFileNameType` (number)    | name of result file                | See below                                                                        |
| `classFileNameLowerCase` (bool) | case of result file                | Same as `classNameLowerCase`                                                     |
| `enumType` (string)             | enum's map type                    | All enums will be converted to your string; I recommend to use `any` or `number` |

### classFileNameType examples

| value | example                                                                             |
| ----- | ----------------------------------------------------------------------------------- |
| 1     | `TestModel.cs` => `Test.model.ts` or `Test.cs` => `Test.model.ts`                   |
| 2     | `TestModel.cs` => `TestModel.ts` or `Test.cs` => `Test.ts`                          |
| 3     | `TestUserModel.cs` => `Test-User.model.ts` or `TestUser.cs` => `Test-User.model.ts` |
It also works with `classNameLowerCase`. On examples `classNameLowerCase = false`


### config.json example

```
{  
  "solutionPath": "C:\\DEV\\TemplateWebApi\\TemplateWebApi.sln",  
  "outputPath": "C:\\DEV\\TemplateWebAuth\\models\\",  
  "targetProjectName": "DTO",  
  "fieldLowerCase": true,  
  "classNameLowerCase": false,  
  "classFileNameType": 3,  
  "classFileNameLowerCase": true,  
  "enumType": "any"  
}
```


### Custom attributes

This generator is compatible with [GeneratorAttributes (Github)](https://github.com/snappyname/GeneratorAttributes) [nuget](https://www.nuget.org/packages/snappyname.FronendGeneratorAttributes)

| Attribute                   | Explain                                                                                                                  |
| --------------------------- | ------------------------------------------------------------------------------------------------------------------------ |
| `CustomFieldGenerationName` | Can be added on field to override name of result (ts file). The settings from config.json are not applied to this field. |
| `GeneratorIgnore`           | Can be added to field or class. Generator will be ignore this field or class                     

---

### TODO (maybe)

* Trim build files
* Add `CustomFileGenerationName` attribute
* Add `CustomClassGenerationName` attribute

---
## Feel free to contribute or open issue if something wrong

 