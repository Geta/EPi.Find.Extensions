## Geta.EPi.Find.Extensions

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_EPiFindExtensions_EPiFindExtensionsDebug)/statusIcon)
[![Platform](https://img.shields.io/badge/Platform-.NET%204.6.1-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/Episerver-%2011-orange.svg?style=flat)](http://world.episerver.com/cms/)

## Description
Extension methods for EPiServer Find.

## Features
- Conditional filtering: Adds an easy way to add filters based on a condition.
- Terms Facet: Retrieve TermsFacet for int properties instead of string properties.
- Wildcards: Allows you to perform queries with wildcards.

## Examples

### Conditional filtering
```csharp
int? someId = ....;
var searchResult = client.Search<any>()
                .Filter(x => x.ExampleProp1.Match(true))
				// Only apply filter if someId has a value
                .Conditional(someId.HasValue, r => r.Filter(x => x.ExampleProp2.Match(someId.Value)))
				.Filter(x => x.ExampleProp3);
```

### TermsFacetFor on int / number properties
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

### Wildcards
```csharp
return typeSearch.For(query, stringQuery =>
            {
                stringQuery.Query = AddWildcards(stringQuery.Query.ToString());
                stringQuery.AllowLeadingWildcard = allowLeadingWildcard;
                stringQuery.AnalyzeWildcard = analyzeWildCard;
                stringQuery.FuzzyMinSim = fuzzyMinSim;
            });
```
			
### Wildcards
```csharp
var searchResult = SearchClient.Instance.Search<ArticlePage>()
                .ForWithWildcards(searchQuery, (x => x.Title, 1.5), (x => x.Name, 0.5))
                .GetContentResultSafe();

```

### Handle Client and Service exceptions

Makes it easy to return an empty results instead of an error, useful in case find is unstable/down. See [this](https://world.episerver.com/blogs/Jonas-Bergqvist/Dates/2016/12/exceptions-in-find/) and [this](https://www.brianweet.com/2017/03/17/handling-find-serviceexception.html)
```csharp
// Throws exception
 var contentResult = search
        .GetContentResult(cacheForSeconds, cacheForEditorsAndAdmins);
// Returns empty result in case of ClientException or ServiceException
 var contentResult = search
        .GetContentResultSafe(cacheForSeconds, cacheForEditorsAndAdmins);

```

## Package Maintainer
https://github.com/DigIntSys

## Changelog
[Changelog](CHANGELOG.md)
