[![Build Status](https://travis-ci.com/carlpaton/VulnusCloud.svg?branch=master)](https://travis-ci.com/carlpaton/VulnusCloud)

# Vulnus Cloud

> Vulnerability comes from the Latin word for "wound," vulnus. Vulnerability is the state of being open to injury, or appearing as if you are.                                                                                            *- vocabulary.com*
>

### About

This application allows you to identify open source dependencies and determine if there are any known, publicly disclosed, vulnerabilities on packages used by your application.

This works by calling the public service at https://ossindex.sonatype.org/ which uses data derived from public sources so its worth checking out their warnings, disclaimers and rate limiting processes.

### Usage

The intended targeted platform would be `docker compose` via PowerShell script. However if useful to an organization this can be hosted using any container orchestration tools. 

#### Containers

The following images are used by default:

* carlpaton/vulnuscloud
* [microsoft/mssql-server-linux:2017-CU13](https://hub.docker.com/r/microsoft/mssql-server-linux)
* [boxfuse/flyway:5.1](https://hub.docker.com/r/boxfuse/flyway/)

#### Technical Implementation

The following is based on the API inputs at ssindex.sonatype.org

* https://ossindex.sonatype.org/doc/coordinates
* (Swagger :D) https://ossindex.sonatype.org/rest

1. Select lookup type:

a. File; So upload `packages.config` or `[PROJECT].csproj` file which assumes `type=nuget`
b. Name; the name of the component you wish to lookup along with its version. Type selection is also needed. Example: npm, nuget ect.

This is then deserialized to `Business.Model.PackageModel` or `Business.Model.PackagesConfigFileModel`

2. Supply your project name which is used for report grouping
3. Check local database for `coordinates` record

Example: `pkg:nuget/System.Net.Http@4.3.1`

4. Check `HasExpired` date field is less than a month old. If not return those results.
5. Call ossindex.sonatype.org/api for new data

Example: GET https://ossindex.sonatype.org/api/v3/component-report/pkg:nuget/System.Net.Http@4.3.1

6. Database result if applicable and return results.

#### Reporting

Basic reporting to screen should be fine for now, dumping to .XLSX or .PDF shouldn't be too hard.

### References

* https://ossindex.sonatype.org/
* https://www.vocabulary.com/dictionary/vulnerability