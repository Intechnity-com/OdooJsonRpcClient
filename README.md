# PortaCapena.OdooJsonRpcClient

OdooJsonRpcClient is a C# library (.NET Standard) for communication with Odoo.

[Porta Capena - Odoo Partner](https://www.odoo.com/partners/porta-capena-3710126)



## Installation

Use the package manager console, nuget package manager or check on
[NuGet.org](https://www.nuget.org/packages/PortaCapena.OdooJsonRpcClient/)

```bash
Install-Package PortaCapena.OdooJsonRpcClient
```




## Usage

Start your work with check version of Odoo. To this request You only need a valid url address.
```C#
var config = new OdooConfig(
        apiUrl: "https://odoo-api-url.com",
        dbName: "odoo-db-name",
        userName: "admin",
        password: "admin"
        );

var odooClient = new OdooClient(config);
var versionResult = await odooClient.GetVersionAsync();

```

In next step try to login.
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
Method `GetDotNetModel()` from class `OdooModelMapper` returns string of class declaration that U can create and paste to Your project.

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
var repository = new OdooRepository<OdooProductProduct>(config);
var products = await repository.Query().ToListAsync();
```

In Repository U can use `OdooQueryBuilder`. 
```C#
  var products = await repository.Query()
                .Where(x => x.Barcode, OdooOperator.EqualsTo, "barcodetest1")
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.WriteDate
                })
                .OrderByDescending(x => x.Id)
                .Take(10)
                .FirstOrDefaultAsync();
```


#### Create
```C#
var odooClient = new OdooClient(config);

var model = new OdooCreateProduct()
{
     Name = "Prod test Kg",
};

var result = await repository.CreateAsync(model);
```

#### Update
```C#            
var result = await repository.UpdateAsync(productId, model);
```

#### Delete
```C#                        
var deleteProductResult = await repository.DeleteAsync(productId);
```













## OdooClient
OdooRepository is a wrapper for OdooClient. This class give option for building more advanced requests or not implemented in this library.



## IOdooCreateModel
If we create an object instance that we want to pass to odoo but do not fill all fields, they will be automatically assigned default values (Create and update methods). In this case is impossible to distinguish whether we want to set null value or not touch this property. The first solution for that is create models based on odoo model with only fields that we want to use. To that use IOdooCreateModel interface.

```C#                        
[OdooTableName("product.product")]
public class OdooCreateProduct : IOdooCreateModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
}
```





## OdooDictionaryModel
This class is second solution for the problem of passing models with default or null values to odoo.

```C#                        
var model = OdooDictionaryModel.Create(() => new PurchaseOrderLineOdooModel()
{
    Name = "test name",
    DateOrder = new DateTime(),
});

if(condition)
    model.Add(x => x.CreateDate, new DateTime());


var id = await odooRepository.CreateAsync(model);
```





## OdooContext
Context is mainly used to send to odoo values such as **language** or **time zone**, but U can use it for more advanced requests. U can set it in **OdooConfig** for multiple usage or only once adding it to request as parameter (also in repository query).

```C#  
var context = new OdooContext("pl_PL");
context.Language = "nl_BE";

var id = await odooRepository.CreateAsync(model, context);
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
