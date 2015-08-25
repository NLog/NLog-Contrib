NLog Contrib
============

Community contributions to [NLog](https://github.com/NLog/NLog/).

* NLog Contrib: 
[![Version](https://img.shields.io/nuget/v/NLog.Contrib.svg)](https://www.nuget.org/packages/NLog.Contrib)
[![AppVeyor](https://img.shields.io/appveyor/ci/Xharze/nlog-Contrib/master.svg)](https://ci.appveyor.com/project/Xharze/nlog-Contrib/branch/master)


The repository contains:

- The MappedDiagnosticsLogicalContext (MDLC)
- MdlcLayoutRenderer: `${mdlc}`
- TraceActivityIdLayoutRenderer: System.Diagnostics trace correlation id. `${activityid}`

How to use
===
.Net 4.0 is required. 

For NLog 4.0+, just install the nuget package.

For older NLog versions, check the [NLog wiki](https://github.com/NLog/NLog/wiki/Extending%20NLog#how-to-use-the-custom-target--layout-renderer).


Moved content
===

The following code was part of this repository, but has been moved to separate repositories:

* [NLog.EWT](https://github.com/NLog/NLog.etw)
* [NLog.ManualFlush](https://github.com/NLog/NLog.ManualFlush)

