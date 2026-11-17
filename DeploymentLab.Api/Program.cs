
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// 1. Đọc connection string từ appsettings.
var connectionString =
    builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException(
        "Missing PostgreSQL connection string");

// 2. Đăng ký NpgsqlDataSource vào DI container.
builder.Services.AddSingleton(
    NpgsqlDataSource.Create(connectionString));

var app = builder.Build();

// 3. Health Check API.
app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        service = "DeploymentLab.Api"
    });
});

// 4. API đọc danh sách sản phẩm từ PostgreSQL.
app.MapGet("/api/products", async (
    NpgsqlDataSource dataSource) =>
{
    var products = new List<Product>();

    // Tạo SQL command.
    await using var command = dataSource.CreateCommand(
        "SELECT id, name, price FROM products ORDER BY id");

    // Thực thi câu SQL.
    await using var reader =
        await command.ExecuteReaderAsync();

    // Đọc từng dòng dữ liệu.
    while (await reader.ReadAsync())
    {
        var product = new Product(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetDecimal(2)
        );

        products.Add(product);
    }

    return Results.Ok(products);
});

app.Run();

record Product(int Id, string Name, decimal Price);
