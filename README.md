# OnspringAttachmentReporter

A tool built for helping [Onspring](https://onspring.com/) customers report on the attachments contained within an Onspring app. The tool will produce a report in the form of a `.csv` file that contains information about each attachment in the app.

_**Note:**_ This is an unofficial Onspring tool. It was not built in consultation with Onspring Technologies LLC or a member of their development team. This tool was developed independently using Onspring's existing [.NET SDK](https://github.com/onspring-technologies/onspring-api-sdk) for the [Onspring API](https://api.onspring.com/swagger/index.html).

## API Key

This tool makes use of version 2 of the Onspring API. Therefore you will need an API Key to be able to utilize the tool. API keys may be obtained by an Onspring user with permissions to at least read API Keys for your instance, using the following instructions:

- Within Onspring, navigate to /Admin/Security/ApiKey.
- On the list page, add a new API Key (requires Create permissions) or click an existing API Key to view its details.
- On the details page for an API Key, click on the Developer Information tab.
- Copy the X-ApiKey Header value from this section.

_**Important:**_

- An API Key must have a status of Enabled in order to be used.
- Each API Key must have an assigned Role. This role controls the permissions for requests that are made by this tool to retrieve files from fields on records in an app. If the API Key does not have sufficient permissions the attachment will not be downloaded.

### Permission Considerations

You can think of any API Key as another user in your Onspring instance and therefore it is subject to all the same permission considerations as any other user when it comes to it's ability to access a file. The API Key you use with this tool need to have all the correct permissions within your instance to access the record where a file is held and the field where the file is held. Things to think about in this context are `role permissions`, `content security`, and `field security`.

## API Usage Considerations

This tool uses version 2 of the Onspring API to report on the attachments within an app. Currently this version of the Onspring API does not provide any endpoints to perform bulk operations for retrieving attachments or information about attachments.

This tool will make a number of api requests to Onspring in order to collect all the fields in the app, collect all the files that are held in attachment/image fields in all the records in the app, and to finally collect the information for each one of these files.

Expressed as an equation the number of api requests would be something like this:

```text
⌈(totalFieldsInApp / 50)⌉ + ⌈(totalRecordsInApp / 50)⌉ + (totalAttachmentFieldsInApp * numOfFilesPerAttachmentField * totalRecordsInApp)
```

As an example if you were to use this tool to report on an app that...

- contains only 1 attachment field and 1 image field
- contains 10 records
- contains 1 file per attachment/or image field

...you would end up making 22 requests.

```text
numOfRequests = ⌈(2 / 50)⌉ + ⌈(10 / 50)⌉ + (2 * 1 * 10)
numOfRequests = 1 + 1 + 20
numOfRequests = 2 + 20
numOfRequests = 22
```

This all being shared because it's important you take into consideration the number of requests it is going to take to report on your attachments. If the quantity is quite considerable I'd encourage you to consult with your Onspring representative to understand what if any limits there are to your usage of the Onspring API.

## Installation

The tool is published as a release where you can download it as a single executable file for the following operating systems:

- win-x64
- linux-x64
- osx-x64 (Minimum OS version is macOS 10.12 Sierra)
  - Note after downloading the executable you may need to run `chmod +x` to give the executable execute permissions on your machine.
  - Note after downloading the executable you may need to provide permission for the application to run via the your systems settings.

You are also welcome to clone this repository and run the tool using the [.NET 7](https://dotnet.microsoft.com/en-us/download) tooling and runtime. As well as modify the tool further for your specific needs.

## General Usage

When starting the tool you will need to provide it with an `Api Key` and an `App Id` which it will use to report on the attachments contained within the given `App Id`.

These pieces of information can be provided in two different ways.

You can provide them as command-line arguments:

```sh
OnspringAttachmentReporter.exe -k 000000ffffff000000ffffff/00000000-ffff-0000-ffff-000000000000 -a 100
```

Or you can provide them in a `.json` configuration file:

```sh
OnspringAttachmentReporter.exe -c config.json
```

Example `config.json`:

```json
{
  "ApiKey": "000000ffffff000000ffffff/00000000-ffff-0000-ffff-000000000000",
  "AppId": "100"
}
```

_**Note:**_ If a configuration file is specified in addition to an Api Key and App Id the Api Key and App Id arguments will take precedence over the values in the configuration file.

### Obtaining Configuration Values

- `Api Keys` can be obtained as outlined in the [API Key](#api-key) section.
- `App Ids` can be obtained...
  - by using the [Onspring API's](https://api.onspring.com/swagger/index.html) [/Apps](/Apps) endpoint
  - by looking at the url of the app in your browser.
    ![app-id-url-example.png](/README/Images/app-id-url-example.png)

## Options

This tool currently has a number of options that can be passed as command-line arguments to alter the behavior of the tool. Theres are detailed below and can also be viewed by passing the `-h` or `--help` option to the tool.

- **Configuration File:** `--config` or `-c`
  - Allows you to specify a path to a `.json` file which contains the necessary configuration values the tool needs to run as outlined in the [General Usage](#general-usage) section.
  - **Example usage:** `OnspringAttachmentTransferrer.exe -c config.json`
- **Api Key:** `--key` or `-k`
  - Allows you to specify an Api Key to use when making requests to the Onspring API.
  - **Example usage:** `OnspringAttachmentTransferrer.exe -k 000000ffffff000000ffffff/00000000-ffff-0000-ffff-000000000000`
- **App Id:** `--app` or `-a`
  - Allows you to specify an App Id to report on.
  - **Example usage:** `OnspringAttachmentTransferrer.exe -a 100`
- **Log Level:** `--log` or `-l`
  - Allows you to specify what the minimum level of event that will be written to the console while the tool is running.
  - By default this will be set to the `Information` level.
  - The valid levels are: `Debug` | `Error` | `Fatal` | `Information` | `Verbose` | `Warning`
  - **Example usage:** `OnspringAttachmentTransferrer.exe -l Debug`

## Output

Each time the tool runs it will generate a new folder that will be named based upon the time at which the tool began running and the word `output` in the following format: `YYYYMMDDHHMM-output`. All files generated by the tool during the run will be saved into this folder.

Example Output Folder Name:

```text
202301212223-output
```

### Report

The tool will generate a report that contains information about the attachments that were found in the app. This report will be saved to the output folder as a `.csv` file. The report will contain the following information about each attachment:

- **Record Id:** The id of the record that the attachment is attached to.
- **Field Id:** The id of the field that the attachment is attached to.
- **Field Name:** The name of the field that the attachment is attached to.
- **File Id:** The id of the file.
- **File Name:** The name of the file.
- **File Size:** The size of the file in bytes.
- **File Size (KB):** The size of the file in kilobytes.
- **File Size (KiB):** The size of the file in kibibytes (1024 bytes).
- **File Size (MB):** The size of the file in megabytes.
- **File Size (MiB):** The size of the file in mebibytes (1024 kibibytes).
- **File Size (GB):** The size of the file in gigabytes.
- **File Size (GiB):** The size of the file in gibibytes (1024 mebibytes).

### Log

In addition to the information the tool will log out to the console as it is running a log file will also be written to the output folder that contains information about the completed run. This log can be used to review the work done and troubleshoot any issues the tool may have encountered during the run. Please note that each log event is written in [Compact Log Event Format](http://clef-json.org/). You are welcome to parse the log file in the way that best suits your needs.

Various tools are available for working with the CLEF format.

- [Analogy.LogViewer.Serilog](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog) - CLEF parser for Analogy Log Viewer
- [clef-tool](https://github.com/datalust/clef-tool) - a CLI application for processing CLEF files
- [Compact Log Format Viewer](https://github.com/warrenbuckley/Compact-Log-Format-Viewer) - a cross-platform viewer for CLEF files
- [Seq](https://datalust.co/seq) - import, search, analyze, and export application logs in the CLEF format
- [seqcli](https://github.com/datalust/seqcli) - pretty-print CLEF files at the command-line

Example log messages:

```json
{"@t":"2023-02-27T20:17:51.9459465Z","@mt":"Starting Onspring Attachment Reporter."}
{"@t":"2023-02-27T20:17:51.9549566Z","@mt":"Retrieving file fields."}
{"@t":"2023-02-27T20:17:53.1594346Z","@mt":"File fields retrieved. {Count} file fields found.","Count":5}
{"@t":"2023-02-27T20:17:53.1621809Z","@mt":"Retrieving files that need to be requested."}
{"@t":"2023-02-27T20:17:53.1639144Z","@mt":"Retrieving records for page {PageNumber}.","@l":"Debug","PageNumber":1}
{"@t":"2023-02-27T20:17:53.4718000Z","@mt":"Records retrieved for page {PageNumber}. {Count} records found.","@l":"Debug","PageNumber":2,"Count":2}
{"@t":"2023-02-27T20:17:53.4783245Z","@mt":"Files retrieved. {Count} files found.","Count":19}
{"@t":"2023-02-27T20:17:53.4785630Z","@mt":"Retrieving information for each file."}
{"@t":"2023-02-27T20:17:55.9330508Z","@mt":"File info retrieved for {Count} of {Total} files.","Count":19,"Total":19}
{"@t":"2023-02-27T20:17:55.9332692Z","@mt":"Start writing attachments report."}
{"@t":"2023-02-27T20:17:56.0590647Z","@mt":"Finished writing attachments report:"}
{"@t":"2023-02-27T20:17:56.0592461Z","@mt":"Onspring Attachment Reporter finished."}
{"@t":"2023-02-27T20:17:56.0595578Z","@mt":"You can find the log and report files in the output directory: {OutputDirectory}","OutputDirectory":"C:\\SoftwareProjects\\OnspringAttachmentReporter\\src\\bin\\Debug\\net7.0\\202302271417-output"}
```
