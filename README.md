[![Docker Cloud Automated build](https://img.shields.io/docker/cloud/automated/carlpaton/vulnuscloud?logoColor=brightgreen)](https://hub.docker.com/r/carlpaton/vulnuscloud) [![Build Status](https://travis-ci.com/carlpaton/VulnusCloud.svg?branch=master)](https://travis-ci.com/carlpaton/VulnusCloud)

# Vulnus Cloud

> Vulnerability comes from the Latin word for "wound," vulnus. Vulnerability is the state of being open to injury, or appearing as if you are.                                                                                            *- vocabulary.com*
>

### About

This application allows you to identify open source dependencies and determine if there are any known, publicly disclosed, vulnerabilities on packages used by your application.

This works by calling the public service at https://ossindex.sonatype.org/ which uses data derived from public sources so its worth checking out their warnings, disclaimers and rate limiting processes.

### Usage

Locally you can run `VulnusCloud\Docker-VulnusCloud\run.ps1` which will use docker compose to setup the environment. Its probably a good idea to add a parameter to also be able to build from source instead of pulling the compiled `carlpaton/vulnuscloud` image...

Parameter `-Reset` will tear down all the infrastructure and start from scratch. 

Then access the UI from http://localhost:8080/ the steps would then be

1. Create your project(s)
2. Upload packages file (see supported packages below)
3. Reporting
   1. Note that the OSS Index API has rate limiting, so if you see `Too Many Requests` the application will automagically retry.

#### Home Page

![Example Home Page](https://raw.githubusercontent.com/carlpaton/VulnusCloud/master/Docker-VulnusCloud/example%20home%20page.jpg)

#### Docker Image

Master branch is built and available to pull from docker hub.

* https://hub.docker.com/r/carlpaton/vulnuscloud

```xc
docker pull carlpaton/vulnuscloud
```

### Reporting

Basic reporting to screen should be fine for now, dumping to .XLSX or .PDF shouldn't be too hard, from the below you would click the the project, then the version uploaded (stored by date) and finally into the actual vulnerabilities.

![Example Reporting Page](https://raw.githubusercontent.com/carlpaton/VulnusCloud/master/Docker-VulnusCloud/example%20reporting%20page.jpg)

### Supported Packages

| Eco System | Type or location                         | File Format/Name      |
| ---------- | ---------------------------------------- | --------------------- |
| NuGET      | packages.config (Legacy)                 | packages.config       |
| NuGET      | Package Reference in project file (.Net) | [project name].csproj |
| NPM        | \packages\AppName\web\client             | package.json          |

### References

* https://ossindex.sonatype.org/
* https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files
* https://docs.microsoft.com/en-us/nuget/reference/packages-config
* https://www.vocabulary.com/dictionary/vulnerability
* https://shields.io/