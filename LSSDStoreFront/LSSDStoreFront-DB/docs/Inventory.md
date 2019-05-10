Inventory
=========

An __Inventory__ object represents all of the product inventory in the store, as well as all the product categories.

## Creating an Inventory object
First, set up a database context object.
```c#
// set up a database connection
string connectionString = "Don't put your connection string in your code";
DatabaseContext dbConnection = new DatabaseContext(connectionString);
```
Then create a new inventory object with your database connection.
```c#
// Set up a connection to the shop inventory
Inventory Inventory = new Inventory(dbConnection);
```

## Get a list of all product categories
```c#
Inventory.AllCategories
```
```c#
List<ProductCategory> allCategories = Inventory.AllCategories;
```

Example:
```c#
// Get a list of all product categories
List<ProductCategory> allCategories = Inventory.AllCategories;
Console.WriteLine("All categories:");
foreach(ProductCategory cat in allCategories)
{
    Console.WriteLine(" " + cat.Name);
}
```

## Get a list of all top level categories only
```c#
Inventory.TopLevelCategories
```
```c#
List<ProductCategory> allCategories = Inventory.TopLevelCategories;
```
Example:
```c#
// Get a list of all top level categories
List<ProductCategory> topLevelCategories = Inventory.TopLevelCategories;
Console.WriteLine("Top level categories: ");
foreach(ProductCategory cat in topLevelCategories)
{
    Console.WriteLine(" " + cat.Name);
}
```

## Get a list of all items in a category
```c#
// By specifying category ID
List<Product> items = Inventory.ItemsFromCategory(1);
```

```c#
// By specifying a category object
ProductCategory myCategory = Inventory.Category(1);
List<Product> items = Inventory.ItemsFromCategory(myCategory);
```
Example:
```c#
// Get a list of all items in a category
foreach(ProductCategory cat in allCategories)
{
    Console.WriteLine("Category: " + cat.Name);
    List<Product> thisCategoryProducts = Inventory.ItemsFromCategory(cat);
    foreach(Product product in thisCategoryProducts)
    {
        Console.WriteLine("  " + product.Price + "\t" + product.Name);
    }
}
```

## Get a specific category by it's ID number
```c#
// Get a specific category by ID number
ProductCategory categoryById = Inventory.Category(2);
Console.WriteLine("Category by Id: " + categoryById.Name);
```

## Get a specific category by name
If a category doesn't exist, you will get an empty category instead.
```c#
// Get a specific category by Name
ProductCategory categoryByName = Inventory.Category("This category doesn't exist");
Console.WriteLine("Category by name: " + categoryByName.Name);
```

## Get all child categories for a given category
Child categories are stored in the `.ChildCategories` collection.

```c#
// Children categories
foreach(ProductCategory cat in categoryById.ChildCategories)
{
    Console.WriteLine("  Child: " + cat.Name);
}
```