using BugStore.Models;
using GetCustomerResponse = BugStore.Responses.Customers.Get;
using GetCustomerByIdResponse = BugStore.Responses.Customers.GetById;
using CreateCustomerRequest = BugStore.Requests.Customers.Create;
using CreateCustomerResponse = BugStore.Responses.Customers.Create;
using UpdateCustomerRequest = BugStore.Requests.Customers.Update;
using UpdateCustomerResponse = BugStore.Responses.Customers.Update;
using GetProductResponse = BugStore.Responses.Products.Get;
using GetProductByIdResponse = BugStore.Responses.Products.GetById;
using CreateProductRequest = BugStore.Requests.Products.Create;
using CreateProductResponse = BugStore.Responses.Products.Create;
using UpdateProductRequest = BugStore.Requests.Products.Update;
using UpdateProductResponse = BugStore.Responses.Products.Update;
using GetOrderByIdResponse = BugStore.Responses.Orders.GetById;
using CreateOrderRequest = BugStore.Requests.Orders.Create;
using CreateOrderResponse = BugStore.Responses.Orders.Create;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// "Banco" em memória
var customers = new List<Customer>();
var products = new List<Product>();
var orders = new List<Order>();

app.MapGet("/", () => "Hello World!");

app.MapGet("/v1/customers", () =>
{
    var response = customers.Select(c => new GetCustomerResponse
    {
        Id = c.Id,
        Name = c.Name,
        Email = c.Email,
        Phone = c.Phone,
        BirthDate = c.BirthDate
    });

    return Results.Ok(response);
});
app.MapGet("/v1/customers/{id:guid}", (Guid id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null) return Results.NotFound();

    var response = new GetCustomerByIdResponse
    {
        Id = customer.Id,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        BirthDate = customer.BirthDate
    };
    return Results.Ok(response);
});
app.MapPost("/v1/customers", (CreateCustomerRequest request) =>
{
    var customer = new Customer
    {
        Name = request.Name,
        Email = request.Email,
        Phone = request.Phone,
        BirthDate = request.BirthDate
    };
    customers.Add(customer);

    var response = new CreateCustomerResponse
    {
        Id = customer.Id
    };

    return Results.Created($"/v1/customers/{customer.Id}", response);
});
app.MapPut("/v1/customers/{id:guid}", (Guid id, UpdateCustomerRequest request) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer is null)
        return Results.NotFound();

    var oldCustomer = new
    {
        customer.Name,
        customer.Email,
        customer.Phone,
        customer.BirthDate
    };

    customer.Name = request.Name;
    customer.Email = request.Email;
    customer.Phone = request.Phone;
    customer.BirthDate = request.BirthDate;

    var response = new UpdateCustomerResponse
    {
        OldName = oldCustomer.Name,
        OldEmail = oldCustomer.Email,
        OldPhone = oldCustomer.Phone,
        OldBirthDate = oldCustomer.BirthDate,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        BirthDate = customer.BirthDate
    };

    return Results.Ok(response);
});

app.MapDelete("/v1/customers/{id:guid}", (Guid id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer is null) return Results.NotFound();

    customers.Remove(customer);
    return Results.NoContent();
});

app.MapGet("/v1/products", () =>
{
    var response = products.Select(p => new GetProductResponse
    {
        Id = p.Id,
        Title = p.Title,
        Description = p.Description,
        Slug = p.Slug,
        Price = p.Price
    });

    return Results.Ok(response);
});
app.MapGet("/v1/products/{id:guid}", (Guid id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null) return Results.NotFound();

    var response = new GetProductByIdResponse
    {
        Id = product.Id,
        Title = product.Title,
        Description = product.Description,
        Slug = product.Slug,
        Price = product.Price
    };
    return Results.Ok(response);
});
app.MapPost("/v1/products", (CreateProductRequest request) =>
{
    var product = new Product
    {
        Id = Guid.NewGuid(),
        Title = request.Title,
        Description = request.Description,
        Slug = request.Slug,
        Price = request.Price
    };
    products.Add(product);

    var response = new CreateProductResponse
    {
        Id = product.Id
    };

    return Results.Created($"/v1/products/{product.Id}", response);
});
app.MapPut("/v1/products/{id:guid}", (Guid id, UpdateProductRequest request) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null) return Results.NotFound();

    var oldProduct = new
    {
        product.Title,
        product.Description,
        product.Slug,
        product.Price
    };

    product.Title = request.Title;
    product.Description = request.Description;
    product.Slug = request.Slug;
    product.Price = request.Price;

    var response = new UpdateProductResponse
    {
        OldTitle = oldProduct.Title,
        OldDescription = oldProduct.Description,
        OldSlug = oldProduct.Slug,
        OldPrice = oldProduct.Price,
        NewTitle = product.Title,
        NewDescription = product.Description,
        NewSlug = product.Slug,
        NewPrice = product.Price
    };

    return Results.Ok(response);
});
app.MapDelete("/v1/products/{id:guid}", (Guid id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null) return Results.NotFound();

    products.Remove(product);
    return Results.NoContent();
});

app.MapGet("/v1/orders/{id:guid}", (Guid id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null) return Results.NotFound();

    var response = new GetOrderByIdResponse
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        CustomerName = order.Customer?.Name,
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt,
        Total = order.Lines.Sum(ol => ol.Total),
        Lines = [.. order.Lines.Select(ol => new OrderLine
        {
            Id = ol.Id,
            OrderId = ol.OrderId,
            Quantity = ol.Quantity,
            Total = ol.Total,
            ProductId = ol.ProductId,
            Product = ol.Product
        })]
    };

    return Results.Ok(response);
});
app.MapPost("/v1/orders", (CreateOrderRequest request) =>
{
    var order = new Order
    {
        Id = Guid.NewGuid(),
        CustomerId = request.CustomerId
    };

    order.Lines.AddRange(request.Lines.Select(ol => new OrderLine
    {
        Id = Guid.NewGuid(),
        Quantity = ol.Quantity,
        Total = ol.Total,
        ProductId = ol.ProductId,
        Product = ol.Product
    }));

    orders.Add(order);

    var response = new CreateOrderResponse
    {
        Id = order.Id
    };

    return Results.Created($"/v1/orders/{order.Id}", response);
});

app.Run();
