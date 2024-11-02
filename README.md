# OdooJsonRpcClient

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Intechnity-com/OdooJsonRpcClient/blob/master/LICENSE.md)
[![NuGet package](https://img.shields.io/nuget/v/PortaCapena.OdooJsonRpcClient?color=blue&logo=NuGet&label=NuGet%20Package)](https://www.nuget.org/packages/PortaCapena.OdooJsonRpcClient)
[![Nuget](https://img.shields.io/nuget/dt/PortaCapena.OdooJsonRpcClient?logo=NuGet&label=Downloads)](https://www.nuget.org/packages/PortaCapena.OdooJsonRpcClient)
[![example workflow](https://github.com/Intechnity-com/OdooJsonRpcClient/actions/workflows/pr_build.yml/badge.svg)](https://github.com/Intechnity-com/OdooJsonRpcClient/actions/workflows/pr_build.yml)
[![example workflow](https://github.com/Intechnity-com/OdooJsonRpcClient/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/Intechnity-com/OdooJsonRpcClient/actions/workflows/codeql-analysis.yml)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/patricoos/54596b8c061e5c3c25b3072822a105a0/raw/code-coverage.json)

OdooJsonRpcClient is a C# library (.NET Standard) for communication with Odoo.


## Installation

Use the package manager console, nuget package manager or check on
[NuGet.org](https://www.nuget.org/packages/PortaCapena.OdooJsonRpcClient/)

```bash
Install-Package PortaCapena.OdooJsonRpcClient
```




## First steps

Start your work with check version of Odoo. To this request You need only a valid url address.
```C#
var config = new OdooConfig(
        apiUrl: "https://odoo-api-url.com", //  "http://localhost:8069"
        dbName: "odoo-db-name",
        userName: "admin",
        password: "admin"
        );

var odooClient = new OdooClient(config);
var versionResult = await odooClient.GetVersionAsync();

```

When U can connect to odoo, try use login method. (There is no need to use this method later. Logging is going on in the background e.g. `OdooClient`, `OdooRepository`)
```C#
var loginResult = await odooClient.LoginAsync();
```

If You have correct data to login, its time to get model that U intrested off.

```C#
var tableName = "product.product";
var modelResult = await odooClient.GetModelAsync(tableName);

var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
```
Method `GetModelAsync` returns model with odoo specification.
Method `GetDotNetModel()` from class `OdooModelMapper` returns string of class declaration that U can create and paste to Your project. Please don't use the models from the example project. Models are different depending on odoo version and added extensions.

```
[OdooTableName("product.product")]
[JsonConverter(typeof(OdooModelConverter))]
public class OdooProductProduct : IOdooModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

    [JsonProperty("price")]
    public double? Price { get; set; }

    // product.template
    [JsonProperty("product_tmpl_id")]
    public int ProductTmplId { get; set; }
    
    [JsonProperty("activity_exception_decoration")]
    public ActivityExceptionDecorationOdooEnum? ActivityExceptionDecoration { get; set; }
    
    ...
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ActivityExceptionDecorationOdooEnum
{
    [EnumMember(Value = "warning")]
    Alert = 1,

    [EnumMember(Value = "danger")]
    Error = 2,
}
```











## OdooRepository
#### Read
```C#
var repository = new OdooRepository<ProductProductOdooModel>(config);
var products = await repository.Query().ToListAsync();
```

In Repository U can use `OdooQueryBuilder` to create queries. 
```C#
  var products = await repository.Query()
                .Where(x => x.Name, OdooOperator.EqualsTo, "test product name")
                .Where(x => x.WriteDate, OdooOperator.GreaterThanOrEqualTo, new DateTime(2020, 12, 2))
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.WriteDate
                })
                .OrderByDescending(x => x.Id)
                .Take(10)
                .ToListAsync();
```


#### Create
```C#
var model = OdooDictionaryModel.Create(() => new ProductProductOdooModel()
{
    Name = "test product name"
});

var result = await repository.CreateAsync(model);
```

#### Update
U can update only this fields that U are intrested in. For updating many records use `UpdateRangeAsync`.
```C#           
var model = OdooDictionaryModel.Create(() => new ProductProductOdooModel()
{
    Name = "test product name updated"
}); 
var result = await repository.UpdateAsync(model, productId);
```

#### Delete
```C#                        
var deleteProductResult = await repository.DeleteAsync(productId);
```













## OdooClient
OdooRepository is a wrapper for OdooClient. This class give option for building more advanced requests or not implemented in this library.



## IOdooCreateModel
If we create an object instance that we want to pass to odoo but do not fill all fields, they will be automatically assigned default values (Create and update methods). In this case is impossible to distinguish whether we want to set null value or not touch this property. The first solution for that is create models based on odoo model with only fields that we want to use. To that use `IOdooCreateModel` interface. To not create multiple models use `OdooDictionaryModel`.

```C#                        
[OdooTableName("product.product")]
public class OdooCreateProduct : IOdooCreateModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
}
```

```C#                        
var model = new OdooCreateProduct()
{
     Name = "Prod test Kg",
};

var createResult = await repository.CreateAsync(model);
```






## OdooDictionaryModel
This class is second solution for the problem of passing models with default or null values to odoo.

```C#                        
var model = OdooDictionaryModel.Create(() => new ProductProductOdooModel()
{
    Name = "test name",
    Description = "test description",
});

if(condition)
    model.Add(x => x.WriteDate, new DateTime());

var createResult = await odooRepository.CreateAsync(model);
```

or 
```C#                        
 var model2 = new OdooDictionaryModel("res.partner") {
                { "name", "test name" },
                { "country_id", 20 },
                { "city", "test city" },
                { "zip", "12345" },
                { "street", "test address" },
                { "company_type", "company" },
            };
var odooClient = new OdooClient(TestConfig);

var createResult = await odooClient.CreateAsync(model2);
```




## OdooContext
Context is mainly used to send to odoo values such as **language** or **time zone**, but U can use it for more advanced requests. U can set it in `OdooConfig` for multiple usage or only once adding it to request as parameter (also in repository query).

```C#  
var context = new OdooContext("pl_PL");
context.Language = "nl_BE";

var id = await odooRepository.CreateAsync(model, context);
``` 



## Advanced queries
To make Your queries faster use `Select` method and get only fields that U are interested of. If U reduced the amount of field in your models use `SelectSimplifiedModel()`.


### OdooFilter
For more advanced queries U can use `OdooFilter` or OdooFilterOfT. To use `or` try:
```C#
var filter = OdooFilter<ResCompanyOdooModel>.Create()
                .Or()
                .EqualTo(x => x.Name, "My Company (San Francisco)")
                .EqualTo(x => x.Name, "PL Company");    

var filter = OdooFilter.Create()
                .Or()
                .EqualTo("name", "My Company (San Francisco)")
                .EqualTo("name", "PL Company");

var products = await repository.Query()
               .Where(filter)
               .ToListAsync();
```

### Deep where
Get ProductProductOdooModel where CountryCode is "BE" in ResCompanyOdooModel (CompanyId)
```C#
var repository = new OdooRepository<ProductProductOdooModel>(config);
var products = await repository.Query()
               .Where<ResCompanyOdooModel>(
                  x => x.CompanyId, 
                     y => y.CountryCode, OdooOperator.EqualsTo, "BE")
               .FirstOrDefaultAsync();
```

```C#
var products = await repository.Query()
               .Where<ResCompanyOdooModel, AccountTaxOdooModel>(
                  x => x.PropertyAccountExpenseId, 
                     y => y.AccountSaleTaxId, 
                        z => z.CountryCode, OdooOperator.EqualsTo, "BE")
               .FirstOrDefaultAsync();
```

## Custom Http Message Handler
If U want to use custom `HttpMessageHandler`, U can use `OdooClientHttp.Configure` before creating `OdooClient` instance. This could be done on your startup class.

These custom handlers can be used for logging odoo requests and responses, adding headers or other customizations.

When configuring the client, you can add multiple handlers. The order in which you add them is the order in which they will be executed.

```C#
OdooHttpClient.Configure(opt =>
{
   opt.AddHttpMessageHandler(new LoggingHandler());
   //you can add more custom handlers like so
   //opt.AddHttpMessageHandler(new CustomHandler());
});

var config = new OdooConfig(
        apiUrl: "https://odoo-api-url.com", //  "http://localhost:8069"
        dbName: "odoo-db-name",
        userName: "admin",
        password: "admin"
        );

var odooClient = new OdooClient(config);
var versionResult = await odooClient.GetVersionAsync();

/** Output **
Made a request at: 7/19/2024 7:11:28PM
Got a response at: 7/19/2024 7:11:32PM
*/

// You can introduce ILogger to the constructor
public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine("Made a request at: {0}", DateTime.UtcNow);
        var response = await base.SendAsync(request, cancellationToken);
        Console.WriteLine("Got a response at: {0}", DateTime.UtcNow);
        return response;
    }
}
```


## Odoo Request and Result models examples
#### Request 
```json
{
   "id":948165322,
   "jsonrpc":"2.0",
   "method":"call",
   "params":{
      "service":"object",
      "method":"execute",
      "args":[
         "odoo_db_name",
         2,
         "odoo_user_name",
         "product.product",
         "search_read",
         [
            [
               "name",
               "=",
               "Bioboxen 610l"
            ],
            [
               "write_date",
               ">=",
               "2020-12-02 00:00:00"
            ]
         ],
         [
            "name",
            "description",
            "write_date"
         ]
      ]
   }
}
```

#### Result
```json
{
   "jsonrpc":"2.0",
   "id":948165322,
   "result":[
      {
         "id":324,
         "name":"Bioboxen 610l",
         "description":false,
         "write_date":"2020-12-02 08:47:08"
      }
   ]
}
```



## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.
