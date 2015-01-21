NLog Contrib
============

Community contributions to NLog.

##Adding a new target

It’s easy. Just put the target in a DLL and reference it from the the config file using the clause as described here.

Configuration file example:
```xml
<nlog> 
  <extensions> 
    <add assembly="MyAssembly"/> 
  </extensions> 
  <targets> 
    <target name="a1" type="MyFirst" host="localhost"/> 
  </targets> 
  <rules> 
    <logger name="*" minLevel="Info" appendTo="a1"/> 
  </rules> 
</nlog>
```

You can also use `TargetFactory.AddTarget()` to register your target programmatically.
Just be sure to do it at the very beginning of your program before any log messages are written.
It should be possible to reference your EXE using the ```<extensions />``` clause.

```csharp
static void Main(string[] args) 
{ 
    ConfigurationItemFactory.Default.Targets.RegisterDefinition("MyFirst", typeof(MyNamespace.MyFirstTarget));
 
    // start logging here 
}
```
