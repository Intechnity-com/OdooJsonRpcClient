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
    
    ...
    
    // Type of the exception activity on record.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityExceptionDecorationOdooEnum
    {
        [EnumMember(Value = "warning")]
        Alert = 1,

        [EnumMember(Value = "danger")]
        Error = 2,
    }
```




## CRUD
#### Read
```C#
var products = await odooClient.GetAsync<OdooProductProduct>();
```
#### Create
```C#
var odooClient = new OdooClient(config);

var model = new OdooCreateProduct()
{
     Name = "Prod test Kg",
     UomId = 3,
     UomPoId = 3
};

var result= await odooClient.CreateAsync(model);
```

#### Update
```C#            
var result = await odooClient.UpdateAsync(model, productId);
```

#### Delete
```C#                        
var deleteProductResult = await odooClient.DeleteAsync("product.product"), createdProductId);
```





## Repository
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
     UomId = 3,
     UomPoId = 3
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
