# Geta.EPi.Find.Extensions

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_EPiFindExtensions_EPiFindExtensionsDebug)/statusIcon)
[![Platform](https://img.shields.io/badge/Platform-.NET 4.5.2-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/Episerver-%2010-orange.svg?style=flat)](http://world.episerver.com/cms/)


Extension methods for EPiServer Find.

## Examples

### Conditional filtering
Adds an easy way to add filters based on a condition.
```csharp
int? someId = ....;
var searchResult = client.Search<any>()
                .Filter(x => x.ExampleProp1.Match(true))
				// Only apply filter if someId has a value
                .ConditionalFilter(someId.HasValue, x => x.ExampleProp2.Match(someId.Value))
				.Filter(x => x.ExampleProp3);
```

### TermsFacetFor on int / number properties
Retrieve TermsFacet for int properties instead of string properties.
Useful if you want to have the termfacet for a given id property (where the id is an int obviously).
```csharp
public class Test {
	public virtual string StringProp { get; set; }
	public virtual int IntProp { get; set; }
}

var searchResult = client.Search<Test>()
                .Take(0)
                .TermsFacetFor(x => x.StringProp)
				// TermsFacetFor only exists for string properties
				// The actual api works with ints (numbers) as well
				.TermsFacetFor(x => x.IntProp);
```

### TermsFacetFor result size (take)
Being able to set result size for termfacet, easily discoverable by IntelliSense.
```csharp

var resultSize = 1000;
var searchResult = client.Search<Test>()
                .Take(0)
				// Easily add resultSize to TermsFacetFor
                .TermsFacetFor(x => x.IntProp, resultSize)
				
				// Resharper/IntelliSense does not like this notation too much
				// And you obviously want to use it on int properties as well right!?
				.TermsFacetFor(x => x.StringProp, r => r.Size = resultSize);
```