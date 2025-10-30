using BugStore.Models;
using GetResponse = BugStore.Responses.Customers.Get;
using GetByIdResponse = BugStore.Responses.Customers.GetById;
using CreateRequest = BugStore.Requests.Customers.Create;
using CreateResponse = BugStore.Responses.Customers.Create;
using UpdateRequest = BugStore.Requests.Customers.Update;
using UpdateResponse = BugStore.Responses.Customers.Update;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// "Banco" em memória
var customers = new List<Customer>();

app.MapGet("/", () => "Hello World!");

app.MapGet("/v1/customers", () =>
{
    var response = customers.Select(c => new GetResponse
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

    var response = new GetByIdResponse
    {
        Id = customer.Id,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        BirthDate = customer.BirthDate
    };
    return Results.Ok(response);
});
app.MapPost("/v1/customers", (CreateRequest request) =>
{
    var customer = new Customer
    {
        Name = request.Name,
        Email = request.Email,
        Phone = request.Phone,
        BirthDate = request.BirthDate
    };
    customers.Add(customer);

    var response = new CreateResponse
    {
        Id = customer.Id
    };

    return Results.Created($"/v1/customers/{customer.Id}", response);
});
app.MapPut("/v1/customers/{id:guid}", (Guid id, UpdateRequest request) =>
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

    var response = new UpdateResponse
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
    return Results.Ok("Hello World!");
});
app.MapGet("/v1/products/{id}", () =>
{
    return Results.Ok("Hello World!");
});
app.MapPost("/v1/products", () =>
{
    return Results.Ok("Hello World!");
});
app.MapPut("/v1/products/{id}", () =>
{
    return Results.Ok("Hello World!");
});
app.MapDelete("/v1/products/{id}", () =>
{
    return Results.Ok("Hello World!");
});

app.MapGet("/v1/orders/{id}", () =>
{
    return Results.Ok("Hello World!");
});
app.MapPost("/v1/orders", () =>
{
    return Results.Ok("Hello World!");
});

app.Run();
